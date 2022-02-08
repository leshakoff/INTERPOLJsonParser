using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WantedMain;
using WantedDetails;
using System.IO;
using System.Net;

namespace SetiINTERPOL
{
    /// <summary>
    /// Класс, описывающий всю работу по отправке GET-запроса
    /// и взаимодействию с JSON-ом. 
    /// </summary>
    public class MyJsonInteraction
    {
        public delegate void Message(string someString); // объявляем делегат для вывода сообщений
        private string name; // приватная строка, содержащая имя искомого
        private string surname; // приватная строка, содержащая фамилию искомого

        /// <summary>
        /// Конструктор, инициализирующий поля класса MyJsonInteraction.
        /// В параметрах указываются имя и фамилия искомого.
        /// </summary>
        /// <param name="name"> Строка с именем. </param>
        /// <param name="surname"> Строка с фамилией. </param>
        public MyJsonInteraction(string name, string surname)
        {
            this.name = name;
            this.surname = surname;
        }

        /// <summary>
        /// Метод, позволяющий по имени и фамилии найти
        /// и вывести подробную информацию об
        /// искомых Интерполом преступников :) 
        /// </summary>
        public void ShowWantedPersons()
        {
            Message message = TextWithIndent; // присваиваем делегату адрес метода
            string wantedURL = "https://ws-public.interpol.int/notices/v1/red?&name="
                                    + surname + "&forename=" + name; // создаём GET-запрос
                                                                     // на основе полученных имени и фамилии
            string wantedJson = GetJsonResponse(wantedURL); // получаем ответ в формате JSON
            var wantedResult = Wanted.FromJson(wantedJson); // десереализуем JSON в объект C#


            // проверяем наличие результата. Если массив Notices пустой, 
            // значит, такого разыскиваемого в Red Notices Интерпола не значится.
            if (wantedResult.Embedded.Notices.Length != 0) 
            {
                ShowInfo(wantedResult);
            }
            else
            {
                message("On INTERPOL'S Red Notices we don't find anything" +
                    "at your request.");
            }
        }

        /// <summary>
        /// Метод для вызова текста на консоль с новой строки. 
        /// </summary>
        /// <param name="text">Принимает строку, которую необходимо вывести на консоль.</param>
        private static void TextWithIndent(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        /// Метод для вызова текста на консоль, не переходящего на новую строку. 
        /// </summary>
        /// <param name="text">Принимает строку, которую необходимо вывести на консоль.</param>
        private static void TextWithoutIndent(string text)
        {
            Console.Write(text);
        }

        /// <summary>
        /// Метод, возвращающий ответ в формате JSON.
        /// </summary>
        /// <param name="jsonURL">Принимает строку с GET-запросом.</param>
        /// <returns>Возвращает строку в формате JSON.</returns>
        private static string GetJsonResponse(string jsonURL)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(jsonURL); // создаём запрос
            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // получаем ответ от сервера
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default); // открываем поток, 
                                                                                                          // в который
                                                                                                          // придёт ответ от 
                                                                                                          // сервера

            string jsonResponse = streamReader.ReadToEnd(); // читаем поток до конца и записываем его в строку
            streamReader.Close(); // закрываем поток
            return jsonResponse; // возвращаем строку в формате JSON
        }

        /// <summary>
        /// Метод, показывающий подробную информацию о преступниках.
        /// </summary>
        /// <param name="wantedResult">Принимает параметр типа Wanted, из которого будут браться конкретные искомые.</param>
        private static void ShowInfo(Wanted wantedResult)
        {
            Message message = TextWithIndent; // присваиваем делегату адрес метода

            message($"Search results: {wantedResult.Embedded.Notices.Length}."); // выводим на экран количество найденных результатов
            message("The results of the your request:");
            foreach (var specimen in wantedResult.Embedded.Notices) // проходимся циклом по массиву Notices,
                                                                    // в котором хранится каждый отдельный преступник,
                                                                    // найденный по совпадению из GET-запроса
            {
                string specID = specimen.EntityId.Replace('/', '-'); // в EntityId строка хранится в формате
                                                                     // 0000/00000,
                                                                     // а в GET-запрос принимается формат 0000-00000. 
                string specJsonURL = "https://ws-public.interpol.int/notices/v1/red/" + specID;
                string specJson = GetJsonResponse(specJsonURL); // получаем JSON-ответ 
                var specimenInfo = Details.FromJson(specJson); // десериализуем JSON

                // и выводим на экран подробную информацию о каждом конкретном преступнике :)

                int age = DateTime.Today.Year - Convert.ToDateTime(specimenInfo.DateOfBirth).Year;

                if (DateTime.Now.DayOfYear < Convert.ToDateTime(specimenInfo.DateOfBirth).DayOfYear) age--;

                message($"Name: {specimenInfo.Forename}\n" +
                    $"Surname: {specimenInfo.Name}\n" +
                    $"Date of birth: {specimenInfo.DateOfBirth} " +
                    $"({age} years old)\n" +
                    $"Place of birth: {specimenInfo.PlaceOfBirth}, {specimenInfo.CountryOfBirthId}\n" +
                    $"Charges: {specimenInfo.ArrestWarrants[0].Charge}");

                message = TextWithoutIndent; 
                message("Issued country: "); // стран может быть несколько, поэтому проверяем массив:
                if (specimenInfo.ArrestWarrants.Length > 1) // если длина массива больше 1,
                {
                    foreach (var countrys in specimenInfo.ArrestWarrants) // проходимся циклом по массиву
                    {
                        message(countrys.IssuingCountryId);
                    }
                }
                else message(specimenInfo.ArrestWarrants[0].IssuingCountryId); // иначе берём нулевой элемент массива. 
                message("\n");
                message("Nationality: "); // также национальностей может быть несколько. делаем похожую проверку.
                if (specimenInfo.Nationalities.Length > 1)
                {
                    foreach (var nationality in specimenInfo.Nationalities)
                    {
                        message(nationality + " ");
                    }
                }
                else  message(specimenInfo.Nationalities[0] + "\n");

                message = TextWithIndent; // возвращаем делегату метод, который включает в себя новые строки :)
                message("----------------------------------------------------"); // разделяем инфу красотой в виде полосочки.
            }
        }
    }
}
