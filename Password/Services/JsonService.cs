using Password.Interfaces;
using Password.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Password.Services
{
    internal class JsonService : IJsonService
    {
        private readonly string _folder;


        public JsonService()
        {
            _folder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Json");


            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
        }


        public Task SaveAsync(PasswordEvent passwordEvent)
        {
            try
            {
                string fileName =
                    $"{passwordEvent.EventType}_{passwordEvent.UserName}_{DateTime.Now:yyyyMMdd_HHmmss}.json";


                string path =
                    Path.Combine(_folder, fileName);


                string json =
                    JsonSerializer.Serialize(
                        passwordEvent,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });


                File.WriteAllText(path, json);


                return Task.CompletedTask;
            }
            catch
            {
                throw;
            }
        }
    }
}