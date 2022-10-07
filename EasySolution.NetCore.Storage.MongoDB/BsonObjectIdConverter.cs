using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.MongoDB
{
    public class BsonObjectIdConverter : JsonConverter<BsonObjectId>
    {
        public override BsonObjectId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new BsonObjectId(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, BsonObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.AsString);
        }
    }
    //internal class BsonObjectIdConverter : JsonConverterFactory
    //{
    //    public override bool CanConvert(Type typeToConvert)
    //    {
    //        if (typeToConvert == typeof(BsonObjectId)) return true;
    //        return false;
    //    }

    //    public override JsonConverter? CreateConverter(Type type, JsonSerializerOptions options)
    //    {
    //        JsonConverter converter = (JsonConverter)Activator.CreateInstance(
    //            typeof(BsonObjectId).MakeGenericType(
    //                new Type[] { keyType, valueType }),
    //            BindingFlags.Instance | BindingFlags.Public,
    //            binder: null,
    //            args: new object[] { options },
    //            culture: null)!;

    //        return converter;
    //    }


    //    private class BsonObjectIDConverterInner: JsonConverter<BsonObjectId>
    //    {
    //        JsonSerializerOptions _options;
    //        public BsonObjectIDConverterInner(JsonSerializerOptions options)
    //        {
    //            _options = options;
    //        }

    //        public override BsonObjectId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //        {
    //            while (reader.Read()) {
    //                if (reader.TokenType == JsonTokenType.EndObject) return null;
    //                Console.WriteLine(reader.GetString());
    //            }
    //        }
    //    }

    //}
}
