using System;
using System.Collections;
using System.Collections.Generic;

namespace BranchSdk.Utils
{
    public static class ObjectUtils
    {
        public static Dictionary<string, int> DictObjectToDictInt(Dictionary<string, object> dict)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (string k in dict.Keys) {
                result.Add(k, Convert.ToInt32(dict[k]));
            }
            return result;
        }

        public static List<string> ListObjectToListString(List<object> list)
        {
            List<string> result = new List<string>();
            foreach (object o in list) {
                result.Add(o.ToString());
            }
            return result;
        }

        public static bool IsArray(object o)
        {
            return o.GetType().IsArray;
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().GetType().IsAssignableFrom(typeof(List<>).GetType());
        }

        public static bool IsDictionary(object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().GetType().IsAssignableFrom(typeof(Dictionary<,>).GetType());
        }

        public static bool IsNumber(object value)
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

        public static Type NumberType(object value)
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
