namespace IsakmpAnalisator.Handling
{
    public  class IsakmpPacket
    {
      
        public DateTime Timestamp { get; set; }
        public string SourceIp { get; set; }
        public string DestIp { get; set; }
        public string SourcePort { get; set; }
        public string DestPort { get; set; }
        public string HexDump { get; set; }
        public int Length { get; set; }
        public string InitiatorsSPI { get; set; }
        public string RespondersSPI { get; set; }
        public string NextPayload { get; set; }
        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }
        public string ExchangeType { get; set; }
        public string Flags { get; set; }
        public string MessageID { get; set; }
        public string MessageLength { get; set; }
    }
}
