// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.SecondTv.XmlHelper
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

namespace SmartView2.Devices.SecondTv
{
  public static class XmlHelper
  {
    public static string ToQuote(this string value) => value.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

    public static string FromQuote(this string value) => value.Replace("&lt;", "<").Replace("&gt;", ">").Replace("\"", "&quot;");
  }
}
