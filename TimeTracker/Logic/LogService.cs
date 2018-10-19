using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private string oldFileName = "workTimeLog.json";
        private string fileName = "timeLog.json";

        private WorkTimeLog logWorkTime; 

        public LogService()
        {
            if (File.Exists(oldFileName))
                TransformLogFile();
            logWorkTime = LoadLog();
        }

        /// <summary>
        /// Load working time log previous version
        /// </summary>
        /// <returns>Work Time Log (previous version)</returns>
        [Obsolete]
        private Dictionary<string, WorkTimeNote> LoadOldLog()
        {
            return File.Exists(oldFileName)
                ? JsonConvert.DeserializeObject<Dictionary<string, WorkTimeNote>>(File.ReadAllText(oldFileName))
                : new Dictionary<string, WorkTimeNote>();
        }

        /// <summary>
        /// Transformation of the journal of the previous version into a new one
        /// </summary>
        private void TransformLogFile()
        {
            var oldLogNotes = LoadOldLog();
            if (!oldLogNotes.Any()) return;
            var newLog = new WorkTimeLog();
            foreach (var workTimeNote in oldLogNotes)
            {
                var day = DateTime.Parse(workTimeNote.Key);
                newLog.LogNotes.Add(day, workTimeNote.Value);
            }
            SaveLog(newLog);
            File.Delete(oldFileName);
        }

        /// <summary>
        /// Load working time log
        /// </summary>
        /// <returns>Work Time Log</returns>
        public WorkTimeLog LoadLog()
        {
            logWorkTime = File.Exists(fileName)
                ? JsonConvert.DeserializeObject<WorkTimeLog>(File.ReadAllText(fileName))
                : new WorkTimeLog();
            return logWorkTime;
        }

        /// <summary>
        /// Save working time log
        /// </summary>
        public void SaveLog(WorkTimeLog logWorkTimes)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(logWorkTimes));
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
                    : new WorkTimeNote {BeginDateTime = DateTime.Now};
            }
        }

        /// <summary>
        /// Curent log
        /// </summary>
        /// <returns>Time log</returns>
        public WorkTimeLog Log => logWorkTime;
    }
}
