using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravlja.Models
{
    public class ZdravstveniKarton
    {
        public List<Termin> Termini {  get; set; }
        public Pacijent Pacijent { get; set; }
    }
}