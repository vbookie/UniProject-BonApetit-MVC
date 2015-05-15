using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cooking.Data.Models
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public Image()
        {
            this.Id = Guid.NewGuid();
        }
    }
}