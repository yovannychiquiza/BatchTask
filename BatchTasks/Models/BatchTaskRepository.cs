using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BatchTask.Models
{
 
    public class BatchTaskRepository : SqlRepository<BatchTasks>
    {
        public BatchTaskRepository(ELimgContext context)
            : base(context)
        {
        }

        public override IQueryable<BatchTasks> GetAll()
        {
            return EfDbSet.Where(t => t.IsActive);
        }

        public override BatchTasks GetById(object id)
        {
            return GetAll().FirstOrDefault(c => c.Id == (int)id);
        }

        public async void Update(BatchTasks data)
        {
            var entry = Context.Entry(data);
           // if (entry.State == EntityState.Detached)
            {
                var attachedOrder =  GetById(data.Id);
                if (attachedOrder != null)
                {
                    Context.Entry(attachedOrder).CurrentValues.SetValues(data);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            Context.SaveChanges();
        }

  

    }
}
