namespace RDI.Domain.Kernel
{
    public class CommandInput<TCommandResult> : MediatorInput<TCommandResult> where TCommandResult : CommandResult
    {
    }
}