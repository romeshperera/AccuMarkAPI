using System;
using System.Runtime.InteropServices;

namespace SampleWapper
{
    public class Wapper
    {
        [DllImport(@"CppImport\Demo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Add(int a, int b);

        [DllImport(@"CppImport\Demo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sub(int a, int b);

        [DllImport(@"CppImport\Demo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mul(int a, int b);

        [DllImport(@"CppImport\Demo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Div(int a, int b);
    }
}
