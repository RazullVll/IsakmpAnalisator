
namespace IsakmpAnalisator.Handling
{
    public class UdpPacketData
    {
        public DateTime Timestamp { get; set; }
        public string SourceIp { get; set; }
        public string DestIp { get; set; }
        public ushort SourcePort { get; set; }
        public ushort DestPort { get; set; }
        public string HexDump { get; set; }
        public int Length { get; set; }
    }
}

