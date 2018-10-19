using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Hooks
{
    public class MouseInput : IDisposable
    {
        public event EventHandler<EventArgs> MouseMoved;

        private WindowsHookHelper.HookDelegate mouseDelegate;
        private IntPtr mouseHandle;
        private const Int32 WH_MOUSE_LL = 14;

        private bool _disposed;

        public MouseInput()
        {
            mouseDelegate = MouseHookDelegate;
            mouseHandle = WindowsHookHelper.SetWindowsHookEx(WH_MOUSE_LL, mouseDelegate, IntPtr.Zero, 0);
        }

        private IntPtr MouseHookDelegate(Int32 code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return WindowsHookHelper.CallNextHookEx(mouseHandle, code, wParam, lParam);

            MouseMoved?.Invoke(this, new EventArgs());

            return WindowsHookHelper.CallNextHookEx(mouseHandle, code, wParam, lParam);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (mouseHandle != IntPtr.Zero)
                    WindowsHookHelper.UnhookWindowsHookEx(mouseHandle);

                _disposed = true;
            }
        }

        ~MouseInput()
        {
            Dispose(false);
        }
    }
}
