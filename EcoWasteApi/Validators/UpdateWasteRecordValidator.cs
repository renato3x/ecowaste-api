using EcoWasteApi.Models;
using EcoWasteApi.ViewModels.Requests;
using FluentValidation;

namespace EcoWasteApi.Validators;

public class UpdateWasteRecordValidator : AbstractValidator<UpdateWasteRecordRequest>
{
  public UpdateWasteRecordValidator()
  {
    RuleFor(x => x.WasteType)
      .NotEmpty()
      .MaximumLength(80);

    RuleFor(x => x.Location)
      .NotEmpty()
      .MaximumLength(180);

    RuleFor(x => x.WeightKg)
      .GreaterThan(0);

    RuleFor(x => x.CollectionDate)
      .Must(x => x <= DateTime.UtcNow)
      .WithMessage("CollectionDate cannot be in the future.");

    RuleFor(x => x.Status)
      .NotEmpty()
      .Must(WasteRecordStatus.IsValid)
      .WithMessage($"Status must be one of: {string.Join(", ", WasteRecordStatus.All)}");

    RuleFor(x => x.RecyclingCenterId)
      .GreaterThan(0);
  }
}

