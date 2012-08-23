using System;
using System.Collections;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class LEDWiz  : IDisposable
{
	private bool disposed = false;

	[StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi)]
		public struct LWZDEVICELIST
	{
		[MarshalAs(UnmanagedType.ByValArray,SizeConst=LWZ_MAX_DEVICES)]
		public uint[] handles;
		public int numdevices;
	}

	[DllImport("LEDWiz", CallingConvention=CallingConvention.Cdecl)]
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

	private const int LWZ_MAX_DEVICES =		16;
	private const int LWZ_ADD =				1;
	private const int LWZ_DELETE =			2;
	
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

		deviceList.handles = new uint[LWZ_MAX_DEVICES] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
		deviceList.numdevices = 0;
	}

	public static void Notify(int reason, uint newDevice)
	{
		if(reason == LWZ_ADD)
		{
			LWZ_REGISTER(newDevice, (uint) MainWindow.ToInt32());
		}
		if(reason == LWZ_DELETE)
		{
		}
	}

	public void StartupLighting()
	{
		try
		{
			NotifyDelegate del = new NotifyDelegate(Notify);

			IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(LWZDEVICELIST)));
			Marshal.StructureToPtr(deviceList, ptr, true);
			LWZ_SET_NOTIFY(del, (uint) ptr.ToInt32());
			deviceList = (LWZDEVICELIST) Marshal.PtrToStructure(ptr, typeof(LWZDEVICELIST));
			Marshal.FreeCoTaskMem(ptr);
		}
		catch(Exception ex)
		{
			System.Windows.Forms.MessageBox.Show(ex.Message, "LEDWiz Error");
		}
	}

    public void ShutdownLighting()
    {
       for(uint i=0; i<deviceList.numdevices; i++)
           LWZ_SBA(DeviceHandles[i], 0, 0, 0, 0, 2);
    }

    public void SBA(int id, int bank1, int bank2, int bank3, int bank4, int globalPulseSpeed)
    {
        LWZ_SBA(DeviceHandles[id], (uint)bank1, (uint)bank2, (uint)bank3, (uint)bank4, (uint)globalPulseSpeed);
    }

	public void PBA(int id, byte[] val)
	{
		IntPtr ptr = Marshal.AllocCoTaskMem(val.Length);
		Marshal.Copy(val, 0, ptr, val.Length);
		LWZ_PBA(DeviceHandles[id], (uint) ptr.ToInt32());
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
			LWZ_SET_NOTIFY((System.MulticastDelegate) null, 0);
			//GC.Collect();
		}
		disposed = true;
	}

    public int NumDevices
    {
        get { return deviceList.numdevices; }
    }

    public uint[] DeviceHandles
    {
        get { return deviceList.handles; }
    }
}

