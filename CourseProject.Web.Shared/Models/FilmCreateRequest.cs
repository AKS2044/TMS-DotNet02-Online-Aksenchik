﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Web.Shared.Models
{
    public class FilmCreateRequest
    {
        /// <summary>
        /// Name.
        /// </summary>
        [Required]
        public string NameFilms { get; set; }

        /// <summary>
        /// Age limit.
        /// </summary>
        [Required]
        public int AgeLimit { get; set; }

        /// <summary>
        /// Identification rating kinopoisk.
        /// </summary>
        [Required]
        public int IdRating { get; set; }

        /// <summary>
        /// Rating kinopoisk.
        /// </summary>
        [Required]
        public string RatingKinopoisk { get; set; }

        /// <summary>
        /// Rating site.
        /// </summary>
        [Required]
        public string RatingSite { get; set; }

        /// <summary>
        /// Rating Imdb.
        /// </summary>
        [Required]
        public string RatingImdb { get; set; }

        /// <summary>
        /// Release date.
        /// </summary>
        [Required]
        public int ReleaseDate { get; set; }

        /// <summary>
        /// Country identification.
        /// </summary>
        [Required]
        public List<int> CountryIds { get; set; }

        /// <summary>
        /// Actor identification.
        /// </summary>
        [Required]
        public List<int> ActorIds { get; set; }

        /// <summary>
        /// Genre identification.
        /// </summary>
        [Required]
        public List<int> GenreIds { get; set; }

        /// <summary>
        /// Stage manager identification.
        /// </summary>
        [Required]
        public List<int> StageManagerIds { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Time.
        /// </summary>
        [Required]
        public int Time { get; set; }

        /// <summary>
        /// Path to file.
        /// </summary>
        [Required]
        public string PathPoster { get; set; }

        /// <summary>
        /// Image name.
        /// </summary>
        [Required]
        public string ImageName { get; set; }
    }
}
