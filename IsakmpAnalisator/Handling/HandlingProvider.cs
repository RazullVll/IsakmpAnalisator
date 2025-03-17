using PacketDotNet;
using PacketDotNet.Ieee80211;
using SharpPcap.LibPcap;

namespace ISAKMP_Analisator.Handling
{
    public class HandlingProvider
    {
        private PcapReader pcapReader;

        public HandlingProvider()
        {
            pcapReader = new PcapReader();
        }

        public void ReadPcapInput(string? inputDirectory, string? outputDirectory)
        {
            if (!Directory.Exists(inputDirectory))
            {
                Console.WriteLine("ОШИБКА: не удалось получить доступ к входной директории!");

                return;
            }

            // Пример: извлекаем пакеты из каждого pcap на входе
            foreach (var file in Directory.EnumerateFiles(inputDirectory, "*.pca*", SearchOption.AllDirectories))
            {
                var packets = pcapReader.ReadPcap(file);
                pcapReader.SaveToJson(outputDirectory);

                Console.WriteLine($"Из {file} извлечено {packets.Count()} ethernet пакетов");
            }
        }
     
    }

}