using System;
using Newtonsoft.Json;

namespace CocoriCore
{


    public class MyTypeConverter : JsonConverter
    {
        public MyTypeConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Type).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new Exception();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }

}