// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.DeviceFactory
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace com.samsung.multiscreen.device
{
  internal class DeviceFactory
  {
    internal DeviceFactory()
    {
    }

    public static Device ParseDevice(HttpWebResponse response)
    {
      try
      {
        IDictionary<string, object> map = JSONUtil.Parse(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
        return map.Count > 0 ? DeviceFactory.CreateWithMap(map) : (Device) null;
      }
      catch (Exception ex)
      {
        return (Device) null;
      }
    }

    public static Device ParseDeviceWithCapability(
      HttpWebResponse response,
      string targetCapability)
    {
      Device device = (Device) null;
      try
      {
        IDictionary<string, object> map = JSONUtil.Parse(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
        if (map.Count > 0)
        {
          if (map.ContainsKey("Capabilities"))
          {
            JArray jarray = (JArray) map["Capabilities"];
            if (jarray != null)
            {
              for (int index = 0; index < jarray.Count; ++index)
              {
                JObject jobject = (JObject) jarray[index];
                string str1 = (string) jobject["name"];
                Logger.Trace("Cloud device capability: " + str1);
                Console.WriteLine("Cloud device capability: " + str1);
                if (str1 != null && str1.Equals(targetCapability, StringComparison.CurrentCultureIgnoreCase))
                {
                  string str2 = (string) jobject["port"];
                  string str3 = (string) jobject["location"];
                  string str4 = "http://" + (string) map["IP"] + ":" + str2 + str3;
                  map["ServiceURI"] = (object) str4;
                  return DeviceFactory.CreateWithMap(map);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Trace(ex);
        Console.WriteLine("Error while parsing JSON: " + ex.ToString());
        device = (Device) null;
      }
      return device;
    }

    public static Device ParseDeviceWithCapability(
      string responseBody,
      string targetCapability)
    {
      IDictionary<string, object> map = JSONUtil.Parse(responseBody);
      try
      {
        if (map.Count > 0)
        {
          if (map.ContainsKey("Capabilities"))
          {
            JArray jarray = (JArray) map["Capabilities"];
            if (jarray != null)
            {
              for (int index = 0; index < jarray.Count; ++index)
              {
                JObject jobject = (JObject) jarray[index];
                string str1 = (string) jobject["name"];
                Logger.Trace("Cloud device capability: " + str1);
                Console.WriteLine("Cloud device capability: " + str1);
                if (str1 != null && str1.Equals(targetCapability, StringComparison.CurrentCultureIgnoreCase))
                {
                  string str2 = (string) jobject["port"];
                  string str3 = (string) jobject["location"];
                  string str4 = "http://" + (string) map["IP"] + ":" + str2 + str3;
                  map["ServiceURI"] = (object) str4;
                  return DeviceFactory.CreateWithMap(map);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Trace(ex);
        Console.WriteLine("Error while parsing JSON: " + ex.ToString());
      }
      return (Device) null;
    }

    public static IList<Device> ParseDeviceList(HttpWebResponse response)
    {
      IList<Device> deviceList = (IList<Device>) null;
      try
      {
        IList<IDictionary<string, object>> list = JSONUtil.ParseList(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
        Logger.Trace("parseDeviceList() jsonCloudDeviceList: " + (object) list.Count);
        Console.WriteLine("parseDeviceList() jsonCloudDeviceList: " + (object) list.Count);
        deviceList = (IList<Device>) new List<Device>();
        foreach (IDictionary<string, object> map in (IEnumerable<IDictionary<string, object>>) list)
        {
          Device device = map.Count > 0 ? DeviceFactory.CreateWithMap(map) : (Device) null;
          if (device != null && !deviceList.Contains(device))
            deviceList.Add(device);
        }
      }
      catch (Exception ex)
      {
      }
      return deviceList;
    }

    public static Device CreateWithMapAndAppURI(
      IDictionary<string, object> map,
      Uri applicationURI)
    {
      if (map != null && applicationURI != (Uri) null)
        map["DialURI"] = (object) applicationURI.ToString();
      return DeviceFactory.CreateWithMap(map);
    }

    public static Device CreateWithMap(IDictionary<string, object> map)
    {
      if (map == null)
        return (Device) null;
      IDictionary<string, string> attributes = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) map)
      {
        if (keyValuePair.Value is string)
        {
          string str = (string) keyValuePair.Value;
          attributes[keyValuePair.Key] = str;
        }
      }
      return new Device(attributes);
    }
  }
}
