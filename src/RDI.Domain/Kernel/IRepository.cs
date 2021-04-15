namespace RDI.Domain.Kernel
{
    public interface IRepository
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}