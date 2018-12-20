using System;
using System.Collections.Generic;
using System.Text;

namespace BranchSdk.Utils
{
    public class WindowsUtils
    {
        public static string GetOSVersion()
        {
            //OperatingSystem os = Environment.OSVersion;
            //Version vs = os.Version;
            //string operatingSystem = "";

            //if (os.Platform == PlatformID.Win32Windows) {
            //    switch (vs.Minor) {
            //        case 0:
            //            operatingSystem = "95";
            //            break;
            //        case 10:
            //            if (vs.Revision.ToString() == "2222A")
            //                operatingSystem = "98SE";
            //            else
            //                operatingSystem = "98";
            //            break;
            //        case 90:
            //            operatingSystem = "Me";
            //            break;
            //        default:
            //            break;
            //    }
            //} else if (os.Platform == PlatformID.Win32NT) {
            //    switch (vs.Major) {
            //        case 3:
            //            operatingSystem = "NT 3.51";
            //            break;
            //        case 4:
            //            operatingSystem = "NT 4.0";
            //            break;
            //        case 5:
            //            if (vs.Minor == 0)
            //                operatingSystem = "2000";
            //            else
            //                operatingSystem = "XP";
            //            break;
            //        case 6:
            //            if (vs.Minor == 0)
            //                operatingSystem = "Vista";
            //            else if (vs.Minor == 1)
            //                operatingSystem = "7.0";
            //            else if (vs.Minor == 2)
            //                operatingSystem = "8.0";
            //            else
            //                operatingSystem = "8.1";
            //            break;
            //        case 10:
            //            operatingSystem = "10.0";
            //            break;
            //        default:
            //            break;
            //    }
            //}
            string operatingSystem = "10.0";
            return operatingSystem;
        }
    }
}
