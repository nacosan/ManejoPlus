using System.Net.Http;
using System.Net.Http.Json;
using ManejoPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManejoPlus.Controllers
{
    public class HistorialPagoesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context; 
        private readonly string _apiUrl = "https://localhost:7149/api/HistorialPago"; 

        public HistorialPagoesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        // GET: HistorialPagoes
        public async Task<IActionResult> Index()
        {
            var planes = await _context.Planes
                .Include(p => p.Plataforma)
                .ToListAsync();

            return View(planes); 
        }


        // GET: HistorialPagoes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var pago = await response.Content.ReadFromJsonAsync<HistorialPago>();
            return View(pago);
        }

        // GET: HistorialPagoes/Create
        public IActionResult Create()
        {
            CargarSuscripciones();
            return View();
        }

        // POST: HistorialPagoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistorialPago historialPago)
        {
            if (!ModelState.IsValid)
            {
                CargarSuscripciones(historialPago.SubscriptionID);
                return View(historialPago);
            }

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, historialPago);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear el historial de pago.");
                CargarSuscripciones(historialPago.SubscriptionID);
                return View(historialPago);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: HistorialPagoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pago = await _httpClient.GetFromJsonAsync<HistorialPago>($"{_apiUrl}/{id}");
            if (pago == null) return NotFound();

            CargarSuscripciones(pago.SubscriptionID);
            return View(pago);
        }

        // POST: HistorialPagoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HistorialPago historialPago)
        {
            if (id != historialPago.PagoID) return NotFound();

            if (!ModelState.IsValid)
            {
                CargarSuscripciones(historialPago.SubscriptionID);
                return View(historialPago);
            }

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", historialPago);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar el historial de pago.");
                CargarSuscripciones(historialPago.SubscriptionID);
                return View(historialPago);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: HistorialPagoes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var pago = await _httpClient.GetFromJsonAsync<HistorialPago>($"{_apiUrl}/{id}");
            if (pago == null) return NotFound();

            return View(pago);
        }

        // POST: HistorialPagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo eliminar el historial.";
            }
            return RedirectToAction(nameof(Index));
        }

        private void CargarSuscripciones(int? seleccionada = null)
        {
            ViewData["SubscriptionID"] = new SelectList(_context.Suscripciones, "SubscriptionID", "NombrePersonalizado", seleccionada);
        }
    }
}
