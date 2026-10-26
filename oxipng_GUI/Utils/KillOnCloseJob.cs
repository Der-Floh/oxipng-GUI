using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace OxipngGui.Utils;

/// <summary>
/// A Windows job object that ends every assigned process when the app exits, even after a crash, and runs them
/// below normal priority so a batch that uses every core leaves the desktop responsive.
/// </summary>
public sealed class KillOnCloseJob : IDisposable
{
    private const int ExtendedLimitInformationClass = 9;
    private const uint PriorityClassFlag = 0x20;
    private const uint DieOnUnhandledExceptionFlag = 0x400;
    private const uint KillOnJobCloseFlag = 0x2000;
    private const uint BelowNormalPriorityClass = 0x4000;

    private readonly SafeJobHandle _job;

    public KillOnCloseJob()
    {
        _job = CreateJobObject(IntPtr.Zero, null);
        if (_job.IsInvalid)
            throw new Win32Exception();

        var limits = new ExtendedLimitInformation
        {
            BasicLimitInformation = new BasicLimitInformation
            {
                LimitFlags = KillOnJobCloseFlag | DieOnUnhandledExceptionFlag | PriorityClassFlag,
                PriorityClass = BelowNormalPriorityClass,
            },
        };

        if (SetInformationJobObject(_job, ExtendedLimitInformationClass, ref limits, (uint)Marshal.SizeOf<ExtendedLimitInformation>()))
            return;

        var error = new Win32Exception();
        _job.Dispose();
        throw error;
    }

    public bool TryAssign(Process process)
    {
        if (process is null)
            throw new ArgumentNullException(nameof(process));

        return AssignProcessToJobObject(_job, process.Handle);
    }

    public void Dispose() => _job.Dispose();

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeJobHandle CreateJobObject(IntPtr jobAttributes, string? name);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetInformationJobObject(SafeJobHandle job, int infoClass, ref ExtendedLimitInformation info, uint length);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AssignProcessToJobObject(SafeJobHandle job, IntPtr process);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr handle);

    private sealed class SafeJobHandle() : SafeHandleZeroOrMinusOneIsInvalid(ownsHandle: true)
    {
        protected override bool ReleaseHandle() => CloseHandle(handle);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BasicLimitInformation
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public uint LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public UIntPtr Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IoCounters
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ExtendedLimitInformation
    {
        public BasicLimitInformation BasicLimitInformation;
        public IoCounters IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }
}
