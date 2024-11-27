using _5PJS.Context;
using System.Net.Mail;

namespace _5PJS
{
    public class ValidateDate
    {
        public static bool ValidateEmail(string email, AppDbContext context)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                if (context.users.Any(usuario => usuario.Email == email))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

    }
}
