using System;
using NXOpen;
using NXOpen.UF;

public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUfSession;
    public static Program theProgram;
    public static bool isDisposeCalled;
    public static bool test1;  // This is checking for merging conflict from newbranch to main

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

            Objects_on_layer();

            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
        return retValue;
    }

    public static void Objects_on_layer()
    {
        try
        {
            Tag wrkprttag = theSession.Parts.Work.Tag;
            Tag tobj = Tag.Null;
            ListingWindow lw = theSession.ListingWindow;
            lw.Open();
            do
            {
                tobj = theUfSession.Obj.CycleAll(wrkprttag, tobj);
                NXOpen.Utilities.NXObjectManager theobject = theSession.GetObjectManager();
                TaggedObject thetagobject = theobject.GetTaggedObject(tobj);
                if (thetagobject is DisplayableObject)
                {
                    DisplayableObject displ_object = (DisplayableObject)thetagobject;
                    if (displ_object.Layer == 5 && !(displ_object is Edge) && !(displ_object is Face))
                    {
                        lw.WriteFullline("Displayable object in the layer : 5 is ==> " + displ_object.JournalIdentifier);
                    }
                }

            } while (tobj!= null);
        }
        catch (Exception)
        {

            throw;
        }

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

