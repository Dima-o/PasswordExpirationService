using Microsoft.Extensions.DependencyInjection;
using Password.Configuration;
using Password.Infrastructure;
using Password.Interfaces;
using Password.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace Password
{
    public partial class Service1 : ServiceBase
    {
        private ServiceProvider _serviceProvider;

        private IActiveDirectoryService _activeDirectoryService;

        private IPasswordService _passwordService;

        private Timer _timer;

        private bool _isRunning;


        public Service1()
        {
            InitializeComponent();

            ServiceName = "PasswordExpirationService";

            CanStop = true;
            CanPauseAndContinue = true;
            CanShutdown = true;
        }


        protected override void OnStart(string[] args)
        {
            _serviceProvider =
                DependencyInjection.Build();


            _activeDirectoryService =
                _serviceProvider
                .GetRequiredService<IActiveDirectoryService>();


            _passwordService =
                _serviceProvider
                .GetRequiredService<IPasswordService>();


            _timer = new Timer();


            string intervalValue =
                AppConfiguration.Configuration["Service:CheckIntervalMinutes"];

            double intervalMinutes = 60;

            if (!string.IsNullOrEmpty(intervalValue))
            {
                double.TryParse(
                    intervalValue,
                    out intervalMinutes);
            }


            _timer.Interval =
                intervalMinutes * 60 * 1000;



            _timer.Elapsed += TimerElapsed;

            _timer.AutoReset = true;

            _timer.Start();

            // Первая проверка сразу при старте, не дожидаясь интервала
            Task.Run(async () =>
            {
                try
                {
                    List<AdUser> users = await _activeDirectoryService.GetAllUsersAsync();
                    await _passwordService.ProcessUsersAsync(users);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(@"C:\Temp\ServiceError.log", DateTime.Now + " " + ex + Environment.NewLine);
                }
            });


            _isRunning = true;
        }


        private async void TimerElapsed(
            object sender,
            ElapsedEventArgs e)
        {
            if (!_isRunning)
                return;


            try
            {
                List<AdUser> users =
                    await _activeDirectoryService
                    .GetAllUsersAsync();


                await _passwordService
                    .ProcessUsersAsync(users);
            }
            catch (Exception ex)
            {
                File.AppendAllText(
                    @"C:\Temp\ServiceError.log",
                    DateTime.Now +
                    " " +
                    ex +
                    Environment.NewLine);
            }
        }


        protected override void OnStop()
        {
            _isRunning = false;


            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }


            _serviceProvider?.Dispose();
        }


        protected override void OnPause()
        {
            _timer?.Stop();
        }


        protected override void OnContinue()
        {
            _timer?.Start();
        }


        protected override void OnShutdown()
        {
            OnStop();

            base.OnShutdown();
        }
    }
}