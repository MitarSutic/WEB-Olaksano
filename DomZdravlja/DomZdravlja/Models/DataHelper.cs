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
                            Console.WriteLine(korisnik);
                        }
                    }
                }
            }
            return _users;
        }
    }
}