using System.Collections.Generic;
using System;
using LAB_4.DAL.Models;
using LAB_4.DAL.Repositories;
using System.Linq;

namespace LAB_4.BLL.Services
{
    public class GenealogyTreeService
    {
        private readonly GenealogyTreeRepository _repository;

        public GenealogyTreeService(GenealogyTreeRepository repository)
        {
            _repository = repository;
        }

        public void AddPerson(string fullName, DateTime dateOfBirth, string gender)
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                Gender = gender
            };
            _repository.People.Add(person);
        }

        public void AddRelationship(Guid personId, Guid relatedPersonId, string relationType)
        {
            var relationship = new Relationship(personId, relatedPersonId, relationType);
            _repository.Relationships.Add(relationship);
        }

        public List<Person> GetRelatives(Guid personId)
        {
            var relativesIds = _repository.Relationships
                .Where(r => r.PersonId == personId)
                .Select(r => r.RelatedPersonId);

            return _repository.People
                .Where(p => relativesIds.Contains(p.Id))
                .ToList();
        }

        public void SaveTree() => _repository.SaveData();
    }
}
