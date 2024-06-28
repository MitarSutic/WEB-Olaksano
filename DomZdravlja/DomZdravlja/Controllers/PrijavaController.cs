using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Type = DomZdravlja.Models.Type;

namespace DomZdravlja.Controllers
{
    public class PrijavaController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.korisnici = HttpContext.Application["korisnici"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string kIme, string sifra)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string,Korisnik>)HttpContext.Application["korisnici"];
            if (korisnici.ContainsKey(kIme))
            {
                var korisnik = korisnici[kIme];
                if (korisnik.Sifra == sifra)
                {
                    
                    if (korisnik.Tip == Type.Pacijent)
                    {
                        return RedirectToAction("Index", "Pacijent");
                    }
                    else if (korisnik.Tip == Type.Lekar)
                    { 
                        return RedirectToAction("Index", "Lekar");
                    }
                    else if (korisnik.Tip == Type.Administrator)
                    {
                        return RedirectToAction("Index", "Administrator");
                    }
                }
            }

            ViewBag.ErrorMessage = "Neispravno korisničko ime ili šifra.";
            return View("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}