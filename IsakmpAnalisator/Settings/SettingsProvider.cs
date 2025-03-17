namespace ISAKMP_Analisator.Settings
{
    using Microsoft.Extensions.Configuration;
    public class SettingsProvider
    {
        // Создаём экземпляр класса SettingsModel
        private SettingsModel settingsModel = new();

        private bool isInitialized = false;

        public bool Init()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Установка базового пути
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Добавление файла конфигурации
                .Build();

            // Проверяем настройки
            if (!this.SettingsIsValid(configuration))
            {
                return false;
            }
            
            try
            {
                // Привязываем (биндим) settingsModel к нашим настройкам
                configuration.Bind(this.settingsModel);

                this.isInitialized = true;

                return this.isInitialized;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                this.isInitialized = false;

                return this.isInitialized;
            }
        }

        private bool SettingsIsValid(IConfigurationRoot configuration)
        {
            try
            {
                if (!Directory.Exists(configuration["Directories:InputDirectory"]))
                {
                    Console.WriteLine($"ОШИБКА: входной директории {configuration["Directories:InputDirectory"]} не существует!");

                    return  false;
                }

                if (!Directory.Exists(configuration["Directories:OutputDirectory"]))
                {
                    if (!Directory.CreateDirectory(configuration["Directories:OutputDirectory"]).Exists)
                    {
                        Console.WriteLine($"ОШИБКА: не удалось создать выходную директорию {configuration["Directories:OutputDirectory"]}");

                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ОШИБКА: при считывании настроек возникло исключение:");

                Console.WriteLine(e);

                return false;
            }
        }

        public void ShowSettingsInConsole()
        {
            if (!this.isInitialized)
            {
                Console.WriteLine("ОШИБКА: настройки не верны для отображения!");

                return;
            }

            var currentColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine($"Input Directory: {settingsModel.Directories.InputDirectory}");

            Console.WriteLine($"Output Directory: {settingsModel.Directories.OutputDirectory}");

            Console.ForegroundColor = currentColor;
        }

        // Хорошим стилем является максимально скрывать внутреннюю логику классов и
        // наружу показывать только то, что требуется
        public string? GetInputPath()
        {
            if (!this.isInitialized)
            {
                Console.WriteLine("ОШИБКА: настройки не считаны!");

                return null;
            }

            return this.settingsModel.Directories.InputDirectory;
        }

        public string? GetOutputPath()
        {
            if (!this.isInitialized)
            {
                Console.WriteLine("ОШИБКА: настройки не считаны!");

                return null;
            }

            return this.settingsModel.Directories.OutputDirectory;
        }
    }
}