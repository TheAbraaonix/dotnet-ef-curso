using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        [Required]
        public float Estoque { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
