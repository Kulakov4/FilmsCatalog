using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FilmsCatalog.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
