using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazingCrud.Shared.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public Company Company { get; set; }
    }
}
