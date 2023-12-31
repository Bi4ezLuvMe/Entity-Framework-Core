﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Coach
    {
        public Coach()
        {
            Footballers = new List<Footballer>();
        }
        [Key]
        public int Id { get; set; }
        [Required,MinLength(2),MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string Nationality  { get; set; }
        public ICollection<Footballer> Footballers { get; set; }
    }
}
