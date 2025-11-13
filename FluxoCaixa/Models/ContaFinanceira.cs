using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.Models
{
    public class ContaFinanceira
    {
        public int ContaFinanceiraId { get; set; }
        [Required(ErrorMessage = "O Campo Nome é obrigatório.")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "O Saldo é obrigatório.")]
        public decimal Saldo { get; set; }
    }
}
