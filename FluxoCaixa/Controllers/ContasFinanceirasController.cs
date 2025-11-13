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
    public class ContasFinanceirasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContasFinanceirasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContasFinanceiras
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContasFinanceiras.ToListAsync());
        }

        // GET: ContasFinanceiras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaFinanceira = await _context.ContasFinanceiras
                .FirstOrDefaultAsync(m => m.ContaFinanceiraId == id);
            if (contaFinanceira == null)
            {
                return NotFound();
            }

            return View(contaFinanceira);
        }

        // GET: ContasFinanceiras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContasFinanceiras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContaFinanceiraId,Nome,Saldo")] ContaFinanceira contaFinanceira)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contaFinanceira);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contaFinanceira);
        }

        // GET: ContasFinanceiras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaFinanceira = await _context.ContasFinanceiras.FindAsync(id);
            if (contaFinanceira == null)
            {
                return NotFound();
            }
            return View(contaFinanceira);
        }

        // POST: ContasFinanceiras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContaFinanceiraId,Nome,Saldo")] ContaFinanceira contaFinanceira)
        {
            if (id != contaFinanceira.ContaFinanceiraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contaFinanceira);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContaFinanceiraExists(contaFinanceira.ContaFinanceiraId))
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
            return View(contaFinanceira);
        }

        // GET: ContasFinanceiras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaFinanceira = await _context.ContasFinanceiras
                .FirstOrDefaultAsync(m => m.ContaFinanceiraId == id);
            if (contaFinanceira == null)
            {
                return NotFound();
            }

            return View(contaFinanceira);
        }

        // POST: ContasFinanceiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contaFinanceira = await _context.ContasFinanceiras.FindAsync(id);
            if (contaFinanceira != null)
            {
                _context.ContasFinanceiras.Remove(contaFinanceira);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContaFinanceiraExists(int id)
        {
            return _context.ContasFinanceiras.Any(e => e.ContaFinanceiraId == id);
        }
    }
}
