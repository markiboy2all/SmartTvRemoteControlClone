// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.SecondTvResponse
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Devices.SecondTv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  public class SecondTvResponse
  {
    private static readonly IDictionary<string, Func<string, SecondTvResponse>> parsers = (IDictionary<string, Func<string, SecondTvResponse>>) new Dictionary<string, Func<string, SecondTvResponse>>()
    {
      {
        "GetAvailableActionsResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetAvailableActionsResponse>(xml))
      },
      {
        "GetBannerInformationResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetBannerInformationResponse>(xml))
      },
      {
        "GetChannelInformationResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetChannelInformationResponse>(xml))
      },
      {
        "GetChannelListResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) GetChannelListResponse.Parse(xml))
      },
      {
        "GetChannelTypeWithAntennaModeResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) GetChannelTypeWithAntennaModeResponse.Parse(xml))
      },
      {
        "GetCurrentAntennaModeResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetCurrentAntennaModeResponse>(xml))
      },
      {
        "GetCurrentChListTypeResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetCurrentChannelListTypeResponse>(xml))
      },
      {
        "GetCurrentExternalSourceResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetCurrentExternalSourceResponse>(xml))
      },
      {
        "GetCurrentMainTVChannelResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetCurrentMainTvChannelResponse>(xml))
      },
      {
        "GetDetailProgramInformationResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetDetailProgramInformationResponse>(xml))
      },
      {
        "GetDTVInformationResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetDTVInformationResponse>(xml))
      },
      {
        "GetMBRDeviceListResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetMbrDeviceListResponse>(xml))
      },
      {
        "GetRecordChannelResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetRecordChannelResponse>(xml))
      },
      {
        "GetSourceListResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<GetSourceListResponse>(xml))
      },
      {
        "StartSecondTVViewResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<StartSecondTvViewResponse>(xml))
      },
      {
        "StartCloneViewResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<StartCloneViewResponse>(xml))
      },
      {
        "StartExtSourceViewResponse",
        (Func<string, SecondTvResponse>) (xml => (SecondTvResponse) SecondTvResponse.Parse<StartExternalSourceResponse>(xml))
      },
      {
        "SetAntennaModeResponse",
        (Func<string, SecondTvResponse>) (xml => SetResponse.Parse(xml))
      },
      {
        "SetMainTVChannelResponse",
        (Func<string, SecondTvResponse>) (xml => SetResponse.Parse(xml))
      },
      {
        "SetMainTVSourceResponse",
        (Func<string, SecondTvResponse>) (xml => SetResponse.Parse(xml))
      },
      {
        "StopViewResponse",
        (Func<string, SecondTvResponse>) (xml => SetResponse.Parse(xml))
      },
      {
        "StopRecordResponse",
        (Func<string, SecondTvResponse>) (xml => SetResponse.Parse(xml))
      }
    };

    [XmlElement("Result")]
    public string Result { get; set; }

    public ErrorCode ErrorCode
    {
      get
      {
        ErrorCode result = ErrorCode.NOTOK;
        Enum.TryParse<ErrorCode>(this.Result, out result);
        return result;
      }
    }

    public bool IsOk() => this.Result.ToLower() == "ok";

    public static SecondTvResponse Parse(string value)
    {
      string key = string.Empty;
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(value)))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            key = xmlReader.Name;
            while (xmlReader.Read())
            {
              if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Result")
              {
                xmlReader.Read();
                ErrorCode result = ErrorCode.NOTOK;
                if (Enum.TryParse<ErrorCode>(xmlReader.Value, out result))
                {
                  if (result == ErrorCode.OK)
                    break;
                }
                throw new SecondTvException(result);
              }
            }
            break;
          }
        }
      }
      Func<string, SecondTvResponse> func = (Func<string, SecondTvResponse>) null;
      if (!SecondTvResponse.parsers.TryGetValue(key, out func))
        throw new ArgumentException("Unsupported Response Type");
      return func(value);
    }

    protected static ResponseType Parse<ResponseType>(string value) where ResponseType : SecondTvResponse
    {
      using (TextReader textReader = (TextReader) new StringReader(value))
        return (ResponseType) new XmlSerializer(typeof (ResponseType)).Deserialize(textReader);
    }
  }
}
