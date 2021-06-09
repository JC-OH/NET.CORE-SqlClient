using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperMicroORM.Model
{
    [Table("tb_country")]
    public class CountryModel
    {
        [Key]
        public int Id { get; set; }
        public string Country { get; set; }
        public bool Active { get; set; }
    }
}
