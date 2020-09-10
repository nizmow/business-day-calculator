using FluentValidation;

namespace BusinessDaysBetween.Business.Infrastructure
{
    public class HolidayDtoValidator : AbstractValidator<HolidayDto>
    {
        public HolidayDtoValidator()
        {
            RuleFor(h => h.Type).NotNull();
            
            When(h => h.Type == HolidayType.ParticularDayOfMonth, () =>
            {
                RuleFor(h => h.ApplicableDay).NotNull();
                RuleFor(h => h.ApplicableMonth).NotNull();
                RuleFor(h => h.OccurenceInMonth).NotNull().LessThanOrEqualTo(5);
            });
          
            When(h => h.Type == HolidayType.Fixed || h.Type == HolidayType.RollsToMonday, () =>
            {
                RuleFor(h => h.Date).NotNull();
            });
        }
    }
}