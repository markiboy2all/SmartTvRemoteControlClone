// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.json.JSONRPCMessage
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace com.samsung.multiscreen.net.json
{
  public class JSONRPCMessage
  {
    private const string JSON_RPC_VERSION = "2.0";
    private const string KEY_JSON_RPC = "jsonrpc";
    private const string KEY_METHOD = "method";
    private const string KEY_PARAMS = "params";
    private const string KEY_RESULT = "result";
    private const string KEY_ERROR = "error";
    public static string KEY_ID = "id";
    public static string KEY_TO = "to";
    public static string KEY_FROM = "from";
    public static string KEY_MESSAGE = "message";
    public static string KEY_ENCRYPTED = "encrypted";
    public static string KEY_CLIENT_ID = "clientId";
    public static string KEY_CLIENTS = "clients";
    public static string KEY_ALL = "all";
    public static string KEY_BROADCAST = "broadcast";
    public static string KEY_HOST = "host";
    private JSONRPCMessage.MessageType type;
    private IDictionary<string, object> map;

    public JSONRPCMessage(IDictionary<string, object> map)
    {
      this.map = map;
      if (map.ContainsKey("result"))
        this.type = JSONRPCMessage.MessageType.RESULT;
      else if (map.ContainsKey("error"))
        this.type = JSONRPCMessage.MessageType.ERROR;
      else if (map.ContainsKey(JSONRPCMessage.KEY_ID))
        this.type = JSONRPCMessage.MessageType.MESSAGE;
      if (!map.ContainsKey("params"))
        return;
      object obj = map["params"];
      if (!(obj is JObject))
        return;
      JObject jobject = (JObject) obj;
      map["params"] = (object) JsonConvert.DeserializeObject<Dictionary<string, object>>(jobject.ToString());
    }

    public JSONRPCMessage(string method)
      : this(JSONRPCMessage.MessageType.NOTIFICATION, method)
    {
    }

    public JSONRPCMessage(JSONRPCMessage.MessageType type, string method)
    {
      this.type = type;
      this.map = (IDictionary<string, object>) new Dictionary<string, object>();
      this.map["jsonrpc"] = (object) "2.0";
      this.map[JSONRPCMessage.KEY_ID] = (object) Guid.NewGuid().ToString();
      this.map[nameof (method)] = (object) method;
      this.map["params"] = (object) new Dictionary<string, object>();
    }

    public override string ToString() => this.toJSONString();

    public virtual JSONRPCMessage.MessageType Type => this.type;

    public virtual string Id => (string) this.map[JSONRPCMessage.KEY_ID];

    public virtual string Method => (string) this.map["method"];

    public virtual IDictionary<string, object> Params
    {
      get => (IDictionary<string, object>) this.map["params"];
      set => this.map["params"] = (object) value;
    }

    public virtual bool IsError() => this.type == JSONRPCMessage.MessageType.ERROR;

    public bool IsResult() => this.type == JSONRPCMessage.MessageType.RESULT;

    public virtual JSONRPCError GetError() => JSONRPCError.CreateWithJSONData(this.map["error"].ToString());

    public IDictionary<string, object> GetResult()
    {
            if (map.ContainsKey("result"))
            {
                object obj = map["result"];
                if (obj is IDictionary)
                {
                    return (IDictionary<string, object>)obj;
                }
                if (obj is JObject)
                {
                    JObject jObject = obj as JObject;
                    IDictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jObject.ToString());
                    dictionary["success"] = jObject["success"];
                    return dictionary;
                }
                if (obj is bool?)
                {
                    bool? flag = (bool?)obj;
                    IDictionary<string, object> dictionary2 = new Dictionary<string, object>();
                    dictionary2["success"] = flag;
                    return dictionary2;
                }
            }
            return null;
        }

    public virtual JObject ToJSON() => JObject.Parse(JsonConvert.SerializeObject((object) this.map));

    public virtual string toJSONString() => this.ToJSON().ToString();

    public static JSONRPCMessage CreateWithJSONData(string jsonData)
    {
      IDictionary<string, object> map = JSONUtil.Parse(jsonData);
      return map == null ? (JSONRPCMessage) null : new JSONRPCMessage(map);
    }

    public static JSONRPCMessage CreateSendMessage(object to, string message) => new JSONRPCMessage("ms.channel.sendMessage")
    {
      Params = {
        [JSONRPCMessage.KEY_TO] = to,
        [JSONRPCMessage.KEY_MESSAGE] = (object) message
      }
    };

    public enum MessageType
    {
      MESSAGE,
      NOTIFICATION,
      RESULT,
      ERROR,
    }
  }
}
