﻿using CourseProject.Mvc2.Interfaces;
using CourseProject.Mvc2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Mvc2.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IFilmService _filmService;

        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="filmService">Film Service.</param>
        public HomeController(IFilmService filmService)
        {
            _filmService = filmService ?? throw new ArgumentNullException(nameof(filmService));
        }

        /// <summary>
        /// Index film (Get).
        /// </summary>
        /// <param name="page">Page number.</param>
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _filmService.GetAllShortAsync();
            int pageSize = 4;
            var count = result.Count();
            var items = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new(count, page, pageSize);
            IndexViewModel viewModel = new()
            {
                PageViewModel = pageViewModel,
                FilmShortModelResponses = items
            };

            var genreCollection = await _filmService.GetAllGenreAsync();
            var resultRandomFilm = await _filmService.GetRandomFilmByIdAsync();

            ViewBag.RandomFilm = resultRandomFilm.Id;
            ViewBag.Genres = genreCollection;
            ViewBag.Films = result;
            return View(viewModel);
        }

        /// <summary>
        /// Search film (Get).
        /// </summary>
        /// <param name="name">Name search.</param>
        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            var filmCollection = await _filmService.GetAllShortAsync();
            var genreCollection = await _filmService.GetAllGenreAsync();
            var resultRandomFilm = await _filmService.GetRandomFilmByIdAsync();

            ViewBag.RandomFilm = resultRandomFilm.Id;
            ViewBag.Genres = genreCollection;
            ViewBag.Films = filmCollection;
            var result = await _filmService.GetByNameAsync(name);

            return View(result);
        }
    }
}
