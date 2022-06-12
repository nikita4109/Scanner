using System.Diagnostics;
using System.IO;
using Entities;

namespace Server.Models
{
    public class ReportProcessor : IReportProcessor
    {
        protected readonly Report Report;

        public Report Process()
        {
            Stopwatch timer = Stopwatch.StartNew();
            ProcessDirectory(Report.Directory);
            timer.Stop();

            Report.ExecutionTime = timer.Elapsed.ToString();

            return Report;
        }

        public ReportProcessor(Report report)
        {
            Report = report;
        }

        /// <summary>
        /// Processing all files in the directory.
        /// </summary>
        private void ProcessDirectory(string directory)
        {
            try
            {
                var fileNames = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                foreach (var fileName in fileNames)
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    ++Report.FilesCount;

                    ProcessFile(fileInfo);
                }
            }
            catch
            {
                ++Report.ErrorsCount;
            }
        }

        protected virtual void ProcessFile(FileInfo fileInfo)
        {
            int count = 0;
            var ahoCorasick = new AhoCorasick();

            try
            {
                foreach (var suspicious in Report.Suspicious)
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
                ++Report.ErrorsCount;
            }
        }
    }
}