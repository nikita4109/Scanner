using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Entities;

namespace Server.Models
{
    public class ReportRepository : IReportRepository
    {
        private readonly List<Task<Report>> _tasks;
        
        public ReportRepository()
        {
            _tasks = new List<Task<Report>>();

        }

        public Optional<Report> GetReport(int id)
        {
            if (_tasks[id].IsCompleted)
                return Optional<Report>.Of(_tasks[id].Result);
            return Optional<Report>.Empty();
        }

        public int Add(Report report)
        {
            var task = Task.Run(() =>
            {
                var processor = new ReportProcessor(report);
                return processor.Process();
            });

            _tasks.Add(task);
            return _tasks.Count - 1;
        }

        public bool IsExist(int id)
        {
            return id < _tasks.Count;
        }
    }
}