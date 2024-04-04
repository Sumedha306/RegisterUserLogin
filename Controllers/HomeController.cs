using RegisterUserLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace RegisterUserLogin.Controllers
{
    public class HomeController : Controller
    {
        DigilinkEntities db = new DigilinkEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            if (db.Users.Any(x => x.Name == user.Name || x.Email == user.Email))
            {
                ViewBag.Notification = "User already exists";
            }
            else
            {
                db.Users.Add(user);
                db.SaveChanges();
                Session["userSS"] = user;
                SendEmailSuccessfulRegistration(user.Email);
                return RedirectToAction("Login","Home",user);
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var UserExists = db.Users.Where(x => x.Name == user.Name && x.Password == user.Password).FirstOrDefault();
            if (UserExists == null)
            {
                ViewBag.Notification = "Wrong credentials ! Kindly Check";
            }
            else
            {
                Session["userSS"] = user;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public void SendEmailSuccessfulRegistration(string email)
        {
            MailAddress mailTo = new MailAddress(email);
            MailAddress mailFrom = new MailAddress("https://mailtrap.io/");

            MailMessage email = new MailMessage(mailFrom, mailTo);
            email.subject = "Successful Registration";
            email.body = "Successful Registration ";
            SmtpClient smtp = new SmtpClient();
            // filling all the details of smtp
            smtp.Send(email);
        }
    }
}