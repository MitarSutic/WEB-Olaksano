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
            ViewBag.pacijenti = HttpContext.Application["pacijenti"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string kIme, string sifra)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string,Korisnik>)HttpContext.Application["korisnici"];
            ViewBag.pacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            if (korisnici.ContainsKey(kIme))
            {
                var korisnik = korisnici[kIme];
                if (korisnik.Sifra == sifra)
                {
                    
                    if (korisnik.Tip == Type.Pacijent)
                    {
                        Session["user"] = korisnik;
                        return RedirectToAction("Index", "Pacijent");
                    }
                    else if (korisnik.Tip == Type.Lekar)
                    {
                        Session["user"] = korisnik;
                        return RedirectToAction("Index", "Lekar");
                    }
                    else if (korisnik.Tip == Type.Administrator)
                    {
                        Session["user"] = korisnik;
                        return RedirectToAction("Index", "Administrator");
                    }
                }
            }

            ViewBag.ErrorMessage = "Neispravno korisničko ime ili šifra.";
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return View("Index");
        }
    }
}