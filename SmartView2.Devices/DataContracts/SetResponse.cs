// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.SetResponse
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.IO;
using System.Xml;

namespace SmartView2.Devices.DataContracts
{
  public class SetResponse : SecondTvResponse
  {
    public new static SecondTvResponse Parse(string value)
    {
      SetResponse setResponse = new SetResponse();
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(value)))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Result")
          {
            xmlReader.Read();
            setResponse.Result = xmlReader.Value;
            break;
          }
        }
      }
      return (SecondTvResponse) setResponse;
    }
  }
}
