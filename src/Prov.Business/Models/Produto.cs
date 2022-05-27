﻿using System.ComponentModel.DataAnnotations;

namespace Prov.Business.Models
{
    public class Produto : Entidade
    {
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal  Valor { get; set; }

        public bool Ativo { get; set; }

        /*EF Relations*/

        public Fornecedor Fornecedor { get; set; }
    }
}
