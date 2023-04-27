using System.Net.Mail;

namespace Alpalis.UtilityServices.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
