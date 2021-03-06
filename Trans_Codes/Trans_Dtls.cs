﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller_Crm;

namespace Controller_Crm
{
    class Trans_Dtls : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;
        public Trans_Dtls(IntPtr handle) { this.handle = handle; }
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        private void Dispose(bool disposing)
        {
            if (!this.disposed) { CloseHandle(handle); handle = IntPtr.Zero; disposed = true; }
        }
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        ~Trans_Dtls() { Dispose(false); }
        public Trans_Dtls()
        {
        }
    }
}
