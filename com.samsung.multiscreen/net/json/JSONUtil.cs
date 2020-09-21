// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.json.JSONUtil
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.net.json
{
  internal class JSONUtil
  {
    public static IDictionary<string, object> Parse(string data)
    {
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new Dictionary<string, object>();
      try
      {
        dictionary = JsonConvert.DeserializeObject<IDictionary<string, object>>(JObject.Parse(data).ToString());
      }
      catch (InvalidCastException ex)
      {
        Logger.Trace((Exception) ex);
      }
      catch (JsonException ex)
      {
        Logger.Trace((Exception) ex);
      }
      return dictionary;
    }

    public static IList<IDictionary<string, object>> ParseList(string data)
    {
      IList<IDictionary<string, object>> dictionaryList = (IList<IDictionary<string, object>>) new List<IDictionary<string, object>>();
      try
      {
        dictionaryList = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JObject.Parse(data).ToString());
      }
      catch (InvalidCastException ex)
      {
        Logger.Trace((Exception) ex);
      }
      catch (JsonException ex)
      {
        Logger.Trace((Exception) ex);
      }
      return dictionaryList;
    }
  }
}
