using System.Diagnostics;
using System.IO;
using Entities;

namespace Server.Models
{
    public class ReportProcessor
    {
        private readonly Report _report;

        public static Report Process(Report report)
        {
            var processor = new ReportProcessor(report);

            Stopwatch timer = Stopwatch.StartNew();
            processor.Process();
            timer.Stop();

            report.ExecutionTime = timer.Elapsed.ToString();

            return report;
        }

        private ReportProcessor(Report report)
        {
            _report = report;
        }

        /// <summary>
        /// Processing all files in the directory.
        /// </summary>
        private void Process()
        {
            try
            {
                var fileNames = Directory.GetFiles(_report.Directory, "*", SearchOption.AllDirectories);
                foreach (var fileName in fileNames)
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    ++_report.FilesCount;

                    ProcessFile(fileInfo);
                }
            }
            catch
            {
                ++_report.ErrorsCount;
            }
        }

        protected void ProcessFile(FileInfo fileInfo)
        {
            int count = 0;
            var ahoCorasick = new AhoCorasick();

            try
            {
                foreach (var suspicious in _report.Suspicious)
                {
                    // Checks if suspicious needs special extension.
                    if (suspicious.Extensions.Count > 0 && !suspicious.Extensions.Contains(fileInfo.Extension))
                        continue;

                    ahoCorasick.Add(suspicious);
                    ++count;
                }

                if (count > 0)
                {
                    using var reader = new StreamReader(fileInfo.OpenRead());
                    ahoCorasick.Process(reader);
                }
            }
            catch
            {
                ++_report.ErrorsCount;
            }
        }
    }
}