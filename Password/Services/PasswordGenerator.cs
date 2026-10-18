using Password.Interfaces;
using System;
using System.Security.Cryptography;

namespace Password.Services
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Lower = "abcdefghijkmnopqrstuvwxyz";
        private const string Digits = "23456789";
        private const string Special = "!@#$%^&*";


        public string Generate()
        {
            string allChars =
                Upper + Lower + Digits + Special;


            char[] password = new char[12];


            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[4];


                for (int i = 0; i < password.Length; i++)
                {
                    rng.GetBytes(buffer);


                    int number =
                        BitConverter.ToInt32(buffer, 0)
                        & int.MaxValue;


                    password[i] =
                        allChars[number % allChars.Length];
                }
            }


            return new string(password);
        }
    }
}