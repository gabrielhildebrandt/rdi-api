using FluentValidation;

namespace RDI.Domain.Kernel
{
    public class MediatorValidator<TMediatorInput> : AbstractValidator<TMediatorInput>, IMediatorValidator
    {
    }
}