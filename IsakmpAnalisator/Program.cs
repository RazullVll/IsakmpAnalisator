namespace ISAKMP_Analisator
{
    using ISAKMP_Analisator.Handling;
    using ISAKMP_Analisator.Settings;

    public class Program
    {
        private static readonly SettingsProvider SettingsProvider = new();

        private static readonly HandlingProvider HandlingProvider = new();

        public static void Main(string[] args)
        {
           
            if (!SettingsProvider.Init())
            {
                Console.ReadKey();

                return;
            }

            
            SettingsProvider.ShowSettingsInConsole();

            while (true)
            {
                ShowActionsList();

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            // Пример считывания входной директории
                            HandlingProvider.ReadPcapInput(SettingsProvider.GetInputPath(), SettingsProvider.GetOutputPath());

                            break;
                        }
                    case "2":
                        {
                            return;
                        }
                }
            }
        }

        private static void ShowActionsList()
        {
            Console.WriteLine("\r\nВыберите цифру действия:\r\n\t" +
                              "1 - Обработать pcap-файлы\r\n\t" +
                              "2 - Выход\r\n" +
                              "и нажмите Enter\r\n");
        }
    }
}