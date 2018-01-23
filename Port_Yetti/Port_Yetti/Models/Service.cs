using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Port_Yetti.Models
{
    public class ServicePost
    {
        public string Name { get; set; }
    }

    public class Service : ServicePost
    {
        [Key]
        public int Id { get; set; }
    }
}
