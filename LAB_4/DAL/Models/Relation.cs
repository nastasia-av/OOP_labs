using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.DAL.Models
{
    public class Relation
    {
        public int Id { get; set; } 
        public int PersonId { get; set; } 
        public int RelatedPersonId { get; set; } 

        public RelationType RelationType { get; set; } 


    }

}
