using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManejoPlus.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Suscripciones.Include(s => s.Plan).Include(s => s.Plataforma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Suscripciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones
                .Include(s => s.Plan)
                .Include(s => s.Plataforma)
                .FirstOrDefaultAsync(m => m.SubscriptionID == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // GET: Suscripciones/Create
        public IActionResult Create()
        {
            ViewData["PlanID"] = new SelectList(_context.Planes, "PlanID", "Nombre");
            ViewData["PlataformaID"] = new SelectList(_context.Plataformas, "PlataformaID", "Nombre");
            return View();
        }

        // POST: Suscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubscriptionID,PlataformaID,PlanID,ApplicationUserId,NombrePersonalizado,Tipo,Descripcion,FechaInicio,FechaFin,Estado")] Suscripcion suscripcion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanID"] = new SelectList(_context.Planes, "PlanID", "Nombre", suscripcion.PlanID);
            ViewData["PlataformaID"] = new SelectList(_context.Plataformas, "PlataformaID", "Nombre", suscripcion.PlataformaID);
            return View(suscripcion);
        }

        // GET: Suscripciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            ViewData["PlanID"] = new SelectList(_context.Planes, "PlanID", "Nombre", suscripcion.PlanID);
            ViewData["PlataformaID"] = new SelectList(_context.Plataformas, "PlataformaID", "Nombre", suscripcion.PlataformaID);
            return View(suscripcion);
        }

        // POST: Suscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubscriptionID,PlataformaID,PlanID,ApplicationUserId,NombrePersonalizado,Tipo,Descripcion,FechaInicio,FechaFin,Estado")] Suscripcion suscripcion)
        {
            if (id != suscripcion.SubscriptionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.SubscriptionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanID"] = new SelectList(_context.Planes, "PlanID", "Nombre", suscripcion.PlanID);
            ViewData["PlataformaID"] = new SelectList(_context.Plataformas, "PlataformaID", "Nombre", suscripcion.PlataformaID);
            return View(suscripcion);
        }

        // GET: Suscripciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones
                .Include(s => s.Plan)
                .Include(s => s.Plataforma)
                .FirstOrDefaultAsync(m => m.SubscriptionID == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // POST: Suscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion != null)
            {
                _context.Suscripciones.Remove(suscripcion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuscripcionExists(int id)
        {
            return _context.Suscripciones.Any(e => e.SubscriptionID == id);
        }
    }
}
