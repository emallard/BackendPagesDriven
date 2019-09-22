using System;
using System.IO;
using System.Threading.Tasks;
using CocoriCore.Page;
using Newtonsoft.Json;
using Xunit;

namespace CocoriCore
{
    public class CallConverterTest
    {

        [Fact]
        public void Test()
        {
            var call = new Call<Message, Response>(new Message() { Id = Guid.NewGuid() });

            var settings = new JsonSerializerSettings()
            {
                Converters = { new CallConverter() }
            };
            var jsonSerializer = JsonSerializer.Create(settings);

            var jsonString = jsonSerializer.Serialize(call);

            var deserialized = (Call)jsonSerializer.Deserialize(
                    new JsonTextReader(new StringReader(jsonString)),
                    typeof(Call));

            Console.WriteLine(deserialized.GetType()); // Must be Call<Message, Response>
        }

        class Message : IMessage<Response>
        {
            public Guid Id;
        }
        class Response
        {

        }
    }
}