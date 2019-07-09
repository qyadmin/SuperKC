using UnityEngine;

using System.Collections;

using System.Runtime.InteropServices;
using System.Text;
using System;

public class C_DEMO
{

    [DllImport("U3DDLL",EntryPoint = "Add")]
    private static extern  IntPtr Add();


    public static string code = "";
    public static string Creat()
    {
        return Marshal.PtrToStringAnsi(Add());   
    }
}
