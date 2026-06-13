using EcoWasteApi.ViewModels.Requests;
using FluentValidation;

namespace EcoWasteApi.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress();

    RuleFor(x => x.Password)
      .NotEmpty();
  }
}

