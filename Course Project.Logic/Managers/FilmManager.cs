﻿using Course_Project.Data.Models;
using Course_Project.Logic.Exceptions;
using Course_Project.Logic.Interfaces;
using Course_Project.Logic.Models;
using CourseProject.Web.Shared.Models.Request;
using CourseProject.Web.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Logic.Managers
{
    /// <inheritdoc cref="IFilmManager"/>
    public class FilmManager : IFilmManager
    {
        private readonly IRepositoryManager<Film> _filmRepository;
        private readonly IRepositoryManager<FilmActor> _filmActorRepository;
        private readonly IRepositoryManager<FilmCountry> _filmCountryRepository;
        private readonly IRepositoryManager<FilmGenre> _filmGenreRepository;
        private readonly IRepositoryManager<FilmStageManager> _filmStageManagerRepository;
        private readonly IRepositoryManager<State> _countryRepository;
        private readonly IRepositoryManager<Genre> _genreRepository;
        private readonly IRepositoryManager<StageManager> _stageManagerRepository;
        private readonly IRepositoryManager<Actor> _actorRepository;



        public FilmManager(IRepositoryManager<Film> filmRepository, 
                           IRepositoryManager<FilmActor> filmActorRepository, 
                           IRepositoryManager<FilmCountry> filmCountryRepository,
                           IRepositoryManager<FilmGenre> filmGenreRepository,
                           IRepositoryManager<FilmStageManager> filmStageManagerRepository,
                           IRepositoryManager<State> countryRepository,
                           IRepositoryManager<Genre> genreRepository,
                           IRepositoryManager<StageManager> stageManagerRepository,
                           IRepositoryManager<Actor> actorRepository)
        {
            _filmRepository = filmRepository ?? throw new ArgumentNullException(nameof(filmRepository));
            _filmActorRepository = filmActorRepository ?? throw new ArgumentNullException(nameof(filmActorRepository));
            _filmCountryRepository = filmCountryRepository ?? throw new ArgumentNullException(nameof(filmCountryRepository));
            _filmGenreRepository = filmGenreRepository ?? throw new ArgumentNullException(nameof(filmGenreRepository));
            _filmStageManagerRepository = filmStageManagerRepository ?? throw new ArgumentNullException(nameof(filmStageManagerRepository));
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
            _stageManagerRepository = stageManagerRepository ?? throw new ArgumentNullException(nameof(stageManagerRepository));
            _actorRepository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
        }

        public async Task CreateAsync(FilmDto filmDto, 
            List<FilmActorDto> filmActorDto, 
            List<FilmGenreDto> filmGenreDto,
            List<FilmCountryDto> filmCountryDto,
            List<FilmStageManagerDto> filmStageManagerDto)
        {
            var film = new Film()
            {
                NameFilms = filmDto.NameFilms,
                AgeLimit = filmDto.AgeLimit,
                ReleaseDate = filmDto.ReleaseDate,
                Description = filmDto.Description,
                Time = filmDto.Time,
                PathPoster = filmDto.PathPoster,
                ImageName = filmDto.ImageName,
                IdRating = filmDto.IdRating,
                RatingSite = filmDto.RatingSite,
                RatingKinopoisk = filmDto.RatingKinopoisk,
                RatingImdb = filmDto.RatingImdb,
            };

            await _filmRepository.CreateAsync(film);
            await _filmRepository.SaveChangesAsync();

            int id = film.Id;
            foreach (var item in filmActorDto)
            {
                FilmActor filmActor = new()
                {
                    FilmId = id,
                    ActorId = item.ActorId
                };
                await _filmActorRepository.CreateAsync(filmActor);
            }

            foreach (var item in filmGenreDto)
            {
                FilmGenre filmGenre = new()
                {
                    FilmId = id,
                    GenreId = item.GenreId
                };
                await _filmGenreRepository.CreateAsync(filmGenre);
            }

            foreach (var item in filmCountryDto)
            {
                FilmCountry filmCountry = new()
                {
                    FilmId = id,
                    CountryId = item.CountryId
                };
                await _filmCountryRepository.CreateAsync(filmCountry);
            }

            foreach (var item in filmStageManagerDto)
            {
                FilmStageManager filmStageManager = new()
                {
                    FilmId = id,
                    StageManagerId = item.StageManagerId
                };
                await _filmStageManagerRepository.CreateAsync(filmStageManager);
            }
            await _filmRepository.SaveChangesAsync();
        }

            public async Task DeleteAsync(int id)
        {
            var filmDelete = await _filmRepository.GetEntityAsync(p => p.Id == id);

            if (filmDelete is null)
            {
                throw new NotFoundException($"'{nameof(id)}' project not found.", nameof(id));
            }

            _filmRepository.Delete(filmDelete);
            await _filmRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<FilmDto>> GetAllShortAsync()
        {
            var FilmDtos = new List<FilmDto>();

            var films = await _filmRepository
                .GetAll().ToListAsync();

            foreach (var item in films)
            {
                FilmDtos.Add(new FilmDto
                {
                    Id = item.Id,
                    NameFilms = item.NameFilms,
                    ReleaseDate = item.ReleaseDate,
                    PathPoster = item.PathPoster
                });
            }
            return FilmDtos;
        }

        public async Task<IEnumerable<FilmDto>> GetByGenreAsync(int idGenre)
        {
            var filmGenreIds = await _filmGenreRepository.GetAll().Where(f => f.GenreId == idGenre).Select(f => f.FilmId).ToListAsync();

            var filmGenreLists = new List<FilmDto>();

            foreach (var item in filmGenreIds)
            {
                var resultFilm = await _filmRepository.GetAll().Where(f => f.Id == item).ToListAsync();
                foreach (var film in resultFilm)
                {
                    filmGenreLists.Add(new FilmDto
                    {
                        Id = film.Id,
                        NameFilms = film.NameFilms,
                        ReleaseDate = film.ReleaseDate,
                        PathPoster = film.PathPoster
                    });
                }
                
            }

            return filmGenreLists;
        }

        public async Task<FilmModelResponse> GetByIdAsync(int id)
        {
            Film film = await _filmRepository.GetEntityAsync(f => f.Id == id);
            var filmCountryIds = await _filmCountryRepository.GetAll().Where(f => f.FilmId == film.Id).Select(f => f.CountryId).ToListAsync();
            var countries = await _countryRepository.GetAll().Where(c => filmCountryIds.Contains(c.Id)).Select(c => c.Country).ToListAsync();

            var filmGenreIds = await _filmGenreRepository.GetAll().Where(f => f.FilmId == film.Id).Select(f => f.GenreId).ToListAsync();
            var genres = await _genreRepository.GetAll().Where(g => filmGenreIds.Contains(g.Id)).Select(g => g.Genres).ToListAsync();

            var filmStageManagerIds = await _filmStageManagerRepository.GetAll().Where(f => f.FilmId == film.Id).Select(f => f.StageManagerId).ToListAsync();
            var stageManagers = await _stageManagerRepository.GetAll().Where(m => filmStageManagerIds.Contains(m.Id)).Select(m => m.StageManagers).ToListAsync();

            var filmActorIds = await _filmActorRepository.GetAll().Where(f => f.FilmId == film.Id).Select(f => f.ActorId).ToListAsync();
            var actors = await _actorRepository.GetAll().Where(a => filmActorIds.Contains(a.Id)).Select(a =>  new Actor
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
            }).ToListAsync();

            FilmModelResponse model = new()
            {
                Id = film.Id,
                NameFilms = film.NameFilms,
                ImageName = film.ImageName,
                PathPoster = film.PathPoster,
                ReleaseDate = film.ReleaseDate,
                RatingKinopoisk = film.RatingKinopoisk,
                RatingImdb = film.RatingImdb,
                RatingSite = film.RatingSite,
                AgeLimit = film.AgeLimit,
                Time = film.Time,
                Description = film.Description,
                Country = countries,
                Genre = genres,
                StageManagers = stageManagers,
                Actors = actors
            };

            return model;
        }

        public async Task<FilmShortModelResponse> GetByNameAsync(string nameSearch) //Сделать поиск по сайту
        {
            Film film = await _filmRepository.GetAll().FirstOrDefaultAsync(f => f.NameFilms == nameSearch);

            if (film is null)
            {
                throw new NotFoundException($"'{nameof(film.NameFilms)}' film not found.", nameof(film.NameFilms));
            }

            FilmShortModelResponse model = new()
            {
                Id = film.Id,
                NameFilms = film.NameFilms,
                PathPoster = film.PathPoster,
                ReleaseDate = film.ReleaseDate,
            };

            return model;
        }

        public async Task UpdateAsync(FilmDto model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            var filmUpdate = await _filmRepository.GetEntityAsync(p => p.Id == model.Id);

            if (filmUpdate is null)
            {
                throw new NotFoundException($"'{nameof(model.Id)}' project not found.", nameof(model.Id));
            }

            if (filmUpdate.NameFilms != model.NameFilms)
            {
                filmUpdate.NameFilms = model.NameFilms;
            }
        }
    }
}
