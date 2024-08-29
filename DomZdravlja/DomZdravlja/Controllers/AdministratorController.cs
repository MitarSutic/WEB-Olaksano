using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
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
            List<Termin> sizTermini = (List<Termin>) HttpContext.Application["sIztermini"];
            Dictionary<string,Korisnik> sviKorisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            Session["korisnici"] = sviKorisnici;
            Session["pacijenti"] = sviPacijenti;
            ViewBag.pacijenti = sviPacijenti;
            ViewBag.sviTermini = sizTermini;
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
                    Pacijent p = sviPacijenti[i];
                    sviPacijenti.RemoveAt(i);
                    //DeletePacijent(p);
                    ViewBag.poruka = $"Uspesno obrisan pacijent {korisnickoIme}";
                    break;
                }
            }
            Session["pacijenti"] = sviPacijenti;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewPacijent(string korisnickoIme)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            List<Termin> sviTermini = (List<Termin>)HttpContext.Application["sIztermini"];
            List<Termin> terminiPacijenta = new List<Termin>();
            foreach(var p in sviPacijenti)
            {
                if(p.KorisnickoIme == korisnickoIme)
                {
                    ViewBag.pacijent = p;
                    foreach(var t in sviTermini)
                    {
                        if(t.ImePacijenta == p.Ime)
                        {
                            terminiPacijenta.Add(t);
                        }
                    }
                }
            }
            ViewBag.terminiPacijenta = terminiPacijenta;
            return View();
        }

        [HttpPost]
        public ActionResult Register(string ime, string prezime, string sifra, string kime, string datum,string email,string jmbg, string tip)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)HttpContext.Application["pacijenti"];
            foreach(Pacijent p in sviPacijenti)
            {
                if (kime == p.KorisnickoIme)
                {
                    ViewBag.kime = "Korisnicko ime mora biti jedinstveno!";
                    return View("Registration");
                }
                else if (email == p.Email)
                {
                    ViewBag.email = "Email mora biti jedinstven!";
                    return View("Registration");
                }
                else if (jmbg == p.JMBG)
                {
                    ViewBag.jmbg = "JMBG mora biti jedinstven!";
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
                DatumRodjenja = DateTime.ParseExact(datum,"dd/MM/yyyy",CultureInfo.CurrentCulture).Date,
                Email = email,
                JMBG = jmbg,
            };
            sviPacijenti.Add(novi);
            //WritePacijenta(novi);
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
                    k.DatumRodjenja = DateTime.ParseExact(datum, "dd/MM/yyyy", CultureInfo.CurrentCulture).Date;
                    k.Email = email;
                }
            }
            ViewBag.pacijenti = sviPacijenti;
            ViewBag.poruka = $"Uspesno promenjen korisnik {kime}";
            Session["pacijenti"] = sviPacijenti;
            Session["pacijent"] = null;
            Session["greska"] = null;
            return RedirectToAction("Index");
        }

        public void WritePacijenta(Pacijent p)
        {
            string fileKorisnici = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            string filePacijenti = HostingEnvironment.MapPath("~/App_Data/pacijenti.txt");
            string date = p.DatumRodjenja.ToString();
            DateTime parsed = DateTime.Parse(date);
            string formattedDate = parsed.ToString("dd/MM/yyyy");
            using (StreamWriter sw = new StreamWriter(fileKorisnici, true))
            {
                sw.WriteLine($"{p.KorisnickoIme};{p.Sifra};{p.Tip};{p.Ime};{p.Prezime};{formattedDate};{p.Email}");
            }
            using (StreamWriter sw = new StreamWriter(filePacijenti, true))
            {
                sw.WriteLine($"{p.KorisnickoIme};{p.Sifra};{p.Tip};{p.Ime};{p.Prezime};{formattedDate};{p.Email};{p.JMBG}");
            }
        }

        public void UpdatePacijent(Pacijent p)
        {
            string fileKorisnici = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            string filePacijenti = HostingEnvironment.MapPath("~/App_Data/pacijenti.txt");
            
        }

        public void DeletePacijent(Pacijent p )
        {
            string fileKorisnici = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            string filePacijenti = HostingEnvironment.MapPath("~/App_Data/pacijenti.txt");
            string tempK = Path.GetTempPath();
            string tempP = Path.GetTempPath();
            string line;
            using (var sr = new StreamReader(fileKorisnici))
            using (var sw = new StreamWriter(tempK))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    if (parts[0] == p.KorisnickoIme) 
                    {
                        
                        continue;
                    }                    
                    sw.WriteLine(line);
                }
            }

        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Prijava");           
        }
    }
}