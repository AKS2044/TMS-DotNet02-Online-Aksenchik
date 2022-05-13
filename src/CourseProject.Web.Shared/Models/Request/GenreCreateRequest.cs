﻿using System.ComponentModel.DataAnnotations;

namespace CourseProject.Web.Shared.Models.Request
{
    public class GenreCreateRequest
    {
        /// <summary>
        /// Genres.
        /// </summary>
        [Required]
        public string Genres { get; set; }
    }
}