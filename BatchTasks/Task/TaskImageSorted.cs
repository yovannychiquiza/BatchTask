using BatchTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using BatchTask.Facade;
using Microsoft.Data.SqlClient;

namespace BatchTask.Task
{
    public class TaskImageSorted : Task
    {

        protected override string RunTask(int id)
        {

            ELimgContext _context = new ELimgContext();
            var repository = new BatchTaskRepository(_context);
            var batchTask = repository.GetById(id);

            ParameterModel parameterModel = Util.Deserialize<ParameterModel>(@batchTask.Parameters);

            StartSortImages(parameterModel);
            return "";
        }

        /// <summary>
        /// Search all pallets ready to sort
        /// </summary>
        /// <param name="parameterModel"></param>
        private void StartSortImages(ParameterModel parameterModel)
        {

            SqlDataReader myReader1 = DBConnection.getDataFromNAVPallets();

            while (myReader1.Read())
            {
                string pallet = myReader1["pallet"].ToString();
                SortImage(pallet, parameterModel);

            }

        }
        /// <summary>
        /// Sort images by pallet from unsorted folder to sorted folder
        /// </summary>
        /// <param name="pallet"></param>
        /// <param name="parameterModel"></param>
        /// <returns></returns>
        public string SortImage(string pallet, ParameterModel parameterModel)
        {

            string server = @"\\" + parameterModel.ServerDestination;


            string result = "success";
            SqlDataReader myReader1 = getDataFromNAV(pallet);
            SqlDataReader myReader2 = getDataFromNAV(pallet);
            SqlDataReader myReader3 = getDataFromNAV(pallet);

            bool resort_flag = false;

            if (!myReader1.HasRows && !myReader2.HasRows)
            {
                result = "Entered Wrong pallet";
            }
            else
            {

                while (myReader1.Read())
                {
                    try
                    {
                        string unsortPath = server + @"\images\Unsorted\" + myReader1["Serial No_"].ToString() + ".jpg";
                        string sortPath = server + @"\images\Sorted\" + pallet + @"\" + myReader1["Serial No_"].ToString() + ".jpg";
                        string resortImagePath = server + @"\images\Resorted\" + pallet + @"\" + myReader1["Serial No_"].ToString() + ".jpg";
                        DirectoryInfo folder = Directory.CreateDirectory(server + @"\images\Sorted\" + pallet);
                        if (File.Exists(unsortPath))
                        {
                            if (File.Exists(sortPath))
                            {

                                if (File.Exists(resortImagePath))
                                {
                                    System.IO.File.Replace(unsortPath, sortPath, resortImagePath);
                                }
                                else
                                {
                                    Directory.CreateDirectory(server + @"\images\Resorted\" + pallet);
                                    System.IO.File.Move(sortPath, resortImagePath);
                                    System.IO.File.Move(unsortPath, sortPath);
                                }
                            }
                            else
                            {
                                System.IO.File.Move(unsortPath, sortPath);
                                resort_flag = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Util.CreateLog(e.Message);
                    }
                }



                while (myReader2.Read())
                {
                    try
                    {
                        string unsortPath = server + @"\images\Unsorted\" + myReader2["Serial No_"].ToString() + ".jpg";
                        string sortPath = server + @"\images\Sorted\" + pallet + @"\" + myReader2["Serial No_"].ToString() + ".jpg";
                        string Path = server + @"\images\Sorted\" + pallet;
                        string resortFolderPath = server + @"\images\Resorted\" + pallet;
                        string resortImagePath = server + @"\images\Resorted\" + pallet + @"\" + myReader2["Serial No_"].ToString() + ".jpg";
                        if (resort_flag)
                        {
                            if (!File.Exists(sortPath))
                            {
                                if (File.Exists(resortImagePath))
                                {
                                    System.IO.File.Move(resortImagePath, sortPath);
                                }
                            }
                        }
                        else
                        {
                            if (Directory.Exists(Path) && !resort_flag)
                            {
                                if (Directory.Exists(resortFolderPath))
                                {
                                    string[] fileEntries = Directory.GetFiles(Path);
                                    foreach (string fileName in fileEntries)
                                    {
                                        if (!File.Exists(resortFolderPath + @"\" + System.IO.Path.GetFileName(fileName)))
                                        {
                                            System.IO.File.Move(fileName, resortFolderPath + @"\" + System.IO.Path.GetFileName(fileName));
                                        }

                                    }
                                    if (!File.Exists(sortPath) && File.Exists(resortImagePath))
                                    {
                                        System.IO.File.Move(resortFolderPath + @"\" + myReader2["Serial No_"].ToString() + ".jpg", sortPath);
                                    }
                                    resort_flag = true;
                                }
                                else
                                {
                                    if (!Directory.Exists(server + @"\images\Sorted\" + pallet))
                                    {
                                        DirectoryInfo folder = Directory.CreateDirectory(server + @"\images\Sorted\" + pallet);
                                    }
                                    if (File.Exists(resortFolderPath + @"\" + myReader2["Serial No_"].ToString()))
                                    {
                                        System.IO.File.Move(resortFolderPath + @"\" + myReader2["Serial No_"].ToString() + ".jpg", sortPath);
                                    }
                                    resort_flag = true;
                                }
                            }
                            else
                            {
                                DirectoryInfo folder = Directory.CreateDirectory(server + @"\images\Sorted\" + pallet);
                                if (File.Exists(unsortPath))
                                {
                                    System.IO.File.Move(unsortPath, sortPath);
                                }
                                else
                                    resort_flag = true;

                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Util.CreateLog(e.Message);
                    }

                }

                while (myReader3.Read()) 
                {
                    try
                    {
                        string unsortPath = server + @"\images\Unsorted\" + myReader3["Serial No_"].ToString() + ".jpg";
                        string sortPath = server + @"\images\Sorted\" + pallet + @"\" + myReader3["Serial No_"].ToString() + ".jpg";
                        string Path = server + @"\images\Sorted\" + pallet;
                        string resortFolderPath = server + @"\images\Resorted\" + pallet;
                        string resortImagePath = server + @"\images\Resorted\" + pallet + @"\" + myReader3["Serial No_"].ToString() + ".jpg";

                        if (Directory.Exists(resortFolderPath))
                        {
                            if (File.Exists(resortImagePath))
                            {
                                if (!File.Exists(sortPath))
                                {
                                    System.IO.File.Move(resortImagePath, sortPath);
                                }
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        Util.CreateLog(e.Message);
                    }
                }
                //Added code to move images from resorted to unsorted folder

                string ResortPallet = server + @"\images\Resorted\" + pallet;
                string unSort = server + @"\images\Unsorted\";

                if (Directory.Exists(ResortPallet))
                {
                    try 
                    { 
                        string[] fileEntries = Directory.GetFiles(ResortPallet);

                        foreach (string fileName in fileEntries)
                        {
                            if (File.Exists(fileName) && Path.GetExtension(fileName) == ".jpg")
                            {
                                if (!File.Exists(unSort + @"\" + System.IO.Path.GetFileName(fileName)))
                                {
                                    System.IO.File.Move(fileName, unSort + @"\" + System.IO.Path.GetFileName(fileName));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Util.CreateLog(e.Message);
                    }
                }


                CreateImagesSorted(pallet, parameterModel);
                result = "Sorting has been done for pallet " + pallet;


            }

            return result;
        }

        protected SqlDataReader getDataFromNAV(string pallet)
        {
            SqlConnection sqlConnection = DBConnection.getNAVConnection();
            string selectQueryString = "SELECT [Pallet No_], [Serial No_] FROM [silfab_ca].[dbo].[Silfab Ontario$Serial No_ Information] where[Pallet No_] = '" + pallet + "'";
            SqlCommand sqlCommand = new SqlCommand(selectQueryString, sqlConnection);
            SqlDataReader myReader = sqlCommand.ExecuteReader();
            return myReader;
        }

        protected void CreateImagesSorted(string pallet, ParameterModel parameterModel)
        {
            try { 
                SqlConnection sqlConnection = DBConnection.getELimgConnection();
                string selectQueryString = "insert into silfabdata.dbo.ImagesSorted (PalletNo) values ('" + pallet + "')";
                SqlCommand sqlCommand = new SqlCommand(selectQueryString, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                SaveSerial(pallet, parameterModel);
            }
            catch (Exception e)
            {
                Util.CreateLog(e.Message);
            }
        }


        public void SaveSerial(string pallet, ParameterModel parameterModel)
        {
            string pathServer = @"\\" + parameterModel.ServerDestination + @"\images\Sorted\";

            DirectoryInfo d = new DirectoryInfo(pathServer + pallet);//Assuming pallet is your Folder in sorted folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting all jpeg files

            foreach (FileInfo jpgfile in Files)
            {
                var serial = jpgfile.Name.Split('.')[0];
                CreateSerialInformation(pallet, serial);
            }

        }

        protected string CreateSerialInformation(string pallet, string serial)
        {
            ELimgContext _context = new ELimgContext();
            Log log = new Log();
            log.PalletNo = pallet;
            log.Sno = serial;
            _context.Add(log);
            _context.SaveChanges();
            return "";
        }

    }
}
