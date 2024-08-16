using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
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

        public ActionResult ObrisiPacijent(string korisnickoIme)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            for(int i = sviPacijenti.Count() - 1; i>=0; i--)
            {
                if (sviPacijenti[i].KorisnickoIme == korisnickoIme)
                {
                    sviPacijenti.RemoveAt(i);
                    ViewBag.poruka = $"Uspesno obrisan pacijent {korisnickoIme}";
                    break;
                }
            }
            return RedirectToAction("Index");
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
            Pacijent novi = new Pacijent
            {
                KorisnickoIme = kime,
                Sifra = sifra,
                Tip = Type.Pacijent,
                Ime = ime,
                Prezime = prezime,
                DatumRodjenja = DateTime.ParseExact(datum, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Email = email,
                JMBG = jmbg
            };
            sviPacijenti.Add(novi);
            ViewBag.pacijenti = sviPacijenti;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditPacijent(string korisnickoIme)
        {
            Pacijent pacijent = (Pacijent)Session["pacijent"];
            ViewBag.greska = Session["greska"];
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            foreach (var p in sviPacijenti)
            {
                if (p.KorisnickoIme == korisnickoIme)
                {
                    ViewBag.pacijent = p;
                }
            }
            if(pacijent != null)
            ViewBag.pacijent = pacijent;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string ime, string prezime, string sifra, string kime, string datum, string email, string jmbg)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            for (int i = 0; i <= sviPacijenti.Count() - 1; i++)
            {
                if (sviPacijenti[i].Email == email)
                {
                    Session["pacijent"] = sviPacijenti[i];
                    Session["greska"] = "Email mora biti unikatan";
                    return RedirectToAction("EditPacijent");
                }
            }
            foreach (var k in sviPacijenti)
            {
                if (k.KorisnickoIme == kime)
                {
                    k.Ime = kime;
                    k.Prezime = prezime;
                    k.Sifra = sifra;
                    k.DatumRodjenja = DateTime.ParseExact(datum, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    k.Email = email;
                }
            }
            ViewBag.pacijenti = sviPacijenti;
            ViewBag.poruka = $"Uspesno promenjen korisnik {kime}";
            Session["pacijent"] = null;
            Session["greska"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Prijava");
        }
    }
}