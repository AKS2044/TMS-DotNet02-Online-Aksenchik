﻿using CourseProject.Mvc2.Interfaces;
using CourseProject.Web.Shared.Models.Request;
using CourseProject.Web.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseProject.Mvc2.Service
{
    /// <inheritdoc cref="IFilmService"/>
    public class FilmService : IFilmService
    {
        private readonly HttpClient _httpClient;

        public FilmService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task AddAsync(object value, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Film/addfilm")
            {
                Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }
        }

        public async Task AddCountryAsync(object value, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Country/addCountry")
            {
                Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }
        }

        public async Task<IEnumerable<FilmShortModelResponse>> GetAllShortAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Film/allFilms");

            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            var films = await response.Content.ReadFromJsonAsync<List<FilmShortModelResponse>>();
            return films;
        }

        public async Task<FilmModelResponse> GetByIdAsync(int id,string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Film/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            var film = await response.Content.ReadFromJsonAsync<FilmModelResponse>();
            return film;
        }

        public async Task<FilmShortModelResponse> GetByNameAsync(string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Film/name");
            request.Content = new StringContent(JsonSerializer.Serialize(name), Encoding.UTF8, "application/json");
            //request.Content = new StringContent(name, Encoding.UTF8, "application/json");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer");

            using var response = await _httpClient.SendAsync(request);

            var film = await response.Content.ReadFromJsonAsync<FilmShortModelResponse>();
            return film;
        }

        public async Task<IEnumerable<GenreModelResponse>> GetAllGenreAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Genre/allGenre");
            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            var films = await response.Content.ReadFromJsonAsync<List<GenreModelResponse>>();
            return films;
        }

        public async Task<IEnumerable<FilmShortModelResponse>> GetByGenreIdAsync(int genreId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Film/genre");
            request.Content = new StringContent(JsonSerializer.Serialize(genreId), Encoding.UTF8, "application/json");
            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            var films = await response.Content.ReadFromJsonAsync<List<FilmShortModelResponse>>();
            return films;
        }
    }
}
