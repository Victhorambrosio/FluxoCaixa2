using System.Diagnostics;
using FluxoCaixa.Data;
using FluxoCaixa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var Grafico = new GraficoDTO();
            Grafico.TotalPagar = _context.Conta.Include(f => f.TipoConta).Where(c => c.TipoConta.Tipo == "P").Sum(c => c.Valor);
            Grafico.TotalReceber = _context.Conta.Include(f => f.TipoConta).Where(c => c.TipoConta.Tipo == "R").Sum(c => c.Valor);
            ViewData["Grafico"] = Grafico;

            var listaGraficoConta = new List<GraficoContaDTO>();

            var movimentacoes = _context.Fluxo.Include(cf => cf.ContaFinanceira).Include(c => c.Conta).ToList();

            foreach (var m in movimentacoes)
            {
                var graficoConta = new GraficoContaDTO();
                graficoConta.Nome = m.ContaFinanceira.Nome;
                graficoConta.TotalPagar += _context.Fluxo.Include(cf => cf.ContaFinanceira).Include(c => c.Conta).Include(c => c.Conta.TipoConta).Where(c => c.Conta.TipoConta.Tipo == "P").Where(cf => cf.ContaFinanceira.Nome == m.ContaFinanceira.Nome).Sum(c => c.Conta.Valor);
                graficoConta.TotalReceber += _context.Fluxo.Include(cf => cf.ContaFinanceira).Include(c => c.Conta).Include(c => c.Conta.TipoConta).Where(c => c.Conta.TipoConta.Tipo == "R").Where(cf => cf.ContaFinanceira.Nome == m.ContaFinanceira.Nome).Sum(c => c.Conta.Valor);
                listaGraficoConta.Add(graficoConta);
            }

            ViewData["ListaGraficoConta"] = listaGraficoConta.ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
