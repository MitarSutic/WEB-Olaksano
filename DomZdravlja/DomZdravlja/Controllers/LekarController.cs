using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            List<Termin> sviTermini = DataHelper.UcitajSlobodneIZakazaneTermine("~/App_Data/slobodni i zakazani termini.csv", "~/App_Data/lekari.csv");
            List<Termin> terminiLekara = new List<Termin>();
            Session["svitermini"] = sviTermini;
            foreach (Termin t in sviTermini)
            {
                if (t.Lekar.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    terminiLekara.Add(t);
                }
            }
            ViewBag.sIztermini = terminiLekara;
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
            Lekar l = (Lekar)Session["user"];
            List<Termin> sviTermini = (List<Termin>)Session["svitermini"];
            Korisnik korisnik = (Korisnik)Session["user"];
            DateTime parsedDatum = DateTime.ParseExact(datum, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
            Termin termin = new Termin
            {
                Lekar = l,
                kImeLekara = korisnik.KorisnickoIme,
                ImePacijenta = String.Empty,
                DatumIVremeZakazanogTermina = parsedDatum,
                Statustermina = StatusTermina.Slobodan,
                OpisTerapije = String.Empty
            };
            sviTermini.Add(termin);
            Session["svitermini"] = sviTermini;

            string t = $"{termin.Lekar.KorisnickoIme};{termin.DatumIVremeZakazanogTermina.ToString("dd/MM/yyyy HH:mm")};{termin.Statustermina}";
            string fileSiZTermini = Server.MapPath("~/App_Data/slobodni i zakazani termini.csv");
            using (StreamWriter sw = new StreamWriter(fileSiZTermini, true))
            {
                sw.WriteLine(t);
            }
            ViewBag.korisnik = korisnik;
            ViewBag.sIztermini = sviTermini;
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
            if(terminiLekara.Count == 0)
                ViewBag.terminiLekara = null;
            else
                ViewBag.terminiLekara = terminiLekara;
            ViewBag.lekar = k;
            return View();
        }


        [HttpPost]
        public ActionResult CreateTerapija(DateTime t, string terapija)
        {
            List<Termin> sviTermini = (List<Termin>)Session["svitermini"];
            string fileSiZTermini = Server.MapPath("~/App_Data/slobodni i zakazani termini.csv");
            DateTime datumTermina;

            // Update the Termin in the session
            foreach (Termin ter in sviTermini)
            {
                if (ter.DatumIVremeZakazanogTermina == t)
                {
                    ter.OpisTerapije = terapija;
                }
            }

            if (System.IO.File.Exists(fileSiZTermini))
            {
                string[] lines = System.IO.File.ReadAllLines(fileSiZTermini);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(';');
                    if(parts.Length == 3)
                    {
                         datumTermina = DateTime.ParseExact(parts[1], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                    }
                    else
                    {
                         datumTermina = DateTime.ParseExact(parts[3], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                    }
                    

                    if (datumTermina == t)
                    {
                        lines[i] = $"{parts[0]};{parts[1]};{parts[2]};{datumTermina.ToString("dd/MM/yyyy HH:mm")};{terapija}";
                        break;
                    }
                }

                System.IO.File.WriteAllLines(fileSiZTermini, lines);
            }

            Session["svitermini"] = sviTermini;
            ViewBag.sIztermini = sviTermini;

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