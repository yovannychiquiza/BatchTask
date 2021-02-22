using BatchTasks.Facade;
using BatchTask.Models;
using System;
using BatchTask.Factory;

namespace BatchTask
{
    class Program
    {
        static void Main(string[] args)
        {
            BatchTaskBL batchTaskBL = new BatchTaskBL();
            var lista = batchTaskBL.GetActiveBatchTasksNames();

            ITasksFactory factory = new TasksFactory();

            foreach (var item in lista)
            {
                factory.GetTask(item.Name).DoWork(item.Id);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
