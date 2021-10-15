using ISOSL_GestContact.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISOSL_GestContact.Models.Repositories
{
    public interface IPersonneRepository
    {
        IEnumerable<Personne> Get();
        int Insert(Personne entity);
        void Update(Personne entity);
        void Delete(int id);
    }
}
