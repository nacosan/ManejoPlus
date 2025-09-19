using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManejoPlus.Models;

namespace ManejoPlus.Controllers
{
    public class HistorialPagoesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context; 
        private readonly string _apiUrl = "http://localhost:5276/api/HistorialPago";

        public HistorialPagoesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        // GET: HistorialPagoes
        public async Task<IActionResult> Index()
        {
            // Llamamos al endpoint de la API para traer los pagos
            var pagos = await _httpClient.GetFromJsonAsync<List<HistorialPago>>(_apiUrl);
            return View(pagos);
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
            SetSelectLists();
            return View();
        }

        // POST: HistorialPagoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistorialPago historialPago)
        {
            if (!ModelState.IsValid)
            {
                SetSelectLists(historialPago.SubscriptionID);
                return View(historialPago);
            }

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, historialPago);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear el historial de pago.");
                SetSelectLists(historialPago.SubscriptionID);
                return View(historialPago);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: HistorialPagoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pago = await _httpClient.GetFromJsonAsync<HistorialPago>($"{_apiUrl}/{id}");
            if (pago == null) return NotFound();

            SetSelectLists(pago.SubscriptionID);
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
                SetSelectLists(historialPago.SubscriptionID);
                return View(historialPago);
            }

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", historialPago);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar el historial de pago.");
                SetSelectLists(historialPago.SubscriptionID);
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
                TempData["Error"] = "No se pudo eliminar el historial de pago.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Método para cargar la lista desplegable de Suscripciones
        private void SetSelectLists(int? selectedSubscriptionID = null)
        {
            ViewData["SubscriptionID"] = new SelectList(
                _context.Suscripciones, "SubscriptionID", "NombrePersonalizado", selectedSubscriptionID);
        }
    }
}
