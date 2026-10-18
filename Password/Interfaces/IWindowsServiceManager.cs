using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password.Interfaces
{
    internal interface IWindowsServiceManager
    {
        void Install();

        void Uninstall();

        void Start();

        void Stop();

        void Restart();

        void Status();

        void Help();

        bool IsInstalled();

        bool IsRunning();
    }
}
