﻿using System;

namespace AppCore.Entities
{
    public class Photo : BaseEntity
    {
        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}