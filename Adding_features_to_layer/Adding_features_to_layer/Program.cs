using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.Features;

public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUfSession;
    public static Program theProgram;
    public static bool isDisposeCalled;
    public static Part workpart;
    
    // This for new branch checking Test2
    //------------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------------
    public Program()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();
            isDisposeCalled = false;
            workpart = theSession.Parts.Work;

        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            // UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
        }
    }

    //------------------------------------------------------------------------------
    //  Explicit Activation
    //      This entry point is used to activate the application explicitly
    //------------------------------------------------------------------------------
    public static int Main(string[] args)
    {
        int retValue = 0;
        try
        {
            theProgram = new Program();

          Feature[] arrfeatt = collect_feature();

            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
        return retValue;
    }

 public static Feature[] collect_feature()
    {
      Feature[] arrfeat = workpart.Features.ToArray();
        foreach (var item in arrfeat)
        {
            if (item.FeatureType == "DATUM_PLANE")
            {
                int layer = 101;
                theUfSession.Modl.AskFeatObject(item.Tag, out int n_ieds, out Tag[] arrtag);
                 DatumPlane nxobj = (NXOpen.DatumPlane)NXOpen.Utilities.NXObjectManager.Get(arrtag[0]);
                 theUfSession.Obj.SetLayer(nxobj.Tag, layer);
            }

            if (item.FeatureType == "LINE" || item.FeatureType == "ARC")
            {
                int layer = 102;
                theUfSession.Modl.AskFeatObject(item.Tag, out int n_ieds, out Tag[] arrtag);
                Curve nxobj = (NXOpen.Curve)NXOpen.Utilities.NXObjectManager.Get(arrtag[0]);
                theUfSession.Obj.SetLayer(nxobj.Tag, layer);
            }
            if (item.FeatureType == "TEXT")
            {
                int layer = 103;
                theUfSession.Modl.AskFeatObject(item.Tag, out int n_ieds, out Tag[] arrtag);
                for (int i = 0; i < arrtag.Length; i++)
                {
                    Spline nxobj = (NXOpen.Spline)NXOpen.Utilities.NXObjectManager.Get(arrtag[i]);
                    theUfSession.Obj.SetLayer(nxobj.Tag, layer);
                }
            }
        }
        return arrfeat;
    }

   

    //------------------------------------------------------------------------------
    // Following method disposes all the class members
    //------------------------------------------------------------------------------
    public void Dispose()
    {
        try
        {
            if (isDisposeCalled == false)
            {
                //TODO: Add your application code here 
            }
            isDisposeCalled = true;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
    }

    public static int GetUnloadOption(string arg)
    {
        //Unloads the image explicitly, via an unload dialog
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

        //Unloads the image immediately after execution within NX
        return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

        //Unloads the image when the NX session terminates
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }

}

