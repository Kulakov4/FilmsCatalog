﻿using FilmsCatalog.Extensions;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using FilmsCatalog.Services;
using FilmsCatalog.ViewModels;
using FilmsCatalog.ViewModels.Film;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly ICrudService<Film> filmService;
        private readonly UserManager<User> userManager;
        private int pageSize = 3;   // количество элементов на странице

        public FilmController(ICrudService<Film> filmService, UserManager<User> userManager)
        {
            this.filmService = filmService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("{controller}/Delete/{filmId}")]
        public async Task<IActionResult> Del(int filmId, int page = 1)
        {
            if (filmId <= 0)
                return BadRequest();

            try
            {
                await filmService.Delete(filmId);
                await filmService.Save();

            }
            catch
            {
                return BadRequest();
            }

            return await List(page);
        }

        [HttpGet]
        [Route("{controller}/Info/{filmId}")]
        public async Task<IActionResult> Info(int filmId, int page = 1)
        {
            if (filmId <= 0)
                return BadRequest();

            var film = await filmService.Get(filmId);

            if (film == null)
                return BadRequest();

            var userId = userManager.GetUserId(User);

            var viewModel = new FilmInfoViewModel();
            viewModel.CopyProperties(film);
            viewModel.Page = page;
            viewModel.ReadOnly = film.UserId != userId;

            return View(viewModel);
        }

        private async Task<IActionResult> Page(IQueryable<Film> allFilms, int page, int pageSize)
        {
            var items = await allFilms.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var count = await allFilms.CountAsync();
            var pageViewModel = new PageViewModel(count, page, pageSize);
            var viewModel = new FilmListViewModel { Films = items, PageViewModel = pageViewModel };
            return View(nameof(List), viewModel);
        }

        [HttpGet]
        [Route("{controller}/List/{filmId}")]
        public async Task<IActionResult> ListById(int filmId)
        {
            if (filmId <= 0)
                return BadRequest();

            int page = 1;
            var allFilms = filmService.GetAll().OrderBy(f => f.Name);

            bool ok = false;
            int i = 0;
            foreach (var film in allFilms)
            {
                if (++i > pageSize)
                {
                    page++;
                    i = 1;
                }
                if (film.Id == filmId)
                {
                    ok = true;
                    break;
                }
            }

            if (!ok)
                page = 1;

            return await Page(allFilms, page, pageSize);
        }

        [HttpGet]
        public async Task<IActionResult> List(int page = 1)  // список всех фильмов
        {
            var allFilms = filmService.GetAll().OrderBy(f => f.Name);
            return await Page(allFilms, page, pageSize);
        }

        [HttpGet]
        [Route("{controller}/Edit/{filmId}")]
        public async Task<IActionResult> Edit(int filmId)  // Редактирование фильма
        {
            if (filmId <= 0)
                return BadRequest();

            var film = await filmService.Get(filmId);

            if (film == null)
                return BadRequest();

            var viewModel = new EditFilmViewModel();
            viewModel.CopyProperties(film);
            viewModel.ReturnUrl = Url.Action(nameof(Info), new { Id = filmId });

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(int page = 1)  // Добавление нового фильма
        {
            var p = page;
            return View(nameof(Edit), new EditFilmViewModel() { ReturnUrl = Url.Action(nameof(List), new { page = p }) });
        }

        private byte[] ReadPoster(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var imgService = new ImageService<PosterImageProfile>();
            try
            {
                imgService.ValidateImage(file);

                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    return binaryReader.ReadBytes((int)file.Length);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("PosterFile", ex.Message);
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditFilmViewModel model)
        {
            if (model == null)
                return BadRequest();

            var film = (model.Id > 0) ? await filmService.Get(model.Id) : new Film();

            // Если в БД фильма с таким идентификатором уже нет
            if (model.Id > 0 && film == null)
                return NotFound();

            var userId = userManager.GetUserId(User);
            // Если редактирует не тот пользователь, который создавал
            if (model.Id > 0 && userId != film.UserId)
                return BadRequest();

            byte[] imageData = ReadPoster(model.PosterFile);

            if (model.Year < 1900 || model.Year > DateTime.Now.Year)
                ModelState.AddModelError("Year", "Некорректный год");

            if (!ModelState.IsValid)
            {
                var viewModel = new EditFilmViewModel();
                viewModel.CopyProperties(model);
                if (model.Id > 0)
                    viewModel.Poster = film.Poster;
                return View(viewModel);
            }

            film.CopyProperties(model);
            film.UserId = userId;
            if (imageData != null)
                film.Poster = imageData;

            var newFilm = film;
            if (model.Id == 0)
            {
                newFilm = await filmService.Insert(film);
            }

            await filmService.Save();  // тут в newFilm.Id записывается новый идентификатор

            if (newFilm.Id == 0)
                return BadRequest();

            return RedirectToAction(nameof(Info), new { filmId = newFilm.Id });
        }
    }
}
