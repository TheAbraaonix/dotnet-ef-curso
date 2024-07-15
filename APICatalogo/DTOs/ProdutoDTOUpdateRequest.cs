﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class ProdutoDTOUpdateRequest : IValidatableObject
    {
        [Required]
        [Range(1, 9999, ErrorMessage = "Estoque deve estar entre {1} e {2}")]
        public float Estoque { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataCadastro.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("A data deve ser maior que a data atual.", new[] { nameof(this.DataCadastro) });
            }
        }
    }
}