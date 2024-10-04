﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor_Temp.Models
{  
        public class Category
        {
            [Key]
            public int Id { get; set; }
            [Required]
            [DisplayName("Category Name")]
            [MaxLength(30, ErrorMessage = "Ops...!!!")]
            public string Name { get; set; }
            [DisplayName("Display Order")]
            [Range(1, 100, ErrorMessage = "Ops  order must be below 100")]
            public int DisplayOrder { get; set; }
        }
  
}
