﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class ProdutoDTOUpdateRequest :IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "O Id deve ser entre 1 e 9999.")]
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DataCadastro.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult("A data de cadastro não pode ser anterior a hoje.", new[] { nameof(DataCadastro) });
            }
        }
    }
}
