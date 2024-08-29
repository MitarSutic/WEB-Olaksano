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
    // za termine napravi sesiju lekara pa onda dodavaj jednog po jednog u termine
    public class PacijentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Korisnik pacijent = (Korisnik)Session["user"];
            ViewBag.korisnik = pacijent;
            List<Termin> slobodniTermini = (List<Termin>)HttpContext.Application["stermini"];
            Session["sltermini"] = slobodniTermini;
            Session["svitermini"] = (List<Termin>)HttpContext.Application["sIztermini"];
            ViewBag.stermini = slobodniTermini;
            return View();
        }

        [HttpPost]
        public ActionResult ZakaziTermin(DateTime datum)
        {
            List<Termin> slobodniTermini = (List<Termin>)HttpContext.Application["stermini"];
            List<Termin> slobodniIZakazaniTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            Korisnik pacijent = (Korisnik)Session["user"];

            for (int t = 0; t < slobodniTermini.Count;)
            {
                if (slobodniTermini[t].DatumIVremeZakazanogTermina == datum)
                {
                    slobodniTermini[t].Statustermina = StatusTermina.Zakazan;
                    slobodniTermini[t].ImePacijenta = pacijent.Ime;
                    ViewBag.poruka = $"Uspesno zakazan termin {datum.ToString("dd/MM/yyyy HH:mm")}";
                    slobodniIZakazaniTermini.Add(slobodniTermini[t]); 
                    slobodniTermini.RemoveAt(t); 
                    break;
                }
                else
                {
                    t++; 
                }
            }

            
            Session["sltermini"] = slobodniTermini;
            Session["svitermini"] = slobodniIZakazaniTermini;
            ViewBag.stermini = slobodniTermini;
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