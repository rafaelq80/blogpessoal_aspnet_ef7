using blogpessoal.Models;
using FluentValidation;

namespace blogpessoal.Validators
{
    public class TemaValidator : AbstractValidator<Tema>
    {
        public TemaValidator()
        {
            RuleFor(t => t.Descricao)
                .NotEmpty()
                .MaximumLength(140);
        }

    }
}
