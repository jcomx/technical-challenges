using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceBalancer.Abstraction;
using ResourceBalancer.Models;
using ResourceBalancer.Repositories;

namespace ResourceBalancer.Tests
{
    [TestClass]
    public class TaskRespositoryTests
    {
        ITasksRepository<ITask> repo;
        SimpleTask task_1 = new SimpleTask("id1", 3);

        [TestMethod]
        public void Add_SimpleTask()
        {
            repo = new TaskRepository<ITask>();
            Assert.IsTrue(repo.InsertTask(task_1));
        }

        [TestMethod]
        public void Add_InvalidTask()
        {
            repo = new TaskRepository<ITask>();
            repo.InsertTask(task_1);

            SimpleTask invalidTask = new SimpleTask(task_1.Id, 9);
            Assert.IsFalse(repo.InsertTask(invalidTask));
        }

        [TestMethod]
        public void Get_TaskById()
        {
            repo = new TaskRepository<ITask>();
            repo.InsertTask(task_1);

            Assert.IsNotNull(repo.GetTaskById(task_1.Id));
        }

        
    }
}
