using System;
using System.Reflection;

namespace BranchSdk.Utils
{
    public static class AssemblyUtils
    {
        public static readonly Assembly Reference = typeof(AssemblyUtils).GetType().Assembly;
        public static readonly Version Version = Reference.GetName().Version;
    }
}
