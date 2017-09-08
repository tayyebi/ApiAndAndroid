using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public partial class Person
    {
        [NotMapped]
        public string Base64Image { get; set; }
    }
}