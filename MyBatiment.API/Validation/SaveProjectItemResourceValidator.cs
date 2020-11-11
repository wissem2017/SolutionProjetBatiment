using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using MyBatiment.API.Resources;

namespace MyBatiment.API.Validation
{
    public class SaveProjectItemResourceValidator : AbstractValidator<SaveProjectItemResource>
    {
        public SaveProjectItemResourceValidator()
        {
            RuleFor(m => m.ProjectName)
               .NotEmpty()
               .MaximumLength(50);

            RuleFor(m => m.Description)
                 .MaximumLength(255)
                  .WithMessage("Il faut avoir 255 caractères au maximum");

        }
    }
}
