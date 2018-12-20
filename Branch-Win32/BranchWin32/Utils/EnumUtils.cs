namespace BranchSdk.Utils
{
    public static class EnumUtils
    {
        public static T TryParse<T>(string value)
        {
            T e;
            try {
                e = (T)System.Enum.Parse(typeof(T), value);
            } catch {
                e = default(T);
            }
            return e;
        }
    }
}
