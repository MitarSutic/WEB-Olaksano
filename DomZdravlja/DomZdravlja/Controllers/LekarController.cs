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
            Session["svitermini"] = sviTermini;
            Session["sltermini"] = slTermini;
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

        [HttpPost]
        public ActionResult CreateTermin(string datum)
        {
            List<Termin> sviTermini = (List<Termin>)Session["svitermini"];
            List<Termin> slTermini = (List<Termin>)Session["sltermini"];
            Korisnik korisnik = (Korisnik)Session["user"];
            DateTime parsedDatum = DateTime.ParseExact(datum, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
            Termin termin = new Termin
            {
                kImeLekara = korisnik.KorisnickoIme,
                ImePacijenta = String.Empty,
                DatumIVremeZakazanogTermina = parsedDatum,
                Statustermina = StatusTermina.Slobodan,
                OpisTerapije = String.Empty
            };
            sviTermini.Add(termin);
            slTermini.Add(termin);
            Session["svitermini"] = sviTermini;
            Session["sltermini"] = slTermini;
            ViewBag.korisnik = korisnik;
            ViewBag.sIztermini = sviTermini;
            ViewBag.slTermini = slTermini;
            return View("Index");
        }

        [HttpGet]
        public ActionResult MakeTerapija()
        {
            List<Termin> sviTermini = (List <Termin>)Session["svitermini"];
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
        public ActionResult CreateTerapija(DateTime t, string terapija)
        {
            List<Termin> sviTermini = (List<Termin>)Session["svitermini"];
            List<Termin> slTermini = (List<Termin>)Session["sltermini"];
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
            Session["svitermini"] = sviTermini;
            Session["sltermini"] = slTermini;
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
            Session["sltermini"] = null;
            Session["svitermini"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Prijava");
        }
    }
}