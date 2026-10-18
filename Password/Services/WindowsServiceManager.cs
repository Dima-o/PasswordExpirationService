using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Password.Interfaces;

namespace Password.Services
{
    internal class WindowsServiceManager : IWindowsServiceManager
    {
        private const string ServiceName =
            "PasswordExpirationService";


        private readonly string _exePath;


        public WindowsServiceManager()
        {
            _exePath =
                System.Reflection.Assembly
                .GetExecutingAssembly()
                .Location;
        }



        public void Install()
        {
            var command =
                "create " + ServiceName +
                " binPath= \"" + _exePath + "\"";


            ExecuteSc(command);

            Console.WriteLine(
                "Служба установлена.");
        }



        public void Uninstall()
        {
            Stop();


            ExecuteSc(
                "delete " + ServiceName);


            Console.WriteLine(
                "Служба удалена.");
        }



        public void Start()
        {
            if (!ServiceExists())
            {
                Console.WriteLine(
                    "Служба не установлена.");

                return;
            }


            using (var service =
                new ServiceController(ServiceName))
            {
                service.Start();

                service.WaitForStatus(
                    ServiceControllerStatus.Running);
            }


            Console.WriteLine(
                "Служба запущена.");
        }



        public void Stop()
        {
            if (!ServiceExists())
            {
                Console.WriteLine(
                    "Служба не установлена.");

                return;
            }


            using (var service =
                new ServiceController(ServiceName))
            {

                if (service.Status !=
                    ServiceControllerStatus.Stopped)
                {
                    service.Stop();

                    service.WaitForStatus(
                        ServiceControllerStatus.Stopped);
                }
            }


            Console.WriteLine(
                "Служба остановлена.");
        }



        public void Restart()
        {
            Stop();

            Thread.Sleep(2000);

            Start();
        }



        public void Status()
        {
            if (!ServiceExists())
            {
                Console.WriteLine(
                    "Служба не установлена.");

                return;
            }


            using (var service =
                new ServiceController(ServiceName))
            {
                Console.WriteLine(
                    "Статус службы: " + service.Status);
            }
        }



        public void Help()
        {
            Console.WriteLine(
                @"PasswordExpirationService

                Команды:

                install     - установить службу
                uninstall   - удалить службу
                start       - запустить службу
                stop        - остановить службу
                restart     - перезапустить службу
                status      - показать состояние
                help        - показать справку");
        }



        private void ExecuteSc(string arguments)
        {
            using (var process =
                new Process())
            {
                process.StartInfo =
                    new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = arguments,
                        UseShellExecute = true,
                        Verb = "runas"
                    };


                process.Start();

                process.WaitForExit();
            }
        }



        public bool ServiceExists()
        {
            return ServiceController
                .GetServices()
                .Any(x =>
                    x.ServiceName == ServiceName);
        }

        public bool IsInstalled()
        {
            return ServiceExists();
        }

        public bool IsRunning()
        {
            if (!ServiceExists())
                return false;

            using (var service = new ServiceController(ServiceName))
            {
                return service.Status == ServiceControllerStatus.Running;
            }
        }

    }
}