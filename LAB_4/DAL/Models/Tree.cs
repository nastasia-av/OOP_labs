using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_4.DAL.Models
{
    public class Tree
        {   
            public int Id { get; set; }
            public List<Person> People { get; set; } = new();
        }
}
