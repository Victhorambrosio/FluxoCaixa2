using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.Models
{
    public class Fluxo
    {
        public int FluxoId { get; set; }
        [Display(Name = "Data de Movimentação")]
        [DataType(DataType.DateTime)]
        public DateTime? DataMovimentacao { get; set; }

        [Display(Name = "Conta Financeira")]
        public int ContaFinanceiraId { get; set; }
        public ContaFinanceira? ContaFinanceira { get; set; }

        [Display(Name = "Conta")]
        public int ContaId { get; set; }
        public Conta? Conta { get; set; }

        [Display(Name = "Usuário")]
        public string? UsuarioId { get; set; }
        public IdentityUser? Usuario { get; set; }
    }
}
