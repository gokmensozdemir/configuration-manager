using ConfigurationManager.API.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationManager.API.Models
{
    public class UpdateConfigurationRequest: IValidatableObject
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!TypeHelper.ValidTypes.ContainsKey(Type))
            {
                results.Add(new ValidationResult($"Invalid Type: {Type}", new string[] { nameof(Type) }));
            }

            if (!TypeHelper.IsValidType(Value, Type))
            {
                results.Add(new ValidationResult($"Value can not be converted. Value: {Value}, Type: {Type}"));
            }

            return results;
        }
    }
}
