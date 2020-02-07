using System;
using System.Reflection;

namespace BranchSdk.Utils
{
    public static class AssemblyUtils
    {
        public static readonly Version Version = Assembly.GetAssembly(typeof(AssemblyUtils)).GetName().Version;
    }
}
