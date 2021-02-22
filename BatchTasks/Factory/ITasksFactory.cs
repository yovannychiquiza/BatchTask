using BatchTask.Task;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchTask.Factory
{
    public interface ITasksFactory
    {
        ITask GetTask(string taskName);
        List<Type> GetTasks();
    }
}
