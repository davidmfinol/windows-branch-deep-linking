using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace COMWrapper
{
    public delegate void CallbackFunction(COMTestCallbackData data);

    [StructLayout(LayoutKind.Sequential)]
    public class COMTestCallbackData
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string Data;

        public static void InvokeCallback(IntPtr callback, COMTestCallbackData data)
        {
            CallbackFunction cbFunc = Marshal.GetDelegateForFunctionPointer<CallbackFunction>(callback);
            cbFunc(data);
        }
    }
}
