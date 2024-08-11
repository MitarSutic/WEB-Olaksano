using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomZdravlja.Controllers
{
    public class LekarController : Controller
    {
        // GET: Lekar
        public ActionResult Index()
        {
            Korisnik korisnik = (Korisnik)Session["user"];
            ViewBag.korisnik = korisnik;
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            List<Termin> terminiLekara = new List<Termin>();
            foreach (Termin t in sviTermini)
            {
                if(t.kImeLekara == korisnik.KorisnickoIme)
                {
                    terminiLekara.Add(t);
                }
            }
            ViewBag.sIztermini = terminiLekara;
            return View();
        }
    }
}