using System.Collections.Generic;

namespace ResourceBalancer.Models
{
    public class OptimalCombinations
    {
        public OptimalCombinations(int consumption)
        {
            OptimalConsumption = consumption;
        }

        private readonly List<Combination> _combinations = new List<Combination>();
        public int OptimalConsumption { get; }
        public void AddCombination(Combination combination) => _combinations.Add(combination);
        public override string ToString() => string.Join(", ", _combinations);
    }

    public struct Combination
    {
        public string taskA;
        public string taskB;

        public override string ToString() => $"({taskA}, {taskB})";
    }
}
