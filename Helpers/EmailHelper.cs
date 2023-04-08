using System.Net.Mail;

namespace Alpalis.UtilityServices.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            string? trimmedEmail = email.Trim();
            if (string.IsNullOrEmpty(trimmedEmail))
                return false;

            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                MailAddress addr = new(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
