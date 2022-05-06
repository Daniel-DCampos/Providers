﻿using Fornecedores.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fornecedores.Models
{
    public class Fornecedor : Entidade
    {

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public Endereco Endereco { get; set; }

        [DisplayName("Ativo?")]
        public bool Atvo { get; set; }

        /*EF Relations*/

        public IEnumerable<Produto> Produtos { get; set; }
    }
}