using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Request;
using FluentValidation;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Validators;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(t => t.Name)
            .NotNull()
            .WithMessage("Valor no puede ser nulo")
            .NotEmpty()
            .WithMessage("Nombre no puede estar vacio")
            .MinimumLength(2)
            .WithMessage("Longitud de nombre debe ser mayir a 2")
            .MaximumLength(50)
            .WithMessage("Longitud de nombre debe ser menor a 50");
    }
}
