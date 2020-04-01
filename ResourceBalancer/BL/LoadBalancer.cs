using ResourceBalancer.Abstraction;
using ResourceBalancer.Models;
using System;

namespace ResourceBalancer.BL
{
    public class LoadBalancer
    {
        public static string GetOptimalConfiguration(int capacity, ITasksRepository<ITask> groupA, ITasksRepository<ITask> groupB)
        {
            if (capacity < 1 || groupA == null || groupA.GetTasks().Count < 1 || groupB == null || groupB.GetTasks().Count < 1)
                throw new Exception("LoadBalancer.GetOptimalConfiguration: Invalid input values");

            var result = new OptimalCombinations(0);

            foreach (var taskA in groupA.GetTasks())
            {
                if (taskA.Consumption > capacity)
                    continue;

                var groupBTasks = groupB.FindByConsumption(capacity - taskA.Consumption);

                if (groupBTasks == null || groupBTasks.Count < 1)
                    continue;

                foreach (var taskB in groupBTasks)
                {
                    int tmpConsumption = taskA.Consumption + taskB.Consumption;
                    if (tmpConsumption < result.OptimalConsumption)
                        break;

                    if (tmpConsumption > result.OptimalConsumption)
                        result = new OptimalCombinations(tmpConsumption);

                    result.AddCombination(new Combination { taskA = taskA.Id, taskB = taskB.Id });
                }
            }

            return result.ToString();
        }
    }
}
