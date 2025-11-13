using FluxoCaixa.Data;
using FluxoCaixa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoCaixa.Controllers
{
    public class FluxosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public FluxosController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Fluxos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fluxo.Include(f => f.Conta).Include(f => f.ContaFinanceira).Include(f => f.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fluxos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fluxo = await _context.Fluxo
                .Include(f => f.Conta)
                .Include(f => f.ContaFinanceira)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.FluxoId == id);
            if (fluxo == null)
            {
                return NotFound();
            }

            return View(fluxo);
        }

        // GET: Fluxos/Create
        public IActionResult Create()
        {
            ViewData["ContaId"] = new SelectList(_context.Conta, "ContaId", "Nome");
            ViewData["ContaFinanceiraId"] = new SelectList(_context.ContasFinanceiras, "ContaFinanceiraId", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Fluxos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FluxoId,DataMovimentacao,ContaFinanceiraId,ContaId,UsuarioId")] Fluxo fluxo)
        {
            if (ModelState.IsValid)
            {
                //Buscar a conta Financeira a partir do Id 
                var contaFinanceira = _context.ContasFinanceiras.FirstOrDefault(p => p.ContaFinanceiraId == fluxo.ContaFinanceiraId);

                //Buscar a Conta a partir do Id
                var conta = _context.Conta.Include(t => t.TipoConta).FirstOrDefault(p => p.ContaId == fluxo.ContaId);


                if (conta.TipoConta.Tipo == "R")
                {
                    contaFinanceira.Saldo += conta.Valor;
                }
                if (conta.TipoConta.Tipo == "P")
                {
                    if (contaFinanceira.Saldo >= conta.Valor)
                    {
                        contaFinanceira.Saldo -= conta.Valor;
                    }
                    else
                    {
                        ViewData["Alerta"] = "Saldo Insuficiente.";
                        ViewData["ContaId"] = new SelectList(_context.Conta, "ContaId", "Nome", fluxo.ContaId);
                        ViewData["ContaFinanceiraId"] = new SelectList(_context.ContasFinanceiras, "ContaFinanceiraId", "Nome", fluxo.ContaFinanceiraId);
                        ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", fluxo.UsuarioId);
                        return View(fluxo);
                    }
                }

                // Pega o Id do Usuario logado e inclui no fluxo
                fluxo.UsuarioId = _signInManager.UserManager.GetUserId(User);

                // Data de Movimentação como a data Autal
                fluxo.DataMovimentacao = DateTime.Now;

                // Marca a conta como paga
                conta.Pago = true;

                _context.Add(fluxo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaId"] = new SelectList(_context.Conta, "ContaId", "Nome", fluxo.ContaId);
            ViewData["ContaFinanceiraId"] = new SelectList(_context.ContasFinanceiras, "ContaFinanceiraId", "Nome", fluxo.ContaFinanceiraId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", fluxo.UsuarioId);
            return View(fluxo);
        }

        // GET: Fluxos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fluxo = await _context.Fluxo.FindAsync(id);
            if (fluxo == null)
            {
                return NotFound();
            }
            ViewData["ContaId"] = new SelectList(_context.Conta, "ContaId", "Nome", fluxo.ContaId);
            ViewData["ContaFinanceiraId"] = new SelectList(_context.ContasFinanceiras, "ContaFinanceiraId", "Nome", fluxo.ContaFinanceiraId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", fluxo.UsuarioId);
            return View(fluxo);
        }

        // POST: Fluxos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FluxoId,DataMovimentacao,ContaFinanceiraId,ContaId,UsuarioId")] Fluxo fluxo)
        {
            if (id != fluxo.FluxoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fluxo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FluxoExists(fluxo.FluxoId))
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
            ViewData["ContaId"] = new SelectList(_context.Conta, "ContaId", "Nome", fluxo.ContaId);
            ViewData["ContaFinanceiraId"] = new SelectList(_context.ContasFinanceiras, "ContaFinanceiraId", "Nome", fluxo.ContaFinanceiraId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", fluxo.UsuarioId);
            return View(fluxo);
        }

        // GET: Fluxos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fluxo = await _context.Fluxo
                .Include(f => f.Conta)
                .Include(f => f.ContaFinanceira)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.FluxoId == id);
            if (fluxo == null)
            {
                return NotFound();
            }

            return View(fluxo);
        }

        // POST: Fluxos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fluxo = await _context.Fluxo.FindAsync(id);
            if (fluxo != null)
            {
                _context.Fluxo.Remove(fluxo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FluxoExists(int id)
        {
            return _context.Fluxo.Any(e => e.FluxoId == id);
        }
    }
}
