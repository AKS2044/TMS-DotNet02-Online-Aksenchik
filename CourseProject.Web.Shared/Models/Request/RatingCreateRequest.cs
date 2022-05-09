﻿using System.ComponentModel.DataAnnotations;

namespace CourseProject.Web.Shared.Models.Request
{
    public class RatingCreateRequest
    {
        /// <summary>
        /// Film id.
        /// </summary>
        [Required]
        public int FilmId { get; set; }

        /// <summary>
        /// Rating.
        /// </summary>
        [Required]
        public int Rating { get; set; }
    }
}
