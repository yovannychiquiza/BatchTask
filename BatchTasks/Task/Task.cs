using BatchTask.Facade;
using BatchTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BatchTask.Task
{
    public abstract class Task : ITask
    {
        public string FINISH_SUCCESSFULLY = "FINISH SUCCESSFULLY";
        int id = 0;
        public virtual string DoWork(int taskId = 0)
        {
            try
            {
                id = taskId;
                var t = new Thread(() => Execute(taskId));
                t.Start();
                return FINISH_SUCCESSFULLY;
            }
            catch (Exception e)
            {
                Util.CreateLog(e.Message);
                return "ERROR";
            }
        }


        public virtual void Execute(int taskId = 0)
        {
            var repository = new BatchTaskRepository(new ELimgContext());
            if (taskId == 0)
                return;
           
            try
            {
                var inicio = DateTime.Now;

                var task = repository.GetById (taskId);
                task.IsRunning = true;
                repository.Update(task);
           
                string res = string.Empty;
                res = RunTask(taskId);

               
                task = repository.GetById(taskId);
                task.IsRunning = false;
                task.LastEjecution = DateTime.Now;
                repository.Update(task);

            }
            catch (Exception e)
            {
                Util.CreateLog(e.Message);
                try { 
                    var task = repository.GetById(taskId);
                    task.IsRunning = false;
                    task.LastEjecution = DateTime.Now;
                    repository.Update(task);
                }
                catch (Exception){}
            }

        }

        protected abstract string RunTask(int var = 0);
    }
}
