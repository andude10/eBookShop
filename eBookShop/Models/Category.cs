﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eBookShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
