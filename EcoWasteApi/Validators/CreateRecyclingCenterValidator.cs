using EcoWasteApi.ViewModels.Requests;
using FluentValidation;

namespace EcoWasteApi.Validators;

public class CreateRecyclingCenterValidator : AbstractValidator<CreateRecyclingCenterRequest>
{
  public CreateRecyclingCenterValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(140);

    RuleFor(x => x.Address)
      .NotEmpty()
      .MaximumLength(180);

    RuleFor(x => x.City)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.State)
      .NotEmpty()
      .MaximumLength(60);

    RuleFor(x => x.ContactPhone)
      .NotEmpty()
      .MaximumLength(30);
  }
}

