using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace DomZdravlja.Models
{
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Sifra { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }
        public Type Tip { get; set; }

        public override string ToString()
        {
            return $"Korisnicko ime: {KorisnickoIme}, Ime: {Ime}, Prezime: {Prezime}, Datum rodjenja: {DatumRodjenja.ToString("dd/MM/yyyy")}, Email: {Email}, Tip: {Tip}";
        }
    }   
}