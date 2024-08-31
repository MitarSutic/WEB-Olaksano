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
            return View();
        }

        [HttpGet]
        public ActionResult Login(string kIme, string sifra)
        {
            Dictionary<string, Korisnik> sviKorisnici = DataHelper.UcitajKorisnike("~/App_Data/korisnici.csv");
            Korisnik korisnik = new Korisnik();
            if (sviKorisnici.ContainsKey(kIme))
            {
                if (sviKorisnici[kIme].Sifra == sifra)
                {
                    ViewBag.ErrorMessage = null;
                    korisnik = sviKorisnici[kIme];
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
                else
                {
                    ViewBag.ErrorMessage = "Netacno korisnicko ime ili lozinka";
                    return View("Index");
                }
                
            }
            else
            {
                ViewBag.ErrorMessage = "Korisnicko ime nije prijavljeno";
            }
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