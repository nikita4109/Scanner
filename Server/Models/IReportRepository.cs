using Entities;

namespace Server.Models
{
    public interface IReportRepository
    {
        Optional<Report> GetReport(int id);
        int Add(Report report);
        bool IsExist(int id);
    }
}