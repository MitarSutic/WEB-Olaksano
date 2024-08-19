using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace DomZdravlja.Models
{
    public class DataHelper
    {
        public static Dictionary<string,Korisnik> UcitajKorisnike(string path)
           {
            Dictionary<string, Korisnik> _users = new Dictionary<string, Korisnik>();
            string filePath = HostingEnvironment.MapPath(path);

            if (System.IO.File.Exists(filePath))
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length >= 6)
                    {
                        string kIme = parts[0];
                        string sifra = parts[1];
                        string type = parts[2];
                        string ime = parts[3];
                        string prezime = parts[4];
                        DateTime datumRodjenja = DateTime.ParseExact(parts[5],"dd/MM/yyyy",CultureInfo.InvariantCulture);
                        string email = parts[6];

                        Korisnik korisnik = null;

                        switch (type)
                        {
                            case "Pacijent":
                                korisnik = new Pacijent
                                {
                                    KorisnickoIme = kIme,
                                    Sifra = sifra,
                                    Tip = Type.Pacijent,
                                    Ime = ime,
                                    Prezime = prezime,
                                    DatumRodjenja = datumRodjenja,
                                    Email = email,
                                    JMBG = parts[7],
                                    ListaZakazanihTermina = new List<Termin>()
                                };
                                break;
                            case "Lekar":
                                korisnik = new Lekar
                                {
                                    KorisnickoIme = kIme,
                                    Sifra = sifra,
                                    Tip = Type.Lekar,
                                    Ime = ime,
                                    Prezime = prezime,
                                    DatumRodjenja = datumRodjenja,
                                    Email = email,
                                    ListaZakazanihISlobodnihTermina = new List<Termin>()
                                };
                                break;
                            case "Administrator":
                                korisnik = new Administrator
                                {
                                    KorisnickoIme = kIme,
                                    Sifra = sifra,
                                    Tip = Type.Administrator,
                                    Ime = ime,
                                    Prezime = prezime,
                                    DatumRodjenja = datumRodjenja,
                                    Email = email
                                };
                                break;
                        }

                        if (korisnik != null)
                        {
                            _users[kIme] = korisnik;
                        }
                    }
                }
            }
            return _users;
        }

        public static List<Pacijent> UcitajPacijente(string path)
        {
            List<Pacijent> pacijenti = new List<Pacijent>();
            string filePath = HostingEnvironment.MapPath(path);
            if (System.IO.File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');

                    string kIme = parts[0];
                    string sifra = parts[1];
                    string type = parts[2];
                    string ime = parts[3];
                    string prezime = parts[4];
                    DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string email = parts[6];
                    string jmbg = parts[7];

                    Pacijent pacijent = new Pacijent
                    {
                        KorisnickoIme = kIme,
                        Sifra = sifra,
                        Ime = ime,
                        Prezime = prezime,
                        DatumRodjenja = datumRodjenja,
                        Email = email,
                        JMBG = jmbg,
                        Tip = Type.Pacijent
                    };
                    pacijenti.Add(pacijent);
                }
            }

            return pacijenti;
        }

        public static List<Lekar> UcitajLekare(string path)
        {
            List<Lekar> lekari = new List<Lekar>();
            string filePath = HostingEnvironment.MapPath(path);
            if (System.IO.File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                   
                        string kIme = parts[0];
                        string sifra = parts[1];
                        string type = parts[2];
                        string ime = parts[3];
                        string prezime = parts[4];
                        DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        string email = parts[6];

                        Lekar lekar = new Lekar
                        {
                            KorisnickoIme = kIme,
                            Sifra = sifra,
                            Ime = ime,
                            Prezime = prezime,
                            DatumRodjenja = datumRodjenja,
                            Email = email,
                            Tip = Type.Pacijent
                        };
                        lekari.Add(lekar);
                    
                }
            }

            return lekari;
        }

        public static List<Termin>UcitajSlobodneTermine(string path)
        {
            List<Termin> slobodniTermini = new List<Termin>();
            string filePath = HostingEnvironment.MapPath(path);
            if (System.IO.File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    string kImeLekara = parts[0];
                    DateTime datumTermina = DateTime.ParseExact(parts[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    Termin termin = new Termin
                        {
                            kImeLekara = kImeLekara,
                            DatumIVremeZakazanogTermina = datumTermina,
                            Statustermina = StatusTermina.Slobodan
                        };
                        slobodniTermini.Add(termin);
                }
            }
            return slobodniTermini;
        }

        public static List<Termin> UcitajSlobodneIZakazaneTermine(string path)
        {
            List<Termin> slobodniIZakazaniTermini = new List<Termin>();
            string filePath = HostingEnvironment.MapPath(path);
            if (System.IO.File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    string kImeLekara = parts[0];
                    string pacijent = parts[1];
                    StatusTermina status = (StatusTermina)Enum.Parse(typeof(StatusTermina), parts[2], true);
                    DateTime datumTermina = DateTime.ParseExact(parts[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string opis = parts[4];                 
                    Termin termin = new Termin
                    {
                        kImeLekara = kImeLekara,
                        Statustermina = status,
                        ImePacijenta = pacijent,
                        DatumIVremeZakazanogTermina = datumTermina,
                        OpisTerapije = opis
                    };
                    slobodniIZakazaniTermini.Add(termin);
                }
            }
            return slobodniIZakazaniTermini;
        }

    }
}
