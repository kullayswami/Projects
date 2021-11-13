
using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.Annotations;
using NXOpen.Assemblies;
using NXOpen.GeometricUtilities;
using System.IO;

public class NXJournal
{
    static Session theSession = Session.GetSession();
    static UFSession theUFSession = UFSession.GetUFSession();
    static Part workPart = theSession.Parts.Work;


    public static void Echo(string output)
    {
        theSession.ListingWindow.Open();
        theSession.ListingWindow.WriteLine(output);
        theSession.LogFile.WriteLine(output);
    }
    static Point3d MapView2Abs(View aView, Point3d loc)
    {
        Matrix3x3 vmx = aView.Matrix;
        Point3d center = aView.AbsoluteOrigin;
        double[] vw = { center.X, center.Y, center.Z,
            vmx.Xx, vmx.Xy, vmx.Xz, vmx.Yx, vmx.Yy, vmx.Yz };
        double[] abs = { 0, 0, 0, 1, 0, 0, 0, 1, 0 };
        double[] mx = new double[12];
        int irc = 0;
        double[] c = { loc.X, loc.Y, loc.Z };

        theUFSession.Trns.CreateCsysMappingMatrix(vw, abs, mx, out irc);
        theUFSession.Trns.MapPosition(c, mx);

        return new Point3d(c[0], c[1], c[2]);
    }

    public static void Main(string[] args)
    {

        try
        {

            Tag UFPart;
            string part_name = "EX_Curve_CreateArc";
            int units = 2;
            string name;

            if (File.Exists(@"D:/Trials/testing.prt"))
            {
                File.Delete(@"D:/Trials/testing.prt");
                Echo("File deleted ");
            }
            theUFSession.Part.New(part_name, units, out UFPart);
            theUFSession.Part.AskPartName(UFPart, out name);
            Echo("Loaded: " + name);

            Tag arc, wcs;
            UFCurve.Arc arc_coords = new UFCurve.Arc();

            /* Fill out the data structure */
            arc_coords.start_angle = 0.0;
            arc_coords.end_angle = 3.0;
            arc_coords.arc_center = new double[3];
            arc_coords.arc_center[0] = 0.0;
            arc_coords.arc_center[1] = 0.0;
            arc_coords.arc_center[2] = 1.0;
            arc_coords.radius = 2.0;


            theUFSession.Csys.AskWcs(out wcs);
            theUFSession.Csys.AskMatrixOfObject(wcs, out arc_coords.matrix_tag);
            theUFSession.Curve.CreateArc(ref arc_coords, out arc);


            theUFSession.Part.Save();
            //theUFSession.Part.SaveAs(@"D:/Trials/testing.prt");

            Echo("Hello Saved...");
        }
        catch (NXException e)
        {
            Echo("FAILED ......." + e.Message);
        }

    }


    public static int GetUnloadOption(string arg)
    {
        return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
    }
}



