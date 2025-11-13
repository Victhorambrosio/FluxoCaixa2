using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.Models
{
    public class Conta
    {
        public int ContaId { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O Valor é obrigatório.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A Data de Vencimento é obrigatótia.")]
        [Display(Name = "Data de Vencimento")]
        [DataType(DataType.Date)]
        public DateOnly? DataVencimento { get; set; }

        [Display(Name = "Pago?")]
        public bool? Pago { get; set; }

        [Display(Name = "Tipo")]
        public int TipoContaId { get; set; }
        public TipoConta? TipoConta { get; set; }
    }
}
