using SmartTVRemoteControl.Native.DLNA;
using SmartTVRemoteControl.Native.HTTP;
using System;
using System.Reflection;
using System.Xml;

namespace SmartTVRemoteControl.Native.DLNA.Handler
{
	internal class DescriptionHandler : IHttpHandler
	{
		public string Prefix
		{
			get
			{
				return JustDecompileGenerated_get_Prefix();
			}
			set
			{
				JustDecompileGenerated_set_Prefix(value);
			}
		}

		private string JustDecompileGenerated_Prefix_k__BackingField;

		public string JustDecompileGenerated_get_Prefix()
		{
			return this.JustDecompileGenerated_Prefix_k__BackingField;
		}

		private void JustDecompileGenerated_set_Prefix(string value)
		{
			this.JustDecompileGenerated_Prefix_k__BackingField = value;
		}

		public DescriptionHandler()
		{
			this.Prefix = "/description.xml";
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<?xml version=\"1.0\"?><root xmlns=\"urn:schemas-upnp-org:device-1-0\" xmlns:dlna=\"urn:schemas-dlna-org:device-1-0\" xmlns:sec=\"http://www.sec.co.kr/dlna\"><specVersion><major>1</major><minor>0</minor></specVersion><device><dlna:X_DLNACAP/><dlna:X_DLNADOC>DMS-1.50</dlna:X_DLNADOC><UDN></UDN><dlna:X_DLNADOC>M-DMS-1.50</dlna:X_DLNADOC><friendlyName/><deviceType>urn:schemas-upnp-org:device:MediaServer:1</deviceType><manufacturer>SEC</manufacturer><manufacturerURL>http://www.samsung.com/sec</manufacturerURL> <modelName>miniDLNA Media Server</modelName><modelDescription></modelDescription><modelNumber></modelNumber> <modelURL>http://www.samsung.com/sec</modelURL> <serialNumber></serialNumber><sec:ProductCap>DCM10,getMediaInfo.sec</sec:ProductCap><sec:X_ProductCap>DCM10,getMediaInfo.sec</sec:X_ProductCap><iconList><icon><mimetype>image/jpeg</mimetype><width>48</width><height>48</height><depth>24</depth><url>/icon/smallJPEG</url></icon><icon><mimetype>image/png</mimetype><width>48</width><height>48</height><depth>24</depth><url>/icon/smallPNG</url></icon><icon><mimetype>image/png</mimetype><width>120</width><height>120</height><depth>24</depth><url>/icon/largePNG</url></icon><icon><mimetype>image/jpeg</mimetype><width>120</width><height>120</height><depth>24</depth><url>/icon/largeJPEG</url></icon></iconList><serviceList><service><serviceType>urn:schemas-upnp-org:service:ContentDirectory:1</serviceType><serviceId>urn:upnp-org:serviceId:ContentDirectory</serviceId><SCPDURL>/contentDirectory.xml</SCPDURL><controlURL>/serviceControl</controlURL><eventSubURL></eventSubURL></service></serviceList></device></root>");
			xmlDocument.GetElementsByTagName("UDN").Item(0).InnerText = string.Format("uuid:{0}", DlnaServer.ServerGuid);
			xmlDocument.GetElementsByTagName("modelNumber").Item(0).InnerText = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			xmlDocument.GetElementsByTagName("friendlyName").Item(0).InnerText = Environment.MachineName;
			xmlDocument.GetElementsByTagName("SCPDURL").Item(0).InnerText = "/contentDirectory.xml";
			xmlDocument.GetElementsByTagName("controlURL").Item(0).InnerText = "/control";
			xmlDocument.GetElementsByTagName("eventSubURL").Item(0).InnerText = "/events";
			return new HttpResponse(request, HttpCode.Ok, "text/xml", xmlDocument.OuterXml);
		}
	}
}