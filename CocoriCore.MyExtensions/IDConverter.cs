using System;
using System.Linq;
using CocoriCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CocoriCore
{
    public class IDConverter : JsonConverter
    {
        public IDConverter()
        {
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableToGeneric(typeof(ID<>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jValue = JValue.Load(reader);
            var s = jValue.ToString();
            var id = Activator.CreateInstance(objectType, s);
            return id;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dynamic d = value;
            writer.WriteValue(d.Id.ToString());
        }
    }
}