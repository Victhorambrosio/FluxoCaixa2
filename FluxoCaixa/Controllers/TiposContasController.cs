using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FluxoCaixa.Data;
using FluxoCaixa.Models;

namespace FluxoCaixa.Controllers
{
    public class TiposContasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposContasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposContas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoConta.ToListAsync());
        }

        // GET: TiposContas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoConta = await _context.TipoConta
                .FirstOrDefaultAsync(m => m.TipoContaId == id);
            if (tipoConta == null)
            {
                return NotFound();
            }

            return View(tipoConta);
        }

        // GET: TiposContas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposContas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoContaId,Nome,Tipo")] TipoConta tipoConta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoConta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoConta);
        }

        // GET: TiposContas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoConta = await _context.TipoConta.FindAsync(id);
            if (tipoConta == null)
            {
                return NotFound();
            }
            return View(tipoConta);
        }

        // POST: TiposContas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoContaId,Nome,Tipo")] TipoConta tipoConta)
        {
            if (id != tipoConta.TipoContaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoConta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoContaExists(tipoConta.TipoContaId))
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
            return View(tipoConta);
        }

        // GET: TiposContas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoConta = await _context.TipoConta
                .FirstOrDefaultAsync(m => m.TipoContaId == id);
            if (tipoConta == null)
            {
                return NotFound();
            }

            return View(tipoConta);
        }

        // POST: TiposContas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoConta = await _context.TipoConta.FindAsync(id);
            if (tipoConta != null)
            {
                _context.TipoConta.Remove(tipoConta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoContaExists(int id)
        {
            return _context.TipoConta.Any(e => e.TipoContaId == id);
        }
    }
}
