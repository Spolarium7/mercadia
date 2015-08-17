using Mvc.Mailer;

namespace Mercadia.Api.Mailers
{
    public class UserMailer : MailerBase
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome(string name, string email, string verificationCode)
        {
            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.VerificationCode = verificationCode;

            return Populate(x =>
            {
                x.Subject = string.Format("{0} - Mercadia", "Welcome");
                x.ViewName = "Welcome";
                x.To.Add(email);
            });
        }

        public virtual MvcMailMessage ForgotPassword(string name, string email, string newPassword)
        {
            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.NewPassword = newPassword;

            return Populate(x =>
            {
                x.Subject = string.Format("{0} - Mercadia", "New Password");
                x.ViewName = "ForgotPassword";
                x.To.Add(email);
            });
        }
    }
}

