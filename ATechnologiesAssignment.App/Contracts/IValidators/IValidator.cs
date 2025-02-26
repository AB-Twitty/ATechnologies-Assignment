using ATechnologiesAssignment.App.Contracts.IValidators.Models;

namespace ATechnologiesAssignment.App.Contracts.IValidators
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T entity);
    }
}
