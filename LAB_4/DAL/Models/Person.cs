using System.Collections.Generic;
using System;

namespace LAB_4.DAL.Models
{
    public class Person
    {
        public Guid Id { get; set; } 
        public string FullName { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public string Gender { get; set; } 
        public List<Guid> Relatives { get; set; } 

        public Person()
        {
            Relatives = new List<Guid>();
        }
    }
}

