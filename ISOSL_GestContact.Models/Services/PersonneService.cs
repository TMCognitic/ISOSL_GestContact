using ISOSL_GestContact.Models.Entities;
using ISOSL_GestContact.Models.Mappers;
using ISOSL_GestContact.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.Connection.Database;

namespace ISOSL_GestContact.Models.Services
{
    public class PersonneService : IPersonneRepository
    {
        private readonly IConnection _connection;

        public PersonneService(IConnection connection)
        {
            _connection = connection;
        }        

        public IEnumerable<Personne> Get()
        {
            Command command = new Command("SELECT Id, Nom, Prenom, Pays FROM Contact", false);
            return _connection.ExecuteReader(command, (dr) => dr.ToPersonne());            
        }
        public int Insert(Personne entity)
        {
            Command command = new Command("INSERT INTO Contact (Nom, Prenom, Pays) OUTPUT inserted.Id VALUES (@Nom, @Prenom, @Pays);", false);
            command.AddParameter("Nom", entity.Nom);
            command.AddParameter("Prenom", entity.Prenom);
            command.AddParameter("Pays", entity.Pays);
            return (int)_connection.ExecuteScalar(command);
        }
        public void Update(Personne entity)
        {
            Command command = new Command("UPDATE Contact SET Nom = @Nom, Prenom = @Prenom, Pays = @Pays WHERE Id = @Id;", false);
            command.AddParameter("Id", entity.Id);
            command.AddParameter("Nom", entity.Nom);
            command.AddParameter("Prenom", entity.Prenom);
            command.AddParameter("Pays", entity.Pays);
            _connection.ExecuteNonQuery(command);
        }
        public void Delete(int id)
        {
            Command command = new Command("DELETE FROM Contact WHERE Id = @Id;", false);
            command.AddParameter("Id", id);
            _connection.ExecuteNonQuery(command);
        }
    }
}
