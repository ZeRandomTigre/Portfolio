using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Port_Yetti.Models
{
    public class SettingPost
    {
        public int SettingNameID { get; set; }
        public int ServiceID { get; set; }

        public string Value { get; set; }
    }

    public class Setting : SettingPost
    {
        [Key]
        public int Id { get; set; }
    }
}
