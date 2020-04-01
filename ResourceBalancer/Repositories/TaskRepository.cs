using ResourceBalancer.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace ResourceBalancer.Repositories
{
    public class TaskRepository<T> : ITasksRepository<T> where T : class, ITask
    {
        public TaskRepository()
        {
            _tasks = new List<T>();
        }

        private readonly ICollection<T> _tasks;

        public ICollection<T> GetTasks() => _tasks;

        public T GetTaskById(string id) =>
            _tasks.FirstOrDefault(t => t.Id == id);


        public ICollection<T> FindByConsumption(int consumption)
        {
            var bestConsumption = _tasks.OrderByDescending(t => t.Consumption).FirstOrDefault(t => t.Consumption <= consumption)?.Consumption ?? 0;
            return _tasks.Where(t => t.Consumption == bestConsumption).ToList();
        }

        public bool InsertTask(T task)
        {
            if (IsValid(task))
            {
                _tasks.Add(task);
                return true;
            }

            return false;
        }

        public bool DeleteTask(string id) => _tasks.Remove(_tasks.FirstOrDefault(t => t.Id == id));

        private bool IsValid(T task) => !_tasks.Any(t => t.Id == task.Id);
    }
}
