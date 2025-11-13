namespace FluxoCaixa.Models
{
    public class GraficoContaDTO
    {
        public int GraficoContaDTOId { get; set; }

        public string Nome { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal TotalReceber { get; set; }
    }
}
