using System.Collections.Generic;

namespace ResourceBalancer.Abstraction
{
    public interface ITasksRepository<T> where T : ITask
    {
        ICollection<T> GetTasks();
        T GetTaskById(string id);
        ICollection<T> FindByConsumption(int consumption);

        bool InsertTask(T task);
        bool DeleteTask(string id);
    }
}
