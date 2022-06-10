using System.Collections.Generic;
using System.IO;
using System.Text;
using Entities;
using NUnit.Framework;
using Server.Models;

namespace Tests
{
    public class AhoCorasickTest
    {
        private static StreamReader BuildReader(string s)
        {
            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(s)));
        }

        [Test]
        public void JsTest()
        {
            Suspicious js = new Suspicious(
                "JS",
                @"<script>evil_script()</script>",
                0,
                new List<string>() {".js"});

            var aho = new AhoCorasick();
            var reader = BuildReader("<script>evil_script()</script>");

            aho.Add(js);
            aho.Process(reader);

            Assert.True(js.FilesCount == 1);
        }

        [Test]
        public void RmTest()
        {
            Suspicious rm = new Suspicious(
                "rm -rf",
                @"rm -rf %userprofile%\Documents",
                0,
                new List<string>());

            var aho = new AhoCorasick();
            var reader = BuildReader(@"rm -rf %userprofile%\Documents");

            aho.Add(rm);
            aho.Process(reader);

            Assert.True(rm.FilesCount == 1);
        }

        [Test]
        public void DllTest()
        {
            Suspicious dll = new Suspicious(
                "Rundll32",
                @"Rundll32 sus.dll SusEntry",
                0,
                new List<string>());

            var aho = new AhoCorasick();
            var reader = BuildReader(@"Rundll32 sus.dll SusEntry");

            aho.Add(dll);
            aho.Process(reader);

            Assert.True(dll.FilesCount == 1);
        }

        [Test]
        public void UselessAddTest()
        {
            Suspicious js = new Suspicious(
                "JS",
                @"<script>evil_script()</script>",
                0,
                new List<string>() {".js"});

            var aho = new AhoCorasick();
            var reader = BuildReader("<script>evil_script()</script>");

            for (int i = 0; i < 100; ++i)
                aho.Add(js);

            aho.Process(reader);

            Assert.True(js.FilesCount == 1);
        }

        [Test]
        public void AllTypesTest()
        {
            Suspicious js = new Suspicious(
                "JS",
                @"<script>evil_script()</script>",
                0,
                new List<string>() {".js"});

            Suspicious rm = new Suspicious(
                "rm -rf",
                @"rm -rf %userprofile%\Documents",
                0,
                new List<string>());

            Suspicious dll = new Suspicious(
                "Rundll32",
                @"Rundll32 sus.dll SusEntry",
                0,
                new List<string>());

            var aho = new AhoCorasick();
            var reader = BuildReader(
                "Дедлайн - 12 июня 2022г. Решить задачу нужно на языке программирования C#. Свое решение прикрепляйте в поле ответа ниже в виде ссылки либо архива с кодом. Не забудьте подписать файл в формате Разработка C#_Тестовое задание_Фамилия Имя. Упорядочивание файлов в вашем решении и комментарии к коду помогут быстрее понять ваш ход решения и завершить проверку задания." +
                "Требуется реализовать два приложения, используя .NET Core / .NET:" +
                "сервисное приложение, реализующее простой REST API, который предоставляет ресурс для создания задач на сканирование файлов в директории и получения статуса задачи по идентификатору" +
                "утилиту, работающую из командной строки, отправляющую сервисному приложению команды на создание и просмотр состояния задач.любой файл, содержащий строку: Rundll32 sus.dll SusEntryПосле завершения команды создание задачи на сканирование должен быть выведен уникальный идентификатор задачи." +
                "После завершения команды просмотра статуса задачи может быть выведено два результата: статус задача" +
                "еще выполняется или отчет о сканировании, в котором присутствует следующая информация:" +
                "путь к директории, сканирование которой производилось" +
                "общее количество обработанных файлов" +
                "количество обнаружений на каждый тип подозрительного содержимого" +
                "количество ошибок анализа файлов (например, не хватает прав на чтение файла)" +
                "время выполнения утилиты." +
                "Пример запуска сервиса и исполнения утилиты из командной строки:");

            aho.Add(js);
            aho.Add(rm);
            aho.Add(dll);
            aho.Process(reader);

            Assert.True(js.FilesCount == 0);
            Assert.True(rm.FilesCount == 0);
            Assert.True(dll.FilesCount == 1);
        }
    }
}