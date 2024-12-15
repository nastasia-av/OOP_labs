using System;

namespace LAB_4.DAL.Models
{
    public class Relationship
    {
        public Guid PersonId { get; set; } 
        public Guid RelatedPersonId { get; set; } 
        public string RelationType { get; set; } 

        public Relationship(Guid personId, Guid relatedPersonId, string relationType)
        {
            PersonId = personId;
            RelatedPersonId = relatedPersonId;
            RelationType = relationType;
        }
    }
}

