using Amazon.S3;
using Amazon.S3.Model;
using System;
using BatchTask.Facade;
using BatchTask.Models;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace BatchTask.Task
{
    public class TaskUploadAWSImages : Task
    {
        protected override string RunTask(int id)
        {
            Upload(id);
            return "";
        }

        /// <summary>
        /// Upload the images to AWS
        /// </summary>
        /// <param name="id"></param>
        protected async void Upload(int id)
        { 

            var connAWS = getAWSConnection(id);
            string pallet;
            string filenameonly;

            ELimgContext _context = new ELimgContext();
            var listSerial = _context.Log.Where(t => !t.UplodedToCloud);

            string pathServer = connAWS.ParameterAWSDto.ServerSource;
            foreach (var item in listSerial)
            {
                pallet = item.PalletNo;
                string year = item.DateModified.Year.ToString();

                filenameonly = item.Sno + ".jpg";
                string fileName = pathServer + pallet + "/" + filenameonly; // getting complete filename to create the path

                if (File.Exists(fileName)) // checking if input value exist in sorted folder or not
                {

                    try   
                    {
                        PutObjectRequest folderRequest = new PutObjectRequest();
                        String delimiter = "/";
                        folderRequest.BucketName = connAWS.ParameterAWSDto.BucketName ;
                        String folderKey = string.Concat(pallet, delimiter);
                        folderRequest.Key = year + delimiter + folderKey;
                        folderRequest.InputStream = new MemoryStream(new byte[0]);
                        connAWS.amazonS3Client.PutObjectAsync(folderRequest);

                        try
                        {
                            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);  //opening file to read
                            string key = year + delimiter + pallet + delimiter + filenameonly;
                            var por = new PutObjectRequest { BucketName = connAWS.ParameterAWSDto.BucketName , InputStream = fs, Key = key };
                            por.ContentType = "image/jpeg";
                            por.Metadata.Add("x-amz-meta-title", filenameonly.Split('.')[0]);
                            por.Metadata.Add("x-amz-meta-pallet", pallet);
                            await connAWS.amazonS3Client.PutObjectAsync(por);
                            fs.Close();
                            UpdateAwskeys(key, connAWS);
                            UpdateLog(item.Id);
                            
                        }

                        catch (Exception ex)
                        {
                            Util.CreateLog("Upload Error:" + filenameonly + " " + ex + " " + DateTime.Now);
                        }
                    }
                    catch (AmazonS3Exception ex)
                    {
                        Util.CreateLog("Upload Error:" + ex.Message);
                    }
                }
                
                if (item.DateModified < DateTime.Now.AddHours(-24))
                {
                    UpdateLog(item.Id);
                }
            }

        }
        /// <summary>
        /// Search the new key at AWS
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connAWS"></param>
        public void UpdateAwskeys(string key, AWSConection connAWS)
        {
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = connAWS.ParameterAWSDto.BucketName,
                    Prefix = key,
                };
                ListObjectsV2Response response = null;

                response = connAWS.amazonS3Client.ListObjectsV2Async(request).Result;

                var entry = response.S3Objects.FirstOrDefault();
                UpdateAwskeys(entry);

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw new Exception("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }
        }

        /// <summary>
        /// Create a new key path for a serial number at AWS
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected string UpdateAwskeys(S3Object entry)
        {
            try
            {

                var array = entry.Key.Split('/');
                var image = array[array.Length - 1].ToString();
                var serial = image.Split('.')[0];

                ELimgContext _context = new ELimgContext();

                var awsKeys = _context.Awskeys.Where(t => t.S3url == entry.Key);
                if (awsKeys == null || awsKeys.Count() == 0) //if do not find any coincidences create a new one(avoid duplicate)
                {

                    Awskeys awskeys = new Awskeys();
                    awskeys.DateCreated = entry.LastModified;
                    awskeys.S3url = entry.Key;
                    awskeys.YearKey = array[0];
                    awskeys.Pallet = array[1];
                    awskeys.SerialImage = serial;
                    awskeys.Size = entry.Size.ToString();

                    _context.Add(awskeys);
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Util.CreateLog(e.ToString());
            }
            return "";
        }
        /// <summary>
        /// Obtain the connection for AWS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AWSConection getAWSConnection(int id)
        {
            ELimgContext _context = new ELimgContext();
            var repository = new BatchTaskRepository(_context);
            var task = repository.GetById(id);
            ParameterAWSDto parameterModel = Util.Deserialize<ParameterAWSDto>(task.Parameters);

            Amazon.S3.AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = parameterModel.ServiceURL;

            AWSConection aWSConection = new AWSConection();
            aWSConection.amazonS3Client = new AmazonS3Client(parameterModel.AccessKey, parameterModel.SecretKey, config);
            aWSConection.ParameterAWSDto = parameterModel;
            return aWSConection;
        }

        /// <summary>
        /// Update the image uploaded to AWS
        /// </summary>
        /// <param name="id"></param>
        protected void UpdateLog(int id)
        {
            ELimgContext _context = new ELimgContext();
            var serial = _context.Log.First(a => a.Id == id);
            serial.UplodedToCloud = true;
            serial.DateUpload = DateTime.Now;
            _context.SaveChanges();
        }


    }

}
