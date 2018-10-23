using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeTracker.Logic.Models;

namespace TimeTracker.Logic
{
    /// <summary>
    /// Service to work with the log
    /// </summary>
    public class LogService
    {
        private string fileName = "timeLog.json";
        private string pathOldFileName;
        private string pathFileName;

        private WorkTimeLog logWorkTime; 

        public LogService()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Assembly.GetExecutingAssembly().GetName().Name);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            pathFileName = Path.Combine(directory, fileName);
            logWorkTime = LoadLog();
        }

        /// <summary>
        /// Load working time log
        /// </summary>
        /// <returns>Work Time Log</returns>
        public WorkTimeLog LoadLog()
        {
            logWorkTime = File.Exists(pathFileName)
                ? JsonConvert.DeserializeObject<WorkTimeLog>(File.ReadAllText(pathFileName))
                : new WorkTimeLog();
            return logWorkTime;
        }

        /// <summary>
        /// Save working time log
        /// </summary>
        public void SaveLog(WorkTimeLog logWorkTimes)
        {
            File.WriteAllText(pathFileName, JsonConvert.SerializeObject(logWorkTimes));
        }

        /// <summary>
        /// Сurrent day note
        /// </summary>
        /// <returns>Current day's time note</returns>
        public WorkTimeNote CurrentDay 
        {
            get
            {
                var date = DateTime.Now.Date;
                return logWorkTime.LogNotes.ContainsKey(date)
                    ? logWorkTime.LogNotes[date]
                    : new WorkTimeNote {BeginDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddSeconds(1)};
            }
        }

        /// <summary>
        /// Curent log
        /// </summary>
        /// <returns>Time log</returns>
        public WorkTimeLog Log => logWorkTime;
    }
}
