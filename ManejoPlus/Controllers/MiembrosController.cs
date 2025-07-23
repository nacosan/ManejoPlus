using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManejoPlus.Models;

namespace ManejoPlus.Controllers
{
    public class MiembrosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context; 
        private readonly string _apiUrl = "https://localhost:7149/api/Miembro"; 

        public MiembrosController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        // GET: Miembros
        public async Task<IActionResult> Index()
        {
            var miembros = await _httpClient.GetFromJsonAsync<List<Miembro>>(_apiUrl);
            return View(miembros);
        }

        // GET: Miembros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var miembro = await response.Content.ReadFromJsonAsync<Miembro>();
            return View(miembro);
        }

        // GET: Miembros/Create
        public IActionResult Create()
        {
            SetSelectLists();
            return View();
        }

        // POST: Miembros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Miembro miembro)
        {
            if (!ModelState.IsValid)
            {
                SetSelectLists();
                return View(miembro);
            }

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, miembro);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear el miembro.");
                SetSelectLists();
                return View(miembro);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Miembros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var miembro = await _httpClient.GetFromJsonAsync<Miembro>($"{_apiUrl}/{id}");
            if (miembro == null) return NotFound();

            SetSelectLists(miembro.SubscriptionID);
            return View(miembro);
        }

        // POST: Miembros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Miembro miembro)
        {
            if (id != miembro.MiembroID) return NotFound();

            if (!ModelState.IsValid)
            {
                SetSelectLists(miembro.SubscriptionID);
                return View(miembro);
            }

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", miembro);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar el miembro.");
                SetSelectLists(miembro.SubscriptionID);
                return View(miembro);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Miembros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var miembro = await _httpClient.GetFromJsonAsync<Miembro>($"{_apiUrl}/{id}");
            if (miembro == null) return NotFound();

            return View(miembro);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo eliminar el miembro.";
            }
            return RedirectToAction(nameof(Index));
        }

        private void SetSelectLists(int? selectedSubscriptionID = null)
        {
            ViewData["SubscriptionID"] = new SelectList(_context.Suscripciones, "SubscriptionID", "ApplicationUserId", selectedSubscriptionID);
        }
    }
}
