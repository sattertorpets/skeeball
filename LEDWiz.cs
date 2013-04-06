using System;
using System.Collections;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SkeeBall
{
    public class LEDWiz : IDisposable
    {
        //LEDWiz Connections
        // 1,2,3    - 30 R,G,B
        // 4,5,6    - 20 R,G,B
        // 7,8,9    - 10 R,G,B
        // 10,11,12 - R100 R,G,B
        // 13,14,15 - 50 R,G,B
        // 16       - Unused
        // 17,18,19 - 40 R,G,B
        // 20,21,22 - L100 R,G,B    All RGB LEDs are common anode, and have their common pin (black wire) connected to 5 V
        // All Button LEDS are common cathode, and are wired with black as ground and colored wire as 12 V.  They are wired to Bank 4.
        // 25       - Up (white)
        // 26       - Down (white)
        // 27       - Back (red)
        // 28       - Forward (blue)private bool disposed = false;

        private bool disposed = false;
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct LWZDEVICELIST
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LWZ_MAX_DEVICES)]
            public uint[] handles;
            public int numdevices;
        }

        [DllImport("LEDWiz", CallingConvention = CallingConvention.Cdecl)]
        private extern static void LWZ_SBA(uint device, uint bank0, uint bank1, uint bank2, uint bank3, uint globalPulseSpeed);

        [DllImport("LEDWiz", CallingConvention = CallingConvention.Cdecl)]
        private extern static void LWZ_PBA(uint device, uint brightness);

        [DllImport("LEDWiz", CallingConvention = CallingConvention.Cdecl)]
        private extern static void LWZ_REGISTER(uint h, uint hwnd);

        [DllImport("LEDWiz", CallingConvention = CallingConvention.Cdecl)]
        private extern static void LWZ_SET_NOTIFY(MulticastDelegate notifyProc, uint list);

        public delegate void NotifyDelegate(int reason, uint newDevice);

        private static IntPtr MainWindow = IntPtr.Zero;

        public static LWZDEVICELIST deviceList;

        private const int LWZ_MAX_DEVICES = 16;
        private const int LWZ_ADD = 1;
        private const int LWZ_DELETE = 2;

        public enum AutoPulseMode : int
        {
            RampUpRampDown = 129,				// /\/\
            OnOff = 130,						// _|-|_|-|
            OnRampDown = 131,					// -\|-\
            RampUpDown = 132					// /-|/-
        }

        public LEDWiz(IntPtr hwnd)
        {
            MainWindow = hwnd;

            deviceList.handles = new uint[LWZ_MAX_DEVICES] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            deviceList.numdevices = 0;
        }

        public static void Notify(int reason, uint newDevice)  //don't think i need this, will not be plugging/unplugging devices
        {
            if (reason == LWZ_ADD)
            {
                LWZ_REGISTER(newDevice, (uint)MainWindow.ToInt32());
            }
            if (reason == LWZ_DELETE)
            {
            }
        }

        public void StartupLighting()   //seems like this may be important even I don't care about the pattern just to initialize the device
        {
            try
            {
                NotifyDelegate del = new NotifyDelegate(Notify);

                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(LWZDEVICELIST)));
                Marshal.StructureToPtr(deviceList, ptr, true);
                LWZ_SET_NOTIFY(del, (uint)ptr.ToInt32());
                deviceList = (LWZDEVICELIST)Marshal.PtrToStructure(ptr, typeof(LWZDEVICELIST));
                Marshal.FreeCoTaskMem(ptr);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "LEDWiz Error");
            }
        }

        public void ShutdownLighting()  //similarly, this is called by ledwiz.Dispose()
        {
            for (uint i = 0; i < deviceList.numdevices; i++)
                LWZ_SBA(DeviceHandles[i], 0, 0, 0, 0, 2);
        }

        public void SBA(int bank1, int bank2, int bank3, int bank4, int globalPulseSpeed)
        {
            LWZ_SBA(DeviceHandles[0], (uint)bank1, (uint)bank2, (uint)bank3, (uint)bank4, (uint)globalPulseSpeed);
        }

        public void PBA(byte[] val)
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(val.Length);
            Marshal.Copy(val, 0, ptr, val.Length);
            LWZ_PBA(DeviceHandles[0], (uint)ptr.ToInt32());
            Marshal.FreeCoTaskMem(ptr);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // remove this from gc finalizer list
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed) // dispose once only
            {
                if (disposing) // called from Dispose
                {
                    // Dispose managed resources.
                }
                // Clean up unmanaged resources here.
                ShutdownLighting();
                LWZ_SET_NOTIFY((System.MulticastDelegate)null, 0);
                //GC.Collect();
            }
            disposed = true;
        }

        public int NumDevices
        {
            get { return deviceList.numdevices; }
        }

        public uint[] DeviceHandles  //will always call this as DeviceHandles[0], since there is only 1 device
        {
            get { return deviceList.handles; }
        }
    }
}


