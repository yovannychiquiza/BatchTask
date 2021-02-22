using System;
using System.Collections.Generic;
using System.Text;

namespace BatchTask.Task
{
    public interface ITask
    {
        string DoWork(int taskId = 0);
    }
}
