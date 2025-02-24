﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodieTime.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string? ProfilePictureUrl { get; set; }

        //Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
