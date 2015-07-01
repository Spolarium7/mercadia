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

    }
}

