using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace BranchSdk
{
    public static class JsonConvert
    {
        public static JsonArray SerializeDictionaryAsJson<T1,T2>(this Dictionary<T1,T2> dictionary)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Dictionary<T1, T2>));
            MemoryStream memoryStream = new MemoryStream();
            jsonSerializer.WriteObject(memoryStream, dictionary);
            return JsonArray.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        public static JsonArray SerializeArrayAsJson<T>(this T[] array)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T[]));
            MemoryStream memoryStream = new MemoryStream();
            jsonSerializer.WriteObject(memoryStream, array);
            return JsonArray.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        public static JsonArray SerializeListAsJson<T>(this List<T> list)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<T>));
            MemoryStream memoryStream = new MemoryStream();
            jsonSerializer.WriteObject(memoryStream, list);
            return JsonArray.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        public static JsonArray SerializeContainerAsJson(this object obj)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            jsonSerializer.WriteObject(memoryStream, obj);
            return JsonArray.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        public static JsonObject SerializeObjectAsJson(this object obj)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            jsonSerializer.WriteObject(memoryStream, obj);
            return JsonObject.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }

        public static string SerializeObject(object obj)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            jsonSerializer.WriteObject(memoryStream, obj);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static T DeserializeObject<T>(string json)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return (T)jsonSerializer.ReadObject(memoryStream);
        }

        public static T DeserializeObject<T>(this JsonObject jsonObject)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonObject.ToString()));
            return (T)jsonSerializer.ReadObject(memoryStream);
        }

        public static T DeserializeObject<T>(this JsonArray jsonArray)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonArray.ToString()));
            return (T)jsonSerializer.ReadObject(memoryStream);
        }
    }
}
