using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManejoPlus.Models;

namespace ManejoPlus.Controllers
{
    public class PlanesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context; 
        private readonly string _apiUrl = "https://localhost:7149/api/Plan"; 

        public PlanesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        // GET: Planes
        public async Task<IActionResult> Index()
        {
            var planes = await _httpClient.GetFromJsonAsync<List<Plan>>(_apiUrl);
            return View(planes);
        }

        // GET: Planes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var plan = await response.Content.ReadFromJsonAsync<Plan>();
            return View(plan);
        }

        // GET: Planes/Create
        public IActionResult Create()
        {
            CargarPlataformas();
            return View();
        }

        // POST: Planes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plan plan)
        {
            if (!ModelState.IsValid)
            {
                CargarPlataformas(plan.PlataformaID);
                return View(plan);
            }

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, plan);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "No se pudo crear el plan.");
                CargarPlataformas(plan.PlataformaID);
                return View(plan);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Planes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var plan = await _httpClient.GetFromJsonAsync<Plan>($"{_apiUrl}/{id}");
            if (plan == null) return NotFound();

            CargarPlataformas(plan.PlataformaID);
            return View(plan);
        }

        // POST: Planes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plan plan)
        {
            if (id != plan.PlanID) return NotFound();

            if (!ModelState.IsValid)
            {
                CargarPlataformas(plan.PlataformaID);
                return View(plan);
            }

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", plan);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "No se pudo actualizar el plan.");
                CargarPlataformas(plan.PlataformaID);
                return View(plan);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Planes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var plan = await _httpClient.GetFromJsonAsync<Plan>($"{_apiUrl}/{id}");
            if (plan == null) return NotFound();

            return View(plan);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo eliminar el plan.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void CargarPlataformas(int? seleccionada = null)
        {
            ViewData["PlataformaID"] = new SelectList(_context.Plataformas, "PlataformaID", "Nombre", seleccionada);
        }
    }
}
