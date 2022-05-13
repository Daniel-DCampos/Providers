using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prov.App.DTO
{
    public class ProdutoDTO
    {
        [Key]
        public Guid id { get; set; }

        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        [DisplayName("Imagem")]
        public IFormFile imagemUpload { get; set; }

        public string? imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        public bool Ativo { get; set; }

        /*EF Relations*/

        public FornecedorDTO? Fornecedor { get; set; }

        public IEnumerable<FornecedorDTO>? Fornecedores { get; set; }
    }
}
