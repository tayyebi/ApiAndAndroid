namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Person
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string ContentType { get; set; }
    }
}
