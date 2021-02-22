using BatchTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Configuration;

namespace BatchTasks.Facade
{
    public class BatchTaskBL
    {
        public List<BatchTask.Models.BatchTasks> GetActiveBatchTasksNames()
        {
           
            var repository = new BatchTaskRepository(new ELimgContext());

            var list = repository.GetAll().ToList();
            int timeMinutesReactiveExecution = 40;
            
            List<BatchTask.Models.BatchTasks> listTask = new List<BatchTask.Models.BatchTasks>();
            foreach (var item in list)
            {

                if (item.LastEjecution.AddMinutes(timeMinutesReactiveExecution) < DateTime.Now && item.IsRunning)
                {
                    var task = repository.GetById(item.Id);
                    task.IsRunning = false;
                    task.LastEjecution = DateTime.Now;
                    repository.Update(task);
                }

                var newDate = item.LastEjecution.AddMinutes(item.IntervalMinutes);
                if (newDate < DateTime.Now && !item.IsRunning)
                {
                    listTask.Add(item);
                }
            }

            return listTask;

        }
        
    }
}
