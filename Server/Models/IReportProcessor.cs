using Entities;

namespace Server.Models
{
    public interface IReportProcessor
    {
        Report Process();
    }
}