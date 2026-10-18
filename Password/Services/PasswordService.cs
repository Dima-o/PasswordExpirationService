using Password.Configuration;
using Password.Interfaces;
using Password.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Password.Services
{
    internal class PasswordService : IPasswordService
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IMailService _mailService;
        private readonly IJsonService _jsonService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly int _maxPasswordAgeDays;
        private readonly int _warningBeforeExpireDays;

        


        private const int MaxPasswordAgeDays = 90;
        private const int WarningBeforeExpireDays = 7;


        public PasswordService(
            IActiveDirectoryService activeDirectoryService,
            IPasswordGenerator passwordGenerator,
            IMailService mailService,
            IJsonService jsonService)
        {
            _activeDirectoryService = activeDirectoryService;
            _passwordGenerator = passwordGenerator;
            _mailService = mailService;
            _jsonService = jsonService;

            _maxPasswordAgeDays = int.TryParse(
                AppConfiguration.Configuration["PasswordPolicy:MaxPasswordAgeDays"],
                out var maxAge) ? maxAge : 90;

            _warningBeforeExpireDays = int.TryParse(
                AppConfiguration.Configuration["PasswordPolicy:WarningBeforeExpireDays"],
                out var warnDays) ? warnDays : 7;
        }



        public async Task ProcessUsersAsync(List<AdUser> users)
        {
            DateTime today = DateTime.Now;


            foreach (var user in users)
            {
                int daysLeft =
                    MaxPasswordAgeDays -
                    (today - user.PasswordLastSet).Days;


                if (daysLeft == WarningBeforeExpireDays)
                {
                    _mailService.SendWarning(
                        user.Email,
                        daysLeft);


                    await _jsonService.SaveAsync(
                        new PasswordEvent
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            EventType = "Warning",
                            EventDate = DateTime.Now
                        });
                }


                if (daysLeft <= 0)
                {
                    string newPassword =
                        _passwordGenerator.Generate();


                    await _activeDirectoryService
                        .ChangePasswordAsync(
                            user.DistinguishedName,
                            newPassword);



                    _mailService.SendNewPassword(
                        user.Email,
                        newPassword);



                    await _jsonService.SaveAsync(
                        new PasswordEvent
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            EventType = "PasswordChanged",
                            EventDate = DateTime.Now,
                            NewPassword = newPassword
                        });


                    newPassword = string.Empty;
                }
            }
        }
    }
}