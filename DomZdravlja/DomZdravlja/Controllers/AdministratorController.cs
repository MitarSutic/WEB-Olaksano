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

        [HttpGet]
        public ActionResult Index()
        {
            Korisnik korisnik = (Korisnik)Session["user"];
            ViewBag.korisnik = korisnik;
            Session["pacijenti"] = DataHelper.UcitajPacijente("~/App_Data/pacijenti.csv");
            Session["svitermini"] = DataHelper.UcitajSlobodneIZakazaneTermine("~/App_Data/slobodni i zakazani termini.csv", "~/App_Data/lekari.csv");
            Session["korisnici"] = DataHelper.UcitajKorisnike("~/App_Data/korisnici.csv");
            ViewBag.pacijenti = Session["pacijenti"];
            ViewBag.sviTermini = Session["siztermini"];
            Session["greska"] = null;
            Session["pacijent"] = null;
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult ObrisiPacijent(string korisnickoIme)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)Session["pacijenti"];
            Dictionary<string, Korisnik> sviKorisnici = (Dictionary<string, Korisnik>)Session["korisnici"];
            for (int i = sviPacijenti.Count() - 1; i>=0; i--)
            {
                if (sviPacijenti[i].KorisnickoIme == korisnickoIme)
                {
                    sviPacijenti.RemoveAt(i);
                    ViewBag.poruka = $"Uspesno obrisan pacijent {korisnickoIme}";
                    break;
                }
            }

            string filePacijenti = Server.MapPath("~/App_Data/pacijenti.csv");
            string[] allLinesPacijenti = System.IO.File.ReadAllLines(filePacijenti);
            List<string> updatedLinesPacijenti = new List<string>();

            foreach (string line in allLinesPacijenti)
            {
                string[] fields = line.Split(';');
                if (fields[0] != korisnickoIme) 
                {
                    updatedLinesPacijenti.Add(line);
                }
            }
            System.IO.File.WriteAllLines(filePacijenti, updatedLinesPacijenti.ToArray());

            

            string fileKorisnici = Server.MapPath("~/App_Data/korisnici.csv");
            string[] allLinesKorisnici = System.IO.File.ReadAllLines(fileKorisnici);
            List<string> updatedLinesKorisnici = new List<string>();

            foreach (string line in allLinesKorisnici)
            {
                string[] fields = line.Split(';');
                if (fields[0] != korisnickoIme) 
                {
                    updatedLinesKorisnici.Add(line);
                }
            }
            System.IO.File.WriteAllLines(fileKorisnici, updatedLinesKorisnici.ToArray());

            Session["pacijenti"] = sviPacijenti;
            Session["korisnici"] = sviKorisnici;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewPacijent(string korisnickoIme)
        {
            List<Pacijent> sviPacijenti = (List<Pacijent>)Session["pacijenti"];
            List<Termin> sviTermini = (List<Termin>)Session["svitermini"];
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
            List<Pacijent> sviPacijenti = (List<Pacijent>)Session["pacijenti"];
            Dictionary<string, Korisnik> sviKorisnici = (Dictionary<string,Korisnik>)Session["korisnici"];
            foreach (Pacijent p in sviPacijenti)
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
            sviKorisnici.Add(novi.KorisnickoIme,novi);
            Session["pacijenti"] = sviPacijenti;
            Session["korisnici"] = sviKorisnici;
            ViewBag.pacijenti = sviPacijenti;
            string fileKorisnici = Server.MapPath("~/App_Data/korisnici.csv");
            string filePacijenti = Server.MapPath("~/App_Data/pacijenti.csv");
            string korisnik = $"{kime};{sifra};{novi.Tip};{ime};{prezime};{datum};{email}";
            string pacijent = $"{kime};{sifra};{novi.Tip};{ime};{prezime};{datum};{email};{jmbg}";

            using (StreamWriter sw = new StreamWriter(fileKorisnici, true))
            {
                sw.WriteLine(korisnik);
            }
            using (StreamWriter sw = new StreamWriter(filePacijenti, true))
            {
                sw.WriteLine(pacijent);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditPacijent(string korisnickoIme)
        {
            Pacijent pacijent = (Pacijent)Session["pacijent"];
            ViewBag.greska = Session["greska"];
            List<Pacijent> sviPacijenti = (List<Pacijent>)Session["pacijenti"];
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
            List<Pacijent> sviPacijenti = (List<Pacijent>)Session["pacijenti"];
            Dictionary<string, Korisnik> sviKorisnici = (Dictionary<string, Korisnik>)Session["korisnici"];
            for (int i = 0; i <= sviPacijenti.Count() - 1; i++)
            {
                if (sviPacijenti[i].Email == email)
                {
                    Session["pacijent"] = sviKorisnici[kime];
                    Session["greska"] = "Email mora biti unikatan";
                    return RedirectToAction("EditPacijent");
                }
            }
            foreach (var k in sviKorisnici.Values)
            {
                if (k.KorisnickoIme == kime)
                {
                    Korisnik korisnik = new Korisnik
                    {
                        KorisnickoIme = kime,
                        Ime = ime,
                        Sifra = sifra,
                        DatumRodjenja = DateTime.ParseExact(datum, "dd/MM/yyyy", CultureInfo.CurrentCulture).Date,
                        Email = email,
                        Prezime = prezime
                    };
                    sviKorisnici.Remove(kime);
                    sviKorisnici.Add(korisnik.KorisnickoIme,korisnik);
                    Session["korisnici"] = sviKorisnici;
                    break;

                    //Pacijent pacijent = new Pacijent
                    //{
                    //    KorisnickoIme = kime,
                    //    Ime = ime,
                    //    Sifra = sifra,
                    //    DatumRodjenja = DateTime.ParseExact(datum, "dd/MM/yyyy", CultureInfo.CurrentCulture).Date,
                    //    Email = email,
                    //    Prezime = prezime,
                    //    JMBG = jmbg
                    //};
                    //for (int i = 0; i <= sviPacijenti.Count() - 1; i++)
                    //{
                    //    if (sviPacijenti[i].KorisnickoIme == kime)
                    //    {
                    //        sviPacijenti[i] = pacijent;
                    //    }
                    //}
                }
            }

            string filePacijenti = Server.MapPath("~/App_Data/pacijenti.csv");
            string[] allLinesPacijenti = System.IO.File.ReadAllLines(filePacijenti);
            for (int i = 0; i < allLinesPacijenti.Length; i++)
            {
                string[] fields = allLinesPacijenti[i].Split(';');
                if (fields[0] == kime) 
                {

                    allLinesPacijenti[i] = $"{kime};{sifra};{Type.Pacijent};{ime};{prezime};{datum};{email};{jmbg}";
                    break;
                }
            }
            System.IO.File.WriteAllLines(filePacijenti, allLinesPacijenti);

            string fileKorisnici = Server.MapPath("~/App_Data/korisnici.csv");
            string[] allLinesKorisnici = System.IO.File.ReadAllLines(fileKorisnici);
            for (int i = 0; i < allLinesKorisnici.Length; i++)
            {
                string[] fields = allLinesKorisnici[i].Split(';');
                if (fields[0] == kime)
                {

                    allLinesKorisnici[i] = $"{kime};{sifra};{Type.Pacijent};{ime};{prezime};{datum};{email}";
                    break;
                }
            }
            System.IO.File.WriteAllLines(fileKorisnici, allLinesKorisnici);

            ViewBag.pacijenti = sviPacijenti;
            ViewBag.poruka = $"Uspesno promenjen korisnik {kime}";
            //Session["pacijenti"] = sviPacijenti;
            Session["pacijent"] = null;
            Session["greska"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["pacijenti"] = null;
            Session["korisnici"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Prijava");           
        }
    }
}