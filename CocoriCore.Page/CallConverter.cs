using System;
using System.Linq;
using CocoriCore;
using CocoriCore.Router;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CocoriCore
{
    public class CallConverter : JsonConverter
    {
        public CallConverter()
        {
        }

        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            return typeof(Call).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var call = jObject.ToObject<Call>();
            if (call._Type.IsAssignableTo(typeof(IAsyncCall)))
            {
                var helper = jObject.ToObject<AsyncCallDeserializationHelper>();
                var asyncCall = (IAsyncCall)jObject.ToObject(helper._Type);
                asyncCall.SetPageQuery(jObject["PageQuery"].ToObject(helper._PageQueryType));
                return asyncCall;
            }
            else
            {
                var newObj = jObject.ToObject(call._Type);
                return newObj;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }

}