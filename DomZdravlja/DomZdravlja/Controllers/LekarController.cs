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

        [HttpGet]
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

        [HttpGet]
        public ActionResult MakeTermin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MakeTerapija()
        {
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            Korisnik k = (Korisnik)Session["user"];
            List<Termin> terminiLekara = new List<Termin>();
            foreach (var t in sviTermini)
            {
                if(k.KorisnickoIme == t.kImeLekara && t.Statustermina == StatusTermina.Zakazan && t.OpisTerapije == String.Empty)
                {
                    terminiLekara.Add(t);
                }
            }
            ViewBag.terminiLekara = terminiLekara;
            ViewBag.lekar = k;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTermin(string datum)
        {
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            List<Termin> slTermini = (List<Termin>)HttpContext.Application["stermini"];
            Korisnik korisnik = (Korisnik)Session["user"];
            DateTime parsedDatum = DateTime.ParseExact(datum, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
            Termin termin = new Termin
            {
                kImeLekara = korisnik.KorisnickoIme,
                DatumIVremeZakazanogTermina = parsedDatum,
                Statustermina = StatusTermina.Slobodan
            };
            sviTermini.Add(termin);
            slTermini.Add(termin);
            ViewBag.korisnik = korisnik;
            ViewBag.sIztermini = sviTermini;
            ViewBag.slTermini = slTermini;
            return View("Index");
        }

        [HttpPost]
        public ActionResult CreateTerapija(DateTime t, string terapija)
        {
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            List<Termin> slTermini = (List<Termin>)HttpContext.Application["stermini"];
            foreach (Termin ter in sviTermini)
            {
                if(ter.DatumIVremeZakazanogTermina == t)
                {
                    ter.OpisTerapije = terapija;
                }
            }
            foreach (Termin ter in slTermini)
            {
                if (ter.DatumIVremeZakazanogTermina == t)
                {
                    ter.OpisTerapije = terapija;
                }
            }
            ViewBag.sIztermini = sviTermini;
            ViewBag.slTermini = slTermini;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PregledTerapija(string terapija)
        {
            ViewBag.terapija = terapija;
            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Prijava");
        }
    }
}