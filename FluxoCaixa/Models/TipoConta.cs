using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.Models
{
    public class TipoConta
    {
        public int TipoContaId { get; set; }
        [Required(ErrorMessage = "Digite o Nome da Conta")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Informe o Tipo da Conta")]
        public string? Tipo { get; set; }
    }
}
