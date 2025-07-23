using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using ManejoPlus.Models;

namespace ManejoPlus.Controllers
{
    public class PlataformasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7149/api/Plataforma"; // Cambia el puerto si es necesario

        public PlataformasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: Plataformas
        public async Task<IActionResult> Index()
        {
            var plataformas = await _httpClient.GetFromJsonAsync<List<Plataforma>>(_apiUrl);
            return View(plataformas);
        }

        // GET: Plataformas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var plataforma = await response.Content.ReadFromJsonAsync<Plataforma>();
            if (plataforma == null) return NotFound();

            return View(plataforma);
        }

        // GET: Plataformas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plataformas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plataforma plataforma)
        {
            if (!ModelState.IsValid)
            {
                return View(plataforma);
            }

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, plataforma);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear la plataforma.");
                return View(plataforma);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Plataformas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var plataforma = await _httpClient.GetFromJsonAsync<Plataforma>($"{_apiUrl}/{id}");
            if (plataforma == null) return NotFound();

            return View(plataforma);
        }

        // POST: Plataformas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plataforma plataforma)
        {
            if (id != plataforma.PlataformaID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(plataforma);
            }

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", plataforma);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar la plataforma.");
                return View(plataforma);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Plataformas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var plataforma = await _httpClient.GetFromJsonAsync<Plataforma>($"{_apiUrl}/{id}");
            if (plataforma == null) return NotFound();

            return View(plataforma);
        }

        // POST: Plataformas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo eliminar la plataforma.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
