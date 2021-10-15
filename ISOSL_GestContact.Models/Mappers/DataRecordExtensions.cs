using ISOSL_GestContact.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ISOSL_GestContact.Models.Mappers
{
    internal static class DataRecordExtensions
    {
        internal static Personne ToPersonne(this IDataRecord dataRecord)
        {
            return new Personne()
            {
                Id = (int)dataRecord["Id"],
                Nom = (string)dataRecord["Nom"],
                Prenom = (string)dataRecord["Prenom"],
                Pays = (string)dataRecord["Pays"]
                //Si le champs est nullable en DB
                //Pays = dataRecord["Pays"] is DBNull ? null : (string)dataRecord["Pays"]
            };
        }
    }
}
