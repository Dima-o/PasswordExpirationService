namespace Password.Interfaces
{
    internal interface IMailService
    {
        void SendWarning(string email, int daysLeft);

        void SendNewPassword(string email, string password);
    }
}
