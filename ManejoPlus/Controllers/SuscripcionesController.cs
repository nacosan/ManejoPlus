using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManejoPlus.Models;
namespace ManejoPlus.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly string _apiUrl = "http://localhost:5276/api/Suscripcion";
        public SuscripcionesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }
        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
            var suscripciones = await _httpClient.GetFromJsonAsync<List<Suscripcion>>(_apiUrl);
            return View(suscripciones);
        }
        // GET: Suscripciones/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var suscripcion = await response.Content.ReadFromJsonAsync<Suscripcion>();
            return View(suscripcion);
        }
        // GET: Suscripciones/Create
        public IActionResult Create()
        {
            SetSelectLists();
            return View();
        }
        // POST: Suscripciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Suscripcion suscripcion)
        {
            if (!ModelState.IsValid)
            {
                SetSelectLists();
                return View(suscripcion);
            }
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, suscripcion);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear la suscripción.");
                SetSelectLists();
                return View(suscripcion);
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Suscripciones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var suscripcion = await _httpClient.GetFromJsonAsync<Suscripcion>($"{_apiUrl}/{id}");
            if (suscripcion == null) return NotFound();
            SetSelectLists(suscripcion.PlanID, suscripcion.PlataformaID, suscripcion.Estado);
            return View(suscripcion);
        }
        // POST: Suscripciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Suscripcion suscripcion)
        {
            if (id != suscripcion.SubscriptionID) return NotFound();
            if (!ModelState.IsValid)
            {
                SetSelectLists(suscripcion.PlanID, suscripcion.PlataformaID, suscripcion.Estado);
                return View(suscripcion);
            }
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", suscripcion);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar la suscripción.");
                SetSelectLists();
                return View(suscripcion);
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Suscripciones/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var suscripcion = await _httpClient.GetFromJsonAsync<Suscripcion>($"{_apiUrl}/{id}");
            if (suscripcion == null) return NotFound();
            return View(suscripcion);
        }
        // POST: Suscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo eliminar la suscripción.";
            }
            return RedirectToAction(nameof(Index));
        }
        private void SetSelectLists(int? selectedPlanId = null, int? selectedPlataformaId = null, string estado = null)
        {
            var planes = _context.Planes
                .Select(p => new
                {
                    p.PlanID,
                    Texto = p.Nombre 
                })
                .ToList();
            ViewData["PlanID"] = new SelectList(planes, "PlanID", "Texto", selectedPlanId);
            ViewData["PlataformaID"] = new SelectList(_context.Plataformas, "PlataformaID", "Nombre", selectedPlataformaId);
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Nombre");
            ViewData["Estado"] = new SelectList(new List<string> { "Activo", "Inactivo", "Cancelado" }, estado);
        }
    }
}
