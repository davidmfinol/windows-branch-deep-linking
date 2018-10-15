using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Utils
{
    public static class ObjectExtensions
    {
        public static bool IsArray(this object o)
        {
            return o.GetType().IsArray;
        }

        public static bool IsList(this object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().GetTypeInfo().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().GetTypeInfo().IsAssignableFrom(typeof(List<>).GetTypeInfo());
        }

        public static bool IsDictionary(this object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().GetTypeInfo().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().GetTypeInfo().IsAssignableFrom(typeof(Dictionary<,>).GetTypeInfo());
        }

        public static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static Type NumberType(this object value)
        {
            if (value is sbyte) return typeof(sbyte);
            else if (value is byte) return typeof(byte);
            else if (value is short) return typeof(short);
            else if (value is ushort) return typeof(ushort);
            else if (value is int) return typeof(int);
            else if (value is uint) return typeof(uint);
            else if (value is long) return typeof(long);
            else if (value is ulong) return typeof(ulong);
            else if (value is float) return typeof(float);
            else if (value is double) return typeof(double);
            else if (value is decimal) return typeof(decimal);
            else return typeof(int);
        }
    }
}
