using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchTask.Facade;
using BatchTask.Factory;
using BatchTask.Models;
using BatchTasks.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BatchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecuteController : ControllerBase
    {
        private IConfiguration configuration;
        
        private readonly ILogger<ExecuteController> _logger;

        public ExecuteController(ILogger<ExecuteController> logger, IConfiguration iConfig)
        {
            _logger = logger;
            configuration = iConfig;
        }

        [HttpGet]
        public string Get()
        {
            try
            {
            
                Settings.Connection_silfab_ca = configuration.GetValue<string>("Settings:Connection_silfab_ca");
                Settings.Connection_ELimg = configuration.GetValue<string>("Settings:Connection_ELimg");
                Settings.Path_Log = configuration.GetValue<string>("Settings:Path_Log");

                BatchTaskBL batchTaskBL = new BatchTaskBL();
                var lista = batchTaskBL.GetActiveBatchTasksNames();

                ITasksFactory factory = new TasksFactory();

                foreach (var item in lista)
                {
                    factory.GetTask(item.Name).DoWork(item.Id);
                }
            
            }
            catch (Exception e)
            {
                Util.CreateLog(e.Message);
            }
            return "Executed process";

        }

    }
}
