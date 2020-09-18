// Decompiled with JetBrains decompiler
// Type: UPnP.UPnPService
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using Networking;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UPnP
{
  internal class UPnPService : IUPnPService, IDisposable
  {
    private readonly INetworkTransport transport;
    private readonly INetworkTransport server;

    public UPnPService(INetworkTransport transport, INetworkTransport server)
    {
      if (transport == null)
        throw new ArgumentNullException(nameof (transport));
      if (server == null)
        throw new ArgumentNullException(nameof (server));
      this.transport = transport;
      this.server = server;
      this.server.MessageReceived += new EventHandler<HttpMessageEventArgs>(this.server_MessageReceived);
    }

    public void Connect(string callbackAddress, int callbackPort) => this.transport.SendRequest(this.CreateSubscriptionRequest(callbackAddress, callbackPort));

    public async Task ConnectAsync(string callbackAddress, int callbackPort)
    {
      HttpMessage request = this.CreateSubscriptionRequest(callbackAddress, callbackPort);
      HttpMessage httpMessage = await this.transport.SendRequestAsync(request);
    }

    public string InvokeAction(string actionName, params ActionArgument[] args) => this.transport.SendRequest(this.CreateActionInvokeRequest(actionName, args)).Content;

    public async Task<string> InvokeActionAsync(string actionName, params ActionArgument[] args)
    {
      HttpMessage request = this.CreateActionInvokeRequest(actionName, args);
      HttpMessage response = await this.transport.SendRequestAsync(request);
      return response.Content;
    }

    private void server_MessageReceived(object sender, HttpMessageEventArgs e)
    {
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(e.Message.Content)))
      {
        if (!xmlReader.ReadToFollowing("e:propertyset"))
          return;
        while (xmlReader.ReadToFollowing("e:property"))
        {
          string property = string.Empty;
          string empty = string.Empty;
          if (xmlReader.Read() && xmlReader.NodeType == XmlNodeType.Element)
            property = xmlReader.Name;
          if (xmlReader.Read())
            empty = xmlReader.Value;
          this.OnPropertyChanged((object) this, new PropertyChangedEventArgs(property, empty));
        }
      }
    }

    private HttpMessage CreateSubscriptionRequest(
      string callbackAddress,
      int callbackPort) => new HttpMessage(this.EventUrl, "SUBSCRIBE")
    {
      {
        "CALLBACK",
        string.Format("<http://{0}:{1}>", (object) callbackAddress, (object) callbackPort)
      },
      {
        "NT",
        "upnp:event"
      },
      {
        "timeout",
        "Second-infinite"
      }
    };

    private HttpMessage CreateActionInvokeRequest(
      string actionName,
      params ActionArgument[] args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      StringBuilder output = stringBuilder;
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "   ",
        Encoding = Encoding.UTF8
      };
      using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
      {
        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("s", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
        xmlWriter.WriteAttributeString("s", "encodingStyle", "http://schemas.xmlsoap.org/soap/envelope/", "http://schemas.xmlsoap.org/soap/encoding/");
        xmlWriter.WriteStartElement("s", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
        xmlWriter.WriteStartElement("u", actionName, this.ServiceType);
        foreach (ActionArgument actionArgument in args)
          xmlWriter.WriteElementString(actionArgument.Name, actionArgument.Value.ToString());
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
        xmlWriter.Flush();
      }
      string content = stringBuilder.ToString().Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
      return new HttpMessage(this.ControlUrl, "POST", string.Empty, "text/xml; charset=\"utf-8\"", content)
      {
        {
          "SOAPACTION",
          string.Format("\"{0}#{1}\"", (object) this.ServiceType, (object) actionName)
        }
      };
    }

    public void Dispose()
    {
      this.server.MessageReceived -= new EventHandler<HttpMessageEventArgs>(this.server_MessageReceived);
      this.transport.Dispose();
      this.server.Dispose();
    }

    public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      EventHandler<PropertyChangedEventArgs> propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(sender, e);
    }

    public string ID { get; set; }

    public string ServiceType { get; set; }

    public Uri ControlUrl { get; set; }

    public Uri EventUrl { get; set; }
  }
}
