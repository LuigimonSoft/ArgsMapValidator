
namespace ArgsMapValidator.Validation.Interfaces
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T entity);
    }
} 

