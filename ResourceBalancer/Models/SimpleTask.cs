using ResourceBalancer.Abstraction;

namespace ResourceBalancer.Models
{
    public class SimpleTask : ITask
    {
        public SimpleTask(string id, int consumption)
        {
            Id = id;
            Consumption = consumption;
        }

        public string Id { get; }

        public int Consumption { get; }

        public override string ToString() => $"({Id}, {Consumption})";
    }
}
