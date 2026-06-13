using EcoWasteApi.Models;
using EcoWasteApi.ViewModels.Requests;
using FluentValidation;

namespace EcoWasteApi.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
  public RegisterRequestValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(120);

    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress()
      .MaximumLength(180);

    RuleFor(x => x.Password)
      .NotEmpty()
      .MinimumLength(6)
      .MaximumLength(100);

    RuleFor(x => x.Role)
      .Must(x => string.IsNullOrWhiteSpace(x) || UserRoles.IsValid(x))
      .WithMessage("Role must be Admin or Operator.");
  }
}

