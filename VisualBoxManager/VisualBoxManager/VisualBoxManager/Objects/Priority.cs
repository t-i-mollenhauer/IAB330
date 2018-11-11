using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualBoxManager.Objects
{
    public class Priority
    {
        [JsonConverter(typeof(Priority))]
        public int priority;
     
        
    }

    public class PriorityConverter : JsonConverter
    {
        private readonly Type[] _types;
        public PriorityConverter(params Type[] types)
        {
            _types = types;
        }

    public override bool CanConvert(Type objectType)
        {
            return false;
        }
        public override bool CanWrite {
            get { return false; }
        }

        public override bool CanRead {
            get { return true; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return string.Empty;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                return serializer.Deserialize(reader, objectType);
            }
            else
            {
                JObject obj = JObject.Load(reader);
                if (obj["priority"] != null)
                    return obj["priority"];
                else
                    return serializer.Deserialize(reader, objectType);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
