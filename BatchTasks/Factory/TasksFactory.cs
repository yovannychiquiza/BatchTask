using BatchTask.Task;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace BatchTask.Factory
{
    public class TasksFactory : ITasksFactory
    {
        public ITask GetTask(string taskName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var currentType = currentAssembly.GetTypes().SingleOrDefault(t => t.Name == taskName);
            return (ITask)Activator.CreateInstance(currentType);
        }

        public List<Type> GetTasks()
        {
            var typeList = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == typeof(ITask).Namespace select t;
            return typeList.ToList();
        }
    }

}
