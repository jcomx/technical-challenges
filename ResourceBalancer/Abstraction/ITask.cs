namespace ResourceBalancer.Abstraction
{
    public interface ITask
    {
        string Id { get; }
        int Consumption { get; }
    }
}
