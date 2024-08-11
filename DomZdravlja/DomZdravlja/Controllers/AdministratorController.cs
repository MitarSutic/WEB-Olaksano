using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Type = DomZdravlja.Models.Type;

namespace DomZdravlja.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        [HttpGet]
        public ActionResult Index()
        {
            Korisnik korisnik = (Korisnik)Session["user"];
            ViewBag.korisnik = korisnik;
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            ViewBag.pacijenti = sviPacijenti;
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewPacijent(string korisnickoIme)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            foreach(var p in sviPacijenti)
            {
                if(p.KorisnickoIme == korisnickoIme)
                {
                    ViewBag.pacijent = p; 
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(string ime, string prezime, string sifra, string kime, string datum,string email,string jmbg)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            if(ime == "" || prezime == "" || sifra == "" || kime == "" || datum == "" || email == "" || jmbg == "")
            {
                ViewBag.greska = "Sva polja su obavezna!";
                return View("Registration");
            }
            foreach(var k in sviPacijenti)
            {
                if (k.KorisnickoIme == kime || k.JMBG == jmbg || k.Email == email)
                {
                    ViewBag.greska = "JMBG, Korisnicko ime, email moraju biti jedinstveni!";
                    return View("Registration");
                }
            }
            Pacijent novi = new Pacijent
            {
                KorisnickoIme = kime,
                Sifra = sifra,
                Tip = Type.Pacijent,
                Ime = ime,
                Prezime = prezime,
                DatumRodjenja = DateTime.ParseExact(datum, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Email = email
            };
            sviPacijenti.Add(novi);
            ViewBag.pacijenti = sviPacijenti;
            return View("Registration");
        }
    }
}