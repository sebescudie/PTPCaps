using System;
using Vanara.PInvoke;
using static Vanara.PInvoke.IpHlpApi;

class Program
{
    static void Main()
    {
        var hr = GetIfTable2(out var tbl);
        if (hr.Failed)
        {
            Console.Error.WriteLine($"GetIfTable2 failed: 0x{hr:X}");
            return;
        }

        using (tbl)
        {
            foreach (MIB_IF_ROW2 row in tbl)
            {
              
                string alias = row.Alias;
                string desc = row.Description;
                var luid = row.InterfaceLuid;

                var capsHr = GetInterfaceSupportedTimestampCapabilities(in luid, out INTERFACE_TIMESTAMP_CAPABILITIES caps);
               
                if (capsHr.Failed && capsHr == Win32Error.ERROR_FILE_NOT_FOUND) continue;

                Console.WriteLine($"{alias}");
                Console.WriteLine($"{desc}");
                Console.WriteLine($"  LUID: 0x{luid.Value:X}");
                Console.WriteLine();


                if (capsHr.Failed)
                {
                    Console.WriteLine($"GetInterfaceSupportedTimestampCapabilities failed with: {capsHr}");
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("  Supported Timestamp Capabilities");
                    Console.WriteLine();

                    Console.WriteLine($"  HardwareClockFrequencyHz: {caps.HardwareClockFrequencyHz}");
                    Console.WriteLine($"  SupportsCrossTimestamp: {caps.SupportsCrossTimestamp}");
                    Console.WriteLine();

                    var hwc = caps.HardwareCapabilities;
                    Console.WriteLine("  Hardware  Capabilities:");
                    PrintFlag("    PtpV2OverUdpIPv4EventMessageReceive", hwc.PtpV2OverUdpIPv4EventMessageReceive);
                    PrintFlag("    PtpV2OverUdpIPv4AllMessageReceive", hwc.PtpV2OverUdpIPv4AllMessageReceive);
                    PrintFlag("    PtpV2OverUdpIPv4EventMessageTransmit", hwc.PtpV2OverUdpIPv4EventMessageTransmit);
                    PrintFlag("    PtpV2OverUdpIPv4AllMessageTransmit", hwc.PtpV2OverUdpIPv4AllMessageTransmit);
                    PrintFlag("    PtpV2OverUdpIPv6EventMessageReceive", hwc.PtpV2OverUdpIPv6EventMessageReceive);
                    PrintFlag("    PtpV2OverUdpIPv6AllMessageReceive", hwc.PtpV2OverUdpIPv6AllMessageReceive);
                    PrintFlag("    PtpV2OverUdpIPv6EventMessageTransmit", hwc.PtpV2OverUdpIPv6EventMessageTransmit);
                    PrintFlag("    PtpV2OverUdpIPv6AllMessageTransmit", hwc.PtpV2OverUdpIPv6AllMessageTransmit);
                    PrintFlag("    AllReceive", hwc.AllReceive);
                    PrintFlag("    AllTransmit", hwc.AllTransmit);
                    PrintFlag("    TaggedTransmit", hwc.TaggedTransmit);

                    Console.WriteLine();
                    var swc = caps.SoftwareCapabilities;
                    Console.WriteLine("  Software Capabilities:");
                    PrintFlag("    AllReceive", swc.AllReceive);
                    PrintFlag("    AllTransmit", swc.AllTransmit);
                    PrintFlag("    TaggedTransmit", swc.TaggedTransmit);

                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
    }

  

    static void PrintFlag(string name, bool enabled)
    {
        if (enabled) Console.WriteLine(name);
    }
}