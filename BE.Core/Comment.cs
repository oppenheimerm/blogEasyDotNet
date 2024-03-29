﻿
using System.ComponentModel.DataAnnotations;

namespace BE.Core
{
    public class Comment
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public string? Author { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(400)]
        public string? Content { get; set; }

        [Required]
        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public bool CommentApproved { get; set; } = false;

        public bool IsAdmin { get; set; }

        public int PostId { get; set; }

        public Post? Post { get; set; }
    }
}
