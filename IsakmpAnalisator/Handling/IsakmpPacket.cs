namespace IsakmpAnalisator.Handling
{
    public class IsakmpPacket
    {
      
        public DateTime Timestamp { get; set; }
        public string SourceIp { get; set; }
        public string DestIp { get; set; }
        public ushort SourcePort { get; set; }
        public ushort DestPort { get; set; }
        public string HexDump { get; set; }
        public int Length { get; set; }
        public string InitiatorsSPI { get; set; }
        public string RespondersSPI { get; set; }
        public ushort NextPayload { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort ExchangeType { get; set; }
        public ushort Flags { get; set; }
        public ushort MessageID { get; set; }
        public ushort MessageLength { get; set; }
    }
}
