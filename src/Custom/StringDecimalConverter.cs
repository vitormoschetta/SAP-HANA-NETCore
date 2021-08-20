using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace src.Custom
{
    public class StringDecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Sap.Data.Hana.HanaDecimal);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue($"{value.ToString().Replace(",", "."):0.00}");
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return token;
        }
    }
}