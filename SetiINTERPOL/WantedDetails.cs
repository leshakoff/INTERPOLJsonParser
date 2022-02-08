using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WantedDetails
{
    /// <summary>
    /// Класс, описывающий подробную информацию об искомом. 
    /// </summary>
    public partial class Details
    {
        /// <summary>
        /// Для каждого свойства устанавливаем JsonProperty
        /// в соответствии с его названием в  JSON,
        /// чтобы осуществить парсинг.
        /// </summary>
        [JsonProperty("arrest_warrants")]
        public ArrestWarrant[] ArrestWarrants { get; set; } // массив, содержащий подробности обвинения 

        [JsonProperty("forename")]
        public string Forename { get; set; } // имя

        [JsonProperty("name")]
        public string Name { get; set; } // фамилия

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; } // дата рождения

        [JsonProperty("nationalities")]
        public string[] Nationalities { get; set; } // гражданство

        [JsonProperty("country_of_birth_id")]
        public string CountryOfBirthId { get; set; } // страна проживания

        [JsonProperty("place_of_birth")]
        public string PlaceOfBirth { get; set; } // место рождения

        /// <summary>
        /// Метод, десериализующий Json в объект класса Details. 
        /// </summary>
        /// <param name="json"> В качестве параметра принимает строку с json. </param>
        /// <returns> Возвращает объект класса Details.</returns>
        public static Details FromJson(string json) => JsonConvert.DeserializeObject<Details>(json);

    }


    /// <summary>
    /// Класс, описывающий подробности обвинения
    /// </summary>
    public partial class ArrestWarrant
    {

        [JsonProperty("issuing_country_id")]
        public string IssuingCountryId { get; set; } // страна, в которой разыскивается искомый

        [JsonProperty("charge")]
        public string Charge { get; set; } // обвинения
        
    }

}
