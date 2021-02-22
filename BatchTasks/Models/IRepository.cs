using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchTask.Models
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(object id);
    }
}
