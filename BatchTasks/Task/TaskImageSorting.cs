using BatchTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using BatchTask.Facade;

namespace BatchTask.Task
{
    public class TaskImageSorting : Task
    {
        protected override string RunTask(int id)
        {

            ELimgContext _context = new ELimgContext();
            var repository = new BatchTaskRepository(_context);
            var batchTask = repository.GetById(id);

            ParameterModel parameterModel = Util.Deserialize<ParameterModel>(@batchTask.Parameters);

            StartMovingImages(parameterModel);
            return "";
        }

        /// <summary>
        /// Move images to Unsorted folder
        /// </summary>
        /// <param name="parameterModel"></param>
        private void StartMovingImages(ParameterModel parameterModel)
        {

            string serverOrigin = @"\\" + parameterModel.ServerOrigin + @"\" + parameterModel.FolderOrigin;
            string serverDestination = @"\\" + parameterModel.ServerDestination;
            string[] fileEntries = Directory.GetFiles(@serverOrigin);
            string destination = "";
            foreach (string fileName in fileEntries)
            {
                try
                {
                    if (!File.Exists(serverDestination + @"\Images\Unsorted" + @"\" + System.IO.Path.GetFileName(fileName)))
                    {
                        destination = serverDestination + @"\Images\Unsorted" + @"\" + System.IO.Path.GetFileName(fileName);
                        System.IO.File.Move(fileName, destination);
                    }
                    else
                    {
                        if (File.Exists(serverDestination + @"\Images\Unsortbackup" + @"\" + System.IO.Path.GetFileName(fileName)))
                        {
                            System.IO.File.Delete(serverDestination + @"\Images\Unsortbackup" + @"\" + System.IO.Path.GetFileName(fileName));
                            System.IO.File.Move(serverDestination + @"\Images\unsorted" + @"\" + System.IO.Path.GetFileName(fileName), serverDestination + @"\Images\Unsortbackup" + @"\" + System.IO.Path.GetFileName(fileName));
                            System.IO.File.Move(fileName, serverDestination + @"\Images\Unsorted" + @"\" + System.IO.Path.GetFileName(fileName));
                        }
                        else
                        {
                            System.IO.File.Move(serverDestination + @"\Images\Unsorted" + @"\" + System.IO.Path.GetFileName(fileName), serverDestination + @"\Images\Unsortbackup" + @"\" + System.IO.Path.GetFileName(fileName));
                            System.IO.File.Move(fileName, serverDestination + @"\Images\Unsorted" + @"\" + System.IO.Path.GetFileName(fileName));
                        }
                    }
                }
                catch (Exception e)
                {
                    Util.CreateLog(e.Message + " Origin: " + fileName + " Destination: " + destination);
                }
            }
    
        }
        

    }
}
