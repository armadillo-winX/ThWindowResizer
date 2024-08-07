using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ThWindowResizer
{
    internal partial class GameWindowHandler
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [LibraryImport("user32.dll", SetLastError = true)]
        private static partial int MoveWindow(IntPtr hwnd, int x, int y,
            int nWidth, int nHeight, int bRepaint);

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        public static int[] GetWindowSizes(Process gameProcess)
        {
            //ウィンドウハンドルの取得
            IntPtr gameProcessMainWindow = gameProcess.MainWindowHandle;

            //ウィンドウサイズの取得
            _ = GetWindowRect(gameProcessMainWindow, out RECT rect);
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            int[] sizes = [width, height];
            return sizes;
        }

        public static WindowPosition GetWindowPosition(Process gameProcess)
        {
            IntPtr gameProcessMainWindow = gameProcess.MainWindowHandle;

            _ = GetWindowRect(gameProcessMainWindow, out RECT rect);

            return new WindowPosition
            {
                X = rect.left,
                Y = rect.top
            };
        }

        public static void ResizeWindow(Process gameProcess, int width, int height)
        {
            IntPtr gameProcessMainWindow = gameProcess.MainWindowHandle;

            WindowPosition gameWindowPosition = GetWindowPosition(gameProcess);

            _ = MoveWindow(gameProcessMainWindow, gameWindowPosition.X, gameWindowPosition.Y, width, height, 1);

            Interaction.AppActivate(gameProcess.Id);
        }

        public static void FixTopMost(Process gameProcess)
        {
            //ウィンドウハンドルの取得
            IntPtr gameProcessMainWindow = gameProcess.MainWindowHandle;

            const int SWP_NOSIZE = 0x0001;
            const int SWP_NOMOVE = 0x0002;

            const int HWND_TOPMOST = -1;
            //ウィンドウを最前面に固定
            _ = SetWindowPos(gameProcessMainWindow, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        public static void ReleaseTopMost(Process gameProcess)
        {
            //ウィンドウハンドルの取得
            IntPtr gameProcessMainWindow = gameProcess.MainWindowHandle;

            const int SWP_NOSIZE = 0x0001;
            const int SWP_NOMOVE = 0x0002;

            const int HWND_TOPMOST = -2;
            //ウィンドウの最前面固定を解除
            _ = SetWindowPos(gameProcessMainWindow, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }
    }
}
