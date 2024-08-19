using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DomZdravlja.Controllers
{
    public class LekarController : Controller
    {

        public ActionResult Index()
        {
            Korisnik korisnik = (Korisnik)Session["user"];
            ViewBag.korisnik = korisnik;
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            List<Termin> slTermini = (List<Termin>)HttpContext.Application["stermini"];
            List<Termin> terminiLekara = new List<Termin>();
            foreach (Termin t in sviTermini)
            {
                if (t.kImeLekara == korisnik.KorisnickoIme)
                {
                    terminiLekara.Add(t);
                }
            }
            ViewBag.sIztermini = terminiLekara;
            ViewBag.slTermini = slTermini;
            return View();
        }


        public ActionResult MakeTermin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTermin(DateTime datum)
        {
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            List<Termin> slTermini = (List<Termin>)HttpContext.Application["stermini"];
            Korisnik korisnik = (Korisnik)Session["user"];
            Termin termin = new Termin
            {
                kImeLekara = korisnik.KorisnickoIme,
                DatumIVremeZakazanogTermina = datum,
                Statustermina = StatusTermina.Slobodan
            };
            sviTermini.Add(termin);
            slTermini.Add(termin);
            ViewBag.korisnik = korisnik;
            ViewBag.sIztermini = sviTermini;
            ViewBag.slTermini = slTermini;
            return View("Index");
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Prijava");
        }
    }
}