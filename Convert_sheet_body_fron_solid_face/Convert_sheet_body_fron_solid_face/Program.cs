using System;
using NXOpen;
using NXOpen.UF;

// new branch commit
public class NXJournal
{
    static Session theSession = Session.GetSession();
    static UFSession theUFSession = UFSession.GetUFSession();
    static Part workPart = theSession.Parts.Work;

    public static void Main(string[] args)
    {
        Face[] selectedFaces = SelectFaces("Create sheet body");
        if (selectedFaces.Length == 0)
        {
            Echo("You didn't select any faces - exiting");
            return;
        }
        Session.UndoMarkId Oops = theSession.SetUndoMark( Session.MarkVisibility.Invisible, "Create sheet from faces");

        NXOpen.Features.UnsewBuilder unsewBuilder1;
        unsewBuilder1 = workPart.Features.CreateUnsewBuilder(null);
        NXOpen.FaceDumbRule theFaceRule = workPart.ScRuleFactory.CreateRuleFaceDumb(selectedFaces);
        NXOpen.SelectionIntentRule[] rules1 = { theFaceRule };
        unsewBuilder1.FaceCollector.ReplaceRules(rules1, false);
        unsewBuilder1.KeepOriginal = true;

        NXOpen.Features.Unsew theUnsew = (NXOpen.Features.Unsew)unsewBuilder1.Commit();
        Body[] newSheets = theUnsew.GetBodies();
        unsewBuilder1.Destroy();

        Point testPoint = createFaceMidpoint(selectedFaces[0]);
        Point3d midPoint = testPoint.Coordinates;
        Double[] here = { midPoint.X, midPoint.Y, midPoint.Z };

        theSession.UpdateManager.AddToDeleteList(testPoint);
        NXOpen.Features.RemoveParametersBuilder removeParametersBuilder1 = workPart.Features.CreateRemoveParametersBuilder();
        removeParametersBuilder1.Objects.Add(newSheets);
        removeParametersBuilder1.Commit();
        Body theSheet = null;
        foreach (Body aSheet in newSheets)
        {
            int status;
            theUFSession.Modl.AskPointContainment(here, aSheet.Tag, out status);
            if (status == 1) theSheet = aSheet;
            else theSession.UpdateManager.AddToDeleteList(aSheet);
        }
        theSession.UpdateManager.DoUpdate(Oops);

        //Echo("This is the new Sheet body:");
        //theSession.Information.DisplayObjectsDetails(new DisplayableObject[] { theSheet });
        //Echo("\nThese are its faces:");

        //Face[] sheetFaces = theSheet.GetFaces();
        //theSession.Information.DisplayObjectsDetails(sheetFaces);
        //Echo("no. selected faces = " + selectedFaces.Length);
        //Echo("no. sheet body faces = " + sheetFaces.Length);
        //if (selectedFaces.Length != sheetFaces.Length)
        //{
        //    Echo("The faces you selected were not all connected.");
        //    Echo("This journal only keeps the sheet at the first selected face.");
        //}
    }
    static Point createFaceMidpoint(Face theFace)
    {
        Scalar halfway = workPart.Scalars.CreateScalar(0.5, Scalar.DimensionalityType.None, SmartObject.UpdateOption.WithinModeling);
        Point thePoint = workPart.Points.CreatePoint(theFace, halfway, halfway, SmartObject.UpdateOption.WithinModeling);

        return thePoint;
    }
    public static Face[] SelectFaces(string prompt)
    {
        TaggedObject[] selobj = null;
        Selection.SelectionType[] faces = { Selection.SelectionType.Faces };

        Selection.Response resp = UI.GetUI().SelectionManager.SelectTaggedObjects(prompt, "Select faces", Selection.SelectionScope.AnyInAssembly, false,
        faces, out selobj);

        System.Collections.Generic.List<Face> faceList = new System.Collections.Generic.List<Face>();

        foreach (TaggedObject aTO in selobj)
            if (aTO is Face) faceList.Add((Face)aTO);
        return faceList.ToArray();
    }
    public static void Echo(string output)
    {
        theSession.ListingWindow.Open();
        theSession.ListingWindow.WriteLine(output);
        theSession.LogFile.WriteLine(output);
    }
    public static int GetUnloadOption(string arg)
    {
        return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
    }
}