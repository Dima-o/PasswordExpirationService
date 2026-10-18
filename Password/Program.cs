using System;
using System.ServiceProcess;
using Password.Services;

namespace Password
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var manager = new WindowsServiceManager();

            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "install": manager.Install(); return;
                    case "uninstall": manager.Uninstall(); return;
                    case "start": manager.Start(); return;
                    case "stop": manager.Stop(); return;
                    case "restart": manager.Restart(); return;
                    case "status": manager.Status(); return;
                    case "help": manager.Help(); return;
                }
            }

            if (Environment.UserInteractive)
            {

                Console.WriteLine("Проверка службы PasswordExpirationService...");

                if (!manager.IsInstalled())
                {
                    Console.WriteLine("Служба не установлена. Устанавливаю...");
                    manager.Install();
                }

                if (!manager.IsRunning())
                {
                    Console.WriteLine("Запускаю службу...");
                    manager.Start();
                }
                else
                {
                    Console.WriteLine("Служба уже запущена.");
                }

                Console.WriteLine(
                    "Служба работает в фоне через диспетчер служб Windows. " +
                    "Это окно можно закрыть — работа службы не прекратится.");

                return;
            }

            ServiceBase.Run(new ServiceBase[] { new Service1() });
        }
    }
}