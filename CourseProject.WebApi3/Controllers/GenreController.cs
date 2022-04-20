﻿using Course_Project.Logic.Interfaces;
using Course_Project.Logic.Models;
using CourseProject.Web.Shared.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CourseProject.WebApi3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreManager _genreManager;

        public GenreController(IGenreManager genreManager)
        {
            _genreManager = genreManager ?? throw new ArgumentNullException(nameof(genreManager));
        }

        [HttpPost("addGenre")]
        public async Task<IActionResult> CreateAsync([FromBody] GenreCreateRequest request)
        {
            GenreDto genreDto = new()
            {
                Genres = request.Genres
            };

            if (ModelState.IsValid)
            {
                await _genreManager.CreateAsync(genreDto);
            }

            return Ok();
        }

        [HttpGet("allGenre")]
        public async Task<IActionResult> GetAllActor()
        {
            var genres = await _genreManager.GetAllAsync();

            return Ok(genres);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _genreManager.DeleteAsync(id);
        }
    }
}
