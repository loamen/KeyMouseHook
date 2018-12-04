using Loamen.KeyMouseHook.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Loamen.KeyMouseHook
{
    public class WinApi
    {
        public const int OPEN_PROCESS_ALL = 2035711;
        public const int PAGE_READWRITE = 4;
        public const int PROCESS_CREATE_THREAD = 2;
        public const int PROCESS_HEAP_ENTRY_BUSY = 4;
        public const int PROCESS_VM_OPERATION = 8;
        public const int PROCESS_VM_READ = 256;
        public const int PROCESS_VM_WRITE = 32;

        private const int PAGE_EXECUTE_READWRITE = 0x4;
        private const int MEM_COMMIT = 4096;
        private const int MEM_RELEASE = 0x8000;
        private const int MEM_DECOMMIT = 0x4000;
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        /// <summary>
        /// 通过窗口的标题来查找窗口的句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 在DLL库中的发送消息函数
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄 </param>
        /// <param name="Msg">在这里是WM_COPYDATA</param>
        /// <param name="wParam">第一个消息参数</param>
        /// <param name="lParam">第二个消息参数</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="hwnd"></param>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);
        /// <summary>
        /// 最大化窗口-3，最小化窗口-2，正常大小窗口-1；
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        /// <summary>
        /// 得到目标进程句柄的函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetWindowThreadProcessId(int hwnd, ref int lpdwProcessId);

        /// <summary>
        /// 得到目标进程句柄的函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetWindowThreadProcessId(IntPtr hwnd,ref int lpdwProcessId);

        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public extern static int OpenProcess(int dwDesiredAccess,int bInheritHandle,int dwProcessId);

        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public extern static IntPtr OpenProcess(uint dwDesiredAccess,int bInheritHandle,uint dwProcessId);

        /// <summary>
        /// 关闭句柄的函数 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(int hObject);

        /// <summary>
        /// 读内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll ")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess,IntPtr lpBaseAddress,[In, Out] byte[] buffer,int size,out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// 读内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll ")]
        public static extern Int32 ReadProcessMemory(int hProcess, int lpBaseAddress, ref int buffer,int size,int lpNumberOfBytesWritten);

        /// <summary>
        /// 读内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll ")]
        public static extern Int32 ReadProcessMemory(int hProcess,int lpBaseAddress,byte[] buffer,int size,int lpNumberOfBytesWritten);

        /// <summary>
        /// 写内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess,IntPtr lpBaseAddress,[In, Out] byte[] buffer,int size,out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// 写内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(int hProcess,int lpBaseAddress,byte[] buffer,int size,int lpNumberOfBytesWritten);

        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpThreadAttributes"></param>
        /// <param name="dwStackSize"></param>
        /// <param name="lpStartAddress"></param>
        /// <param name="lpParameter"></param>
        /// <param name="dwCreationFlags"></param>
        /// <param name="lpThreadId"></param>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
        public static extern int CreateRemoteThread(int hProcess,int lpThreadAttributes,int dwStackSize,int lpStartAddress,int lpParameter,int dwCreationFlags,ref int lpThreadId);

        /// <summary>
        /// 开辟指定进程的内存空间  
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flAllocationType"></param>
        /// <param name="flProtect"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern System.Int32 VirtualAllocEx(System.IntPtr hProcess,System.Int32 lpAddress,System.Int32 dwSize,System.Int16 flAllocationType,System.Int16 flProtect);

        /// <summary>
        /// 开辟指定进程的内存空间
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flAllocationType"></param>
        /// <param name="flProtect"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern System.Int32 VirtualAllocEx(int hProcess,int lpAddress,int dwSize,int flAllocationType,int flProtect);

        /// <summary>
        /// 释放内存空间
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flAllocationType"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern System.Int32 VirtualFreeEx(int hProcess,int lpAddress,int dwSize,int flAllocationType);
        /// <summary>
        /// 捕获鼠标
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr h);
        /// <summary>
        /// 释放鼠标
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}
