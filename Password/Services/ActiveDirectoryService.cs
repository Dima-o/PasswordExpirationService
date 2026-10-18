using Password.Configuration;
using Password.Interfaces;
using Password.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace Password.Services
{
    internal class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly string _server;
        private readonly string _username;
        private readonly string _password;


        public ActiveDirectoryService()
        {
            _server =
                AppConfiguration.Configuration["ActiveDirectory:Server"];

            _username =
                AppConfiguration.Configuration["ActiveDirectory:Username"];

            _password =
                AppConfiguration.Configuration["ActiveDirectory:Password"];
        }


        public Task<List<AdUser>> GetAllUsersAsync()
        {
            var users = new List<AdUser>();

            try
            {
                if (string.IsNullOrEmpty(_server))
                {
                    throw new InvalidOperationException(
                        "Не указан сервер Active Directory");
                }


                using (var entry =
                    new DirectoryEntry(
                        _server,
                        _username,
                        _password))
                {

                    using (var searcher =
                        new DirectorySearcher(entry))
                    {
                        searcher.Filter =
                            "(&(objectCategory=person)(objectClass=user))";


                        searcher.PropertiesToLoad.Add("samaccountname");
                        searcher.PropertiesToLoad.Add("mail");
                        searcher.PropertiesToLoad.Add("pwdlastset");
                        searcher.PropertiesToLoad.Add("distinguishedname");


                        foreach (SearchResult result in searcher.FindAll())
                        {
                            var user = new AdUser();


                            if (result.Properties["samaccountname"].Count > 0)
                                user.UserName =
                                    result.Properties["samaccountname"][0].ToString();


                            if (result.Properties["mail"].Count > 0)
                                user.Email =
                                    result.Properties["mail"][0].ToString();


                            if (result.Properties["distinguishedname"].Count > 0)
                                user.DistinguishedName =
                                    result.Properties["distinguishedname"][0].ToString();


                            if (result.Properties["pwdlastset"].Count > 0)
                            {
                                long value =
                                    (long)result.Properties["pwdlastset"][0];

                                user.PasswordLastSet =
                                    DateTime.FromFileTime(value);
                            }


                            users.Add(user);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }


            return Task.FromResult(users);
        }



        public Task ChangePasswordAsync(
            string distinguishedName,
            string newPassword)
        {
            try
            {
                using (var entry =
                    new DirectoryEntry(
                        _server,
                        _username,
                        _password))
                {
                    entry.Path =
                        "LDAP://" + distinguishedName;


                    entry.Invoke(
                        "SetPassword",
                        newPassword);


                    entry.CommitChanges();
                }
            }
            catch
            {
                throw;
            }


            return Task.CompletedTask;
        }
    }
}