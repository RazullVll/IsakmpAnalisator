namespace ISAKMP_Analisator.Handling
{
    using IsakmpAnalisator.Handling;
    using PacketDotNet;
    using SharpPcap;
    using SharpPcap.LibPcap;
    using System.Text.Json;


    // Для примера
    public class PcapReader
    {
        private List<EthernetPacket> packets = new();
        private List<UdpPacketData> UdpPacketList = new();
        private static int packetIndex = 0;

        public IEnumerable<EthernetPacket> ReadPcap(string pcapFilePath)
        {
            ICaptureDevice device;

            try
            {
                // Get an offline device
                device = new CaptureFileReaderDevice(pcapFilePath);

                // Open the device
                device.Open();

                // Подписываемся на событие о новом найденном пакете и по нему выполняем метод device_OnPacketArrival
                device.OnPacketArrival +=
                    new PacketArrivalEventHandler(device_OnPacketArrival);

                // Запускает чтение дампа pcap. Здесь начинают срабатывать события OnPacketArrival
                device.Capture();

                device.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ОШИБКА: {e} при открытии файла {pcapFilePath}");
            }
         
            return this.packets;
        }

        private void device_OnPacketArrival(object sender, PacketCapture e)
        {
            var rawPacket = e.GetPacket();

            var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            var ethernetPacket = packet.Extract<EthernetPacket>();;
            if (ethernetPacket != null && (packet is EthernetPacket))  //Добавлена проверка на Ethernet пакет
            {
                this.packets.Add(ethernetPacket);
                PortAnalize(ethernetPacket);

                //Пример инфы, которую можно дёргать из пакет


                packetIndex++;
            }
        }

        private void PortAnalize(EthernetPacket ethernetPacket)
        {
            // Извлекаем IP пакет
            var ipPacket = ethernetPacket.PayloadPacket as IPPacket;
            if (ipPacket == null) return;


            // Извлекаем UDP пакет
            var udpPacket = ipPacket.PayloadPacket as UdpPacket;
            if (udpPacket != null)
            {
                
                byte[] payload = udpPacket.PayloadData;
                if (payload != null && payload.Length > 0 && ((udpPacket.SourcePort == 4500) || (udpPacket.DestinationPort == 4500) || (udpPacket.SourcePort == 500) || udpPacket.DestinationPort == 500))
                { 
                    UdpPacketList.Add(new UdpPacketData
                    {
                        Timestamp = DateTime.Now,
                        SourceIp = ipPacket.SourceAddress.ToString(),
                        DestIp = ipPacket.DestinationAddress.ToString(),
                        SourcePort = udpPacket.SourcePort,
                        DestPort = udpPacket.DestinationPort,
                        HexDump = BitConverter.ToString(payload).Replace("-", " "),
                        Length = payload.Length
                    });
                }
            }
           
        }
        public void SaveToJson(string? OutputDirectory)
        {
            try
            {
                // Если путь не указан - используем default
                string outputDirectory = Path.Combine(
                    OutputDirectory,
                    $"isakmp_dump.json"
                );
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string json = JsonSerializer.Serialize(UdpPacketList, options);
                File.WriteAllText(outputDirectory, json);

                Console.WriteLine($"Данные сохранены в {outputDirectory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения JSON: {ex.Message}");
            }

        }





    }
}