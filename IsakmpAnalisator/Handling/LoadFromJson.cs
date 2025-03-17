using System.Text.Json;

namespace IsakmpAnalisator.Handling
{
    using System.Text.Json;
    using IsakmpAnalisator.Handling;

    //Десеирриализация необходима, чтобы была возможность не делать все в PcapReader
    public List<IsakmpPacket> LoadFromJson(string inputPath, bool appendData = true, bool deleteAfterLoad = true)
    {
        //  private List<IsakmpPacket> IsakmpPackets = new();
        try
        {
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Файл {inputPath} не найден!");
                return new List<IsakmpPacket>();
            }

            // Десериализация
            string json = File.ReadAllText(inputPath);
            var loadedData = JsonSerializer.Deserialize<List<IsakmpPacket>>(json) ?? new();

            // Обработка данных
            if (appendData)
            {
                // Объединение с дедупликацией
                IsakmpPackets = IsakmpPackets
                    .UnionBy(loadedData, x => x.HexDump)
                    .ToList();
            }
            else
            {
                IsakmpPackets = loadedData.ToList();
            }

            // Удаление файла при успехе
            if (deleteAfterLoad)
            {
                File.Delete(inputPath);
                Console.WriteLine($"Файл {inputPath} удален.");
            }

            Console.WriteLine($"Загружено записей: {loadedData.Count}");
            return loadedData;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка формата JSON: {ex.Message}");
            return new List<IsakmpPacket>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            return new List<IsakmpPacket>();
        }
    }
}
