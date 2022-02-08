using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace WantedMain
{
    /// <summary>
    /// Класс, описывающий второй рут ответа на GET-запрос
    /// с необходимым содержимым.
    /// </summary>
    public partial class Wanted
    {
        /// <summary>
        /// Для каждого свойства устанавливаем JsonProperty
        /// в соответствии с его названием в  JSON,
        /// чтобы осуществить парсинг.
        /// </summary>
        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; } // второй рут JSON-a. 

        /// <summary>
        /// Метод, десериализующий Json в объект класса Wanted. 
        /// </summary>
        /// <param name="json"> В качестве параметра принимает строку с json. </param>
        /// <returns> Возвращает объект класса Wanted.</returns>
        public static Wanted FromJson(string json) => JsonConvert.DeserializeObject<Wanted>(json);
    }

    /// <summary>
    /// Класс, описывающий содержимое второго рута.
    /// Содержит раздел Notices, в котором хранится
    /// массив ответов на GET-запрос.
    /// </summary>
    public partial class Embedded
    {
        [JsonProperty("notices")]
        public Notice[] Notices { get; set; } // третий рут JSON-a. 
    }

    /// <summary>
    /// Класс, описывающий содержимое третьего рута, 
    /// содержащий ID искомого с подробной информацией.
    /// </summary>
    public partial class Notice
    {
        [JsonProperty("entity_id")]
        public string EntityId { get; set; } // строка внутри третьего рута JSON-a. 
    }
}
