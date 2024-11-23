﻿using EstoqueProdutos.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EstoqueProdutos.Models
{
    public class VendaSemProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public double? Valor { get; set; }

        public ClienteModel Cliente { get; set; }

        public FormaDePagamentoEnum FormaDePagamento { get; set; }
        public bool Parcelado { get; set; }
        public int QuantidadeDeParcela { get; set; }

        public DateTime DtInc { get; set; } = DateTime.Now;
        public DateTime DtAlt { get; set; } = DateTime.Now;


        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }

    }
}