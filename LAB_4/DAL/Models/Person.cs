using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.DAL.Models
{
    public class Person
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string MiddleName { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public Gender Gender { get; set; } 
        public List<Relation> Relations { get; set; } = new(); 
    }
}
