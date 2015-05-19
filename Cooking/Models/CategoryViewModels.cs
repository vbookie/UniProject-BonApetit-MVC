﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cooking.Models
{
    public class CreateCategoryViewModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}