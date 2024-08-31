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
                        DateTime datumRodjenja = DateTime.ParseExact(parts[5],"dd/MM/yyyy",CultureInfo.CurrentCulture);
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
                                    //JMBG = parts[7],
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
                    DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.CurrentCulture);
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

        public static List<Administrator> UcitajAdmine(string path)
        {
            List<Administrator> admini = new List<Administrator>();
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
                    DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.CurrentCulture);
                    string email = parts[6];

                    Administrator admin = new Administrator
                    {
                        KorisnickoIme = kIme,
                        Sifra = sifra,
                        Ime = ime,
                        Prezime = prezime,
                        DatumRodjenja = datumRodjenja,
                        Email = email,
                        Tip = Type.Administrator
                    };
                    admini.Add(admin);
                }
            }

            return admini;
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
                        DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.CurrentCulture);
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

        public static List<Termin>UcitajSlobodneTermine(string pathTermini,string pathLekari)
        {
            List<Termin> slobodniTermini = new List<Termin>();
            string filePathTermini = HostingEnvironment.MapPath(pathTermini);
            string filePathLekari = HostingEnvironment.MapPath(pathLekari);

            Dictionary<string, Lekar> lekari = new Dictionary<string, Lekar>();
            if (System.IO.File.Exists(filePathLekari))
            {
                string[] lekarLines = File.ReadAllLines(filePathLekari);
                foreach (string lekarLine in lekarLines)
                {
                    string[] parts = lekarLine.Split(';');

                    string kIme = parts[0];
                    string sifra = parts[1];
                    string type = parts[2];
                    string ime = parts[3];
                    string prezime = parts[4];
                    DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.CurrentCulture);
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
                    lekari[kIme] = lekar;
                }
            }

            if (System.IO.File.Exists(filePathTermini))
            {
                string[] lines = File.ReadAllLines(filePathTermini);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    string kImeLekara = parts[0];
                    DateTime datumTermina = DateTime.ParseExact(parts[1], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);

                    Termin termin = new Termin
                    {
                        kImeLekara = kImeLekara,
                        DatumIVremeZakazanogTermina = datumTermina,
                        Statustermina = StatusTermina.Slobodan
                    };

                    if (lekari.ContainsKey(kImeLekara))
                    {
                        termin.Lekar = lekari[kImeLekara];
                    }

                    slobodniTermini.Add(termin);
                }
            }

            return slobodniTermini;
        }

        public static List<Termin> UcitajSlobodneIZakazaneTermine(string pathTermini, string pathLekari)
        {
            List<Termin> slobodniIZakazaniTermini = new List<Termin>();
            string filePathTermini = HostingEnvironment.MapPath(pathTermini);
            string filePathLekari = HostingEnvironment.MapPath(pathLekari);

            // Load doctors from lekari.csv
            Dictionary<string, Lekar> lekari = new Dictionary<string, Lekar>();
            if (System.IO.File.Exists(filePathLekari))
            {
                string[] lekarLines = File.ReadAllLines(filePathLekari);
                foreach (string lekarLine in lekarLines)
                {
                    string[] parts = lekarLine.Split(';');
                    string kIme = parts[0];
                    string sifra = parts[1];
                    string type = parts[2];
                    string ime = parts[3];
                    string prezime = parts[4];
                    DateTime datumRodjenja = DateTime.ParseExact(parts[5], "dd/MM/yyyy", CultureInfo.CurrentCulture);
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
                    lekari[kIme] = lekar;
                }
            }
            if (System.IO.File.Exists(filePathTermini))
            {
                string[] lines = File.ReadAllLines(filePathTermini);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 3)
                    {
                        string kImeLekara = parts[0];
                        DateTime datumTermina = DateTime.ParseExact(parts[1], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                        StatusTermina status = (StatusTermina)Enum.Parse(typeof(StatusTermina), parts[2], true);
                        Termin termin = new Termin
                        {
                            kImeLekara = kImeLekara,
                            Statustermina = status,
                            ImePacijenta = String.Empty,
                            DatumIVremeZakazanogTermina = datumTermina,
                            OpisTerapije = String.Empty
                        };
                        if (lekari.ContainsKey(kImeLekara))
                        {
                            termin.Lekar = lekari[kImeLekara];
                        }

                        slobodniIZakazaniTermini.Add(termin);
                    }
                    else
                    {
                        string kImeLekara = parts[0];
                        string pacijent = parts[1];
                        StatusTermina status = (StatusTermina)Enum.Parse(typeof(StatusTermina), parts[2], true);
                        DateTime datumTermina = DateTime.ParseExact(parts[3], "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                        string opis = parts.Length > 4 ? parts[4] : string.Empty;
                        Termin termin = new Termin
                        {
                            kImeLekara = kImeLekara,
                            Statustermina = status,
                            ImePacijenta = pacijent,
                            DatumIVremeZakazanogTermina = datumTermina,
                            OpisTerapije = opis
                        };
                        if (lekari.ContainsKey(kImeLekara))
                        {
                            termin.Lekar = lekari[kImeLekara]; // Assign the corresponding Lekar object
                        }

                        slobodniIZakazaniTermini.Add(termin);
                    }
                    // Match the doctor from lekari.csv using kImeLekara
                    
                }
            }

            return slobodniIZakazaniTermini;
        }
    }
}
