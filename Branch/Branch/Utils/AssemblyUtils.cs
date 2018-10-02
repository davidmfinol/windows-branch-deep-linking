using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Utils
{
    public static class AssemblyUtils
    {
        public static readonly Assembly Reference = typeof(AssemblyUtils).GetTypeInfo().Assembly;
        public static readonly Version Version = Reference.GetName().Version;
    }
}
