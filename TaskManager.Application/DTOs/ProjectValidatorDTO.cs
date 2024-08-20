using FluentValidation;

namespace TaskManager.Application
{
    public class ProjectValidatorDTO : AbstractValidator<NewProjectDTO>
    {
        public ProjectValidatorDTO()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Owner).NotEmpty().MaximumLength(320);
            RuleFor(p => p.StartDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(p => p.FinishDate).GreaterThan(p => p.StartDate);
        }
    }
}