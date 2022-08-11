using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Task2.Models;
using Task2.service;

namespace Task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPizzaService _pizzaService;
        public HomeController(ILogger<HomeController> logger, IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pizzaHtml = await _pizzaService.GetPizzaFromHtml();
            return View(pizzaHtml);            
        }


        [HttpGet]
        public IActionResult Privacy()
        {            
            return View();
        }

        [HttpGet("home/get-all-pizza")]
        public async Task<IActionResult> GetAllPizza()
        {
            var pizzaHtml = await _pizzaService.GetPizzaFromHtml();
            return Json(pizzaHtml);
        }

        [HttpPost("home/search")]
        public async Task<IActionResult> Search([FromBody]SearchViewModel model)
        {
            var pizzaHtml = await _pizzaService.GetPizzaFromHtml();

            var result = pizzaHtml.Where(x => x.Name.ToLower().Contains(model.Name.ToLower())).ToList();

            return Json(result);
        }

        [HttpGet("home/details/{id}")]
        public async Task<IActionResult> DetailsById(int id)
        {
            var pizzaHtml = await _pizzaService.GetPizzaFromHtml();
            var model = pizzaHtml.FirstOrDefault(x => x.Id == id);

            return View("Details", model);
        }

    }
}
