using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomZdravlja.Controllers
{
    public class PacijentController : Controller
    {
        private static List<Lekar> lekari = new List<Lekar>();
        private static List<Pacijent> pacijenti = new List<Pacijent>();
        [HttpGet]
        public ActionResult Index()
        {
            Korisnik pacijent = (Korisnik)Session["user"];
            ViewBag.korisnik = pacijent;
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["stermini"];
            List<Termin> terminiPacijenta = new List<Termin>();
            foreach(Termin t in sviTermini)
            {
                if(t.ImePacijenta == pacijent.KorisnickoIme)
                {
                    terminiPacijenta.Add(t);
                }
            }
            ViewBag.stermini = terminiPacijenta;
            return View();
        }

        public ActionResult ZakaziTermin(string datum)
        {
            List<Termin>sviTermini = (List<Termin>)HttpContext.Application["stermini"];

            return View();
        }
    }
}