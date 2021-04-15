namespace RDI.Domain.Kernel
{
    public interface IEnvironment
    {
        string Name { get; }

        string this[string key] { get; }

        bool IsDevelopment();

        bool IsTesting();

        bool IsDevelopmentOrTesting();

        bool IsProduction();
    }
}