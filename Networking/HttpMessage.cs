using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Networking
{
  public class HttpMessage : IEnumerable<string>, IEnumerable
  {
    private readonly IDictionary<string, string> headers = (IDictionary<string, string>) new Dictionary<string, string>();

    private HttpMessage()
    {
    }

    public HttpMessage(Uri address)
      : this(address, "GET", string.Empty, string.Empty, string.Empty)
    {
    }

    public HttpMessage(Uri address, string method)
      : this(address, method, string.Empty, string.Empty, string.Empty)
    {
    }

    public HttpMessage(Uri address, string method, string argument)
      : this(address, method, argument, string.Empty, string.Empty)
    {
    }

    public HttpMessage(
      Uri address,
      string method,
      string argument,
      string contentType,
      string content)
    {
      this.RemoteAddress = address;
      this.Method = method;
      this.MethodArgument = argument;
      this.ContentType = contentType;
      this.Content = content;
    }

    public void Add(string key, string value) => this.headers.Add(key.ToUpper(), value);

    public void Remove(string key) => this.headers.Remove(key.ToUpper());

    public static HttpMessage Parse(string source)
    {
      HttpMessage httpMessage = new HttpMessage();
      Match match = new Regex("^((?<Method>[a-zA-Z-_]+)\\s+)?((?<Argument>.+)\\s+)?HTTP\\s*/\\d\\.\\d\\s*((?<Code>\\d{3})\\s*)?((?<Status>[a-zA-Z ]*)\\s*)?\\r\\n(?<Headers>(.+:.+\\r\\n)*)\\r\\n(?<Content>(.+\\r?\\n?)*)?.*").Match(source);
      if (match.Success)
      {
        httpMessage.Method = match.Groups["Method"].Value;
        httpMessage.MethodArgument = match.Groups["Argument"].Value;
        httpMessage.Content = match.Groups["Content"].Value;
        string[] strArray = match.Groups["Headers"].Value.Split(new char[1]
        {
          '\n'
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < strArray.Length; ++index)
        {
          int length = strArray[index].IndexOf(':');
          if (length > -1)
          {
            string key = strArray[index].Substring(0, length).Trim();
            string str = strArray[index].Substring(length + 1).Trim();
            if (!string.IsNullOrEmpty(key))
              httpMessage.Add(key, str);
          }
        }
        int result = 0;
        if (int.TryParse(match.Groups["Code"].Value, out result))
          httpMessage.Code = result;
        httpMessage.Status = match.Groups["Status"].Value;
      }
      return httpMessage;
    }

    public static HttpMessage Parse(byte[] source) => HttpMessage.Parse(Encoding.UTF8.GetString(source, 0, source.Length));

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrEmpty(this.Method))
      {
        stringBuilder.AppendFormat("{0} ", (object) this.Method);
        if (!string.IsNullOrEmpty(this.MethodArgument))
          stringBuilder.AppendFormat("{0} ", (object) this.MethodArgument);
      }
      stringBuilder.Append("HTTP/1.1");
      if (!string.IsNullOrEmpty(this.Status))
        stringBuilder.AppendFormat(" {0} {1}", (object) this.Code, (object) this.Status);
      stringBuilder.Append("\r\n");
      foreach (KeyValuePair<string, string> header in (IEnumerable<KeyValuePair<string, string>>) this.headers)
        stringBuilder.AppendFormat("{0}: {1}\r\n", (object) header.Key.ToUpper(), (object) header.Value);
      stringBuilder.Append("\r\n");
      if (!string.IsNullOrEmpty(this.Content))
      {
        stringBuilder.Append(this.Content);
        stringBuilder.Append("\r\n");
      }
      return stringBuilder.ToString();
    }

    public byte[] ToBytes() => Encoding.UTF8.GetBytes(this.ToString());

    public IEnumerator GetEnumerator() => (IEnumerator) this.headers.Keys.GetEnumerator();

    IEnumerator<string> IEnumerable<string>.GetEnumerator() => this.headers.Keys.GetEnumerator();

    public string this[string key]
    {
      get
      {
        string empty = string.Empty;
        return this.headers.TryGetValue(key.ToUpper(), out empty) ? empty : string.Empty;
      }
      set
      {
        if (this.headers.ContainsKey(key.ToUpper()))
          this.headers[key.ToUpper()] = value;
        else
          this.headers.Add(key.ToUpper(), value);
      }
    }

    public string Method { get; set; }

    public string MethodArgument { get; set; }

    public int Code { get; set; }

    public string Status { get; set; }

    public string Content { get; set; }

    public string ContentType
    {
      get => this["content-type"];
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this["content-type"] = value;
      }
    }

    public Uri LocalAddress { get; set; }

    public Uri RemoteAddress { get; set; }

    public DateTime ReceiveTime { get; set; }
  }
}
