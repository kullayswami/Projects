using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NXOpen;
using NXOpen.Features;

static class Module1
{
    public static void Main()
    {
        Session theSession = Session.GetSession();
        

        Part workPart = theSession.Parts.Work;
        ListingWindow lw = theSession.ListingWindow;
        lw.Open();

        foreach (NXOpen.Features.Feature partFeature in workPart.Features)
        {
            lw.WriteLine("feature name: " + partFeature.GetFeatureName());
            lw.WriteLine("feature type: " + partFeature.FeatureType);
            lw.WriteLine("");
        }

        lw.Close();
    }

    public static int GetUnloadOption(string dummy)
    {

        // Unloads the image immediately after execution within NX
       return System.Convert.ToInt32( NXOpen.Session.LibraryUnloadOption.Immediately);
    }
}