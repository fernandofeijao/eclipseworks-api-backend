using FluentValidation;

namespace TaskManager.Application
{
    public class NewTaskValidatorDTO : AbstractValidator<NewTaskDTO>
    {
        public NewTaskValidatorDTO()
        {
            RuleFor(t => t.User).NotEmpty().MaximumLength(320);
            RuleFor(t => t.Title).NotEmpty().MaximumLength(50);
            RuleFor(t => t.Description).NotEmpty();
            RuleFor(t => t.Priority).NotEmpty().InclusiveBetween((byte)1, (byte)3);
            RuleFor(t => t.TargetDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }

    public class EditTaskValidatorDTO : AbstractValidator<EditTaskDTO>
    {
        public EditTaskValidatorDTO()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.User).NotEmpty().MaximumLength(320);
            RuleFor(t => t.Title).NotEmpty().MaximumLength(50);
            RuleFor(t => t.Description).NotEmpty();
            RuleFor(t => t.State).NotEmpty().InclusiveBetween((byte)1, (byte)3);
            RuleFor(t => t.TargetDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }

    public class NewTaskDiscussionValidatorDTO : AbstractValidator<NewTaskDiscussionDTO>
    {
        public NewTaskDiscussionValidatorDTO()
        {
            RuleFor(t => t.User).NotEmpty().MaximumLength(320);
            RuleFor(t => t.Comment).NotEmpty();
            RuleFor(t => t.TaskId).NotEmpty();
        }
    }
}