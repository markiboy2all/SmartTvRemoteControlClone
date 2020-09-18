using System;

namespace Networking
{
  public class HttpMessageEventArgs : EventArgs
  {
    public HttpMessage Message { get; set; }

    public HttpMessageEventArgs()
    {
    }

    public HttpMessageEventArgs(HttpMessage message) => this.Message = message;
  }
}
