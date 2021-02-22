using BatchTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using BatchTask.Facade;

namespace BatchTask.Task
{
    public class TaskCheckImages : Task
    {
        protected override string RunTask(int id)
        {

            ELimgContext _context = new ELimgContext();
            var repository = new BatchTaskRepository(_context);
            var batchTask = repository.GetById(id);

            ParameterMailModel parameterModel = Util.Deserialize<ParameterMailModel>(@batchTask.Parameters);

            var listServer = parameterModel.Servers.Split(';');
            foreach (var item in listServer)
            {

                string serverOrigin = @item;
                string[] images = new string[0];
                try
                {
                    images = Directory.GetFiles(serverOrigin);

                }
                catch (Exception e)
                {
                    Util.SendEmail(e.Message, parameterModel);
                    Util.CreateLog(e.Message);
                }

                int NumberImagesLimit = Convert.ToInt32(parameterModel.NumberImagesLimit);

                if (images.Count() >= NumberImagesLimit)
                {
                    string message = images.Count().ToString() + " on Server " + item;
                    Util.SendEmail(message, parameterModel);
                }
            }
            return "";
        }
    }
}
