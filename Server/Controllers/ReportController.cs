using Entities;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        public Entity Get(int id)
        {
            if (!_reportRepository.IsExist(id))
                return new Entity(Status.NotExist, null);

            var report = _reportRepository.GetReport(id);

            if (report.IsPresent)
                return new Entity(Status.Done, report.Get());
            return new Entity(Status.InProcess, null);
        }

        [HttpPost]
        public int Post(Report report)
        {
            return _reportRepository.Add(report);
        }
    }
}