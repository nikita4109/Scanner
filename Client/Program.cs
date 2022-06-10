using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Entities;

namespace Client
{
    class Program
    {
        private static string _url;
        private static List<Suspicious> _suspiciousList;

        static void Main(string[] args)
        {
            Init();

            using HttpClient httpClient = new HttpClient();
            if (args[0] == "scan")
            {
                var id = CreateTask(httpClient, args[1]);
                Console.WriteLine($"Scan task was created with ID: {id}");
            }
            else if (args[0] == "status")
            {
                var entity = GetEntity(httpClient, Int32.Parse(args[1]));
                if (entity.Status == Status.Done)
                    Console.WriteLine(entity.Report);
                else if (entity.Status == Status.InProcess)
                    Console.WriteLine("Scan task in progress, please wait");
                else if (entity.Status == Status.NotExist)
                    Console.WriteLine("Incorrect task ID");
            }
        }

        /// <summary>
        /// Sends to server request to create a task.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="directory">scanning place</param>
        /// <returns>task id</returns>
        private static string CreateTask(HttpClient httpClient, string directory)
        {
            var report = new Report(directory, 0, _suspiciousList, 0, "");
            var json = JsonSerializer.Serialize(report);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(_url, data).Result;
            var id = response.Content.ReadAsStringAsync().Result;

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="id">task id</param>
        /// <returns>status of task and report if its done</returns>
        private static Entity GetEntity(HttpClient httpClient, int id)
        {
            var response = httpClient.GetStringAsync($"{_url}?id={id}").Result;
            return JsonSerializer.Deserialize<Entity>(response);
        }

        /// <summary>
        /// Initializing host's url and list of suspicious strings.
        /// </summary>
        private static void Init()
        {
            _url = "https://localhost:5001/Report";

            _suspiciousList = new List<Suspicious>();
            _suspiciousList.Add(new Suspicious(
                "JS",
                @"<script>evil_script()</script>",
                0,
                new List<string>() {".js"}));

            _suspiciousList.Add(new Suspicious(
                "rm -rf",
                @"rm -rf %userprofile%\Documents",
                0,
                new List<string>()));

            _suspiciousList.Add(new Suspicious(
                "Rundll32",
                @"Rundll32 sus.dll SusEntry",
                0,
                new List<string>()));
        }
    }
}