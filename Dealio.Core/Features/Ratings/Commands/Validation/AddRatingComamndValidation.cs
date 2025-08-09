using Dealio.Core.Features.Ratings.Commands.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Ratings.Commands.Validation
{
    public class AddRatingComamndValidation : AbstractValidator<AddRatingCommand>
    {
        public AddRatingComamndValidation()
        {
            RuleFor(x => x.RatingValue)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating value must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Comment cannot be empty.")
                .MaximumLength(500)
                .WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}
