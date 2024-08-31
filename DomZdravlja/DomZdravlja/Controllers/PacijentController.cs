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
    public class PacijentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Korisnik pacijent = (Korisnik)Session["user"];
            ViewBag.korisnik = pacijent;
            List<Termin> svitermini = DataHelper.UcitajSlobodneIZakazaneTermine("~/App_Data/slobodni i zakazani termini.csv", "~/App_Data/lekari.csv");
            Session["svitermini"] = svitermini;
            List<Termin> slobodniTermini = new List<Termin>();
            for(int i = 0; i < svitermini.Count; i++)
            {
                if (svitermini[i].Statustermina == StatusTermina.Slobodan)
                {
                    slobodniTermini.Add(svitermini[i]);
                }
            }
            if(slobodniTermini.Count() == 0)
            {
                ViewBag.stermini = null;
            }
            else
            ViewBag.stermini = slobodniTermini;
            return View();
        }

        [HttpPost]
        public ActionResult ZakaziTermin(DateTime datum)
        {

            List<Termin> slobodniIZakazaniTermini = DataHelper.UcitajSlobodneIZakazaneTermine("~/App_Data/slobodni i zakazani termini.csv", "~/App_Data/lekari.csv");
            Korisnik pacijent = (Korisnik)Session["user"];
            string fileSiZTermini = Server.MapPath("~/App_Data/slobodni i zakazani termini.csv");

            for (int t = 0; t < slobodniIZakazaniTermini.Count;)
            {
                if (slobodniIZakazaniTermini[t].DatumIVremeZakazanogTermina == datum)
                {
                    ViewBag.poruka = $"Uspesno zakazan termin {datum.ToString("dd/MM/yyyy HH:mm")}";
                    slobodniIZakazaniTermini[t].Statustermina = StatusTermina.Zakazan;
                    slobodniIZakazaniTermini[t].ImePacijenta = pacijent.Ime;
                    if (System.IO.File.Exists(fileSiZTermini))
                    {
                        string[] lines = System.IO.File.ReadAllLines(fileSiZTermini);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] parts = lines[i].Split(';');
                            if(parts.Count() > 3)
                            {
                                continue;
                            }
                            DateTime datumTermina = DateTime.ParseExact(parts[1], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);

                            // Find the line in the CSV file and update it
                            if (datumTermina == datum)
                            {
                                // Update the CSV line with the new status and patient name
                                lines[i] = $"{parts[0]};{pacijent.Ime};{slobodniIZakazaniTermini[t].Statustermina};{datumTermina.ToString("dd/MM/yyyy HH:mm")}";
                                break;
                            }
                        }
                        // Write the updated lines back to the CSV file
                        System.IO.File.WriteAllLines(fileSiZTermini, lines);
                    }
                    break;
                }
                else
                {
                    t++; 
                }
            }
            Session["svitermini"] = slobodniIZakazaniTermini;
            List<Termin> terminiPacijenta = new List<Termin>();
            for (int i = 0; i < slobodniIZakazaniTermini.Count; i++)
            {
                if (slobodniIZakazaniTermini[i].Statustermina == StatusTermina.Slobodan)
                {
                    terminiPacijenta.Add(slobodniIZakazaniTermini[i]);
                }
            }
            if(terminiPacijenta.Count == 0)
            {
                ViewBag.stermini = null;
            }
            else
            ViewBag.stermini = terminiPacijenta;

            ViewBag.korisnik = pacijent;
            return View("Index");
        }

        [HttpGet]
        public ActionResult PregledTerapija()
        {
            Korisnik pacijent = (Korisnik)Session["user"];
            List<Termin> zakazanitermini = (List<Termin>)Session["svitermini"];
            List<Termin> terminipacijenta = new List<Termin>();
            foreach(Termin ter in zakazanitermini)
            {
                if(pacijent.Ime == ter.ImePacijenta && ter.Statustermina == StatusTermina.Zakazan)
                {
                    terminipacijenta.Add(ter);
                }
            }
            if (terminipacijenta.Count() == 0)
            {
                ViewBag.termini = null;
            }
            else
            ViewBag.termini = terminipacijenta;

            return View();
        }

        [HttpGet]
        public ActionResult Terapija(string terapija)
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