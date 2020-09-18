using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SocketIOClient.Messages
{
	public class JsonEncodedEventMessage
	{
		[JsonProperty(PropertyName = "name")]
		public string Name
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "args")]
		public dynamic[] Args
		{
			get;
			set;
		}

		public JsonEncodedEventMessage()
		{
		}

		public JsonEncodedEventMessage(string name, object payload)
			: this(name, new object[1]
			{
			payload
			})
		{
		}

		public JsonEncodedEventMessage(string name, object[] payloads)
		{
			Name = name;
			Args = payloads;
		}

		public T GetFirstArgAs<T>()
		{
			try
			{
				dynamic val = Args.FirstOrDefault();
				if (val != null)
				{
					return JsonConvert.DeserializeObject<T>(val.ToString());
				}
			}
			catch (Exception)
			{
				throw;
			}
			return default(T);
		}

		public IEnumerable<T> GetArgsAs<T>()
		{
			List<T> list = new List<T>();
			object[] args = Args;
			foreach (dynamic val in args)
			{
				list.Add(JsonConvert.DeserializeObject<T>(val.ToString(Formatting.None)));
			}
			return list.AsEnumerable();
		}

		public string ToJsonString()
		{
			return JsonConvert.SerializeObject(this, Formatting.None);
		}

		public static JsonEncodedEventMessage Deserialize(string jsonString)
		{
			jsonString = jsonString.Replace("\"[", "[\"").Replace("]\"", "\"]");
			JsonEncodedEventMessage result = null;
			try
			{
				result = JsonConvert.DeserializeObject<JsonEncodedEventMessage>(jsonString);
				return result;
			}
			catch (Exception value)
			{
				Trace.WriteLine(value);
				return result;
			}
		}
	}
}
