using System;

namespace ISOSL_GestContact.Models.Entities
{
    public class Personne
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Pays { get; set; }
    }
}
