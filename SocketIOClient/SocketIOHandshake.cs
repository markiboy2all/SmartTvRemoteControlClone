// Decompiled with JetBrains decompiler
// Type: SocketIOClient.SocketIOHandshake
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketIOClient
{
  public class SocketIOHandshake
  {
    public List<string> Transports = new List<string>();

    public string SID { get; set; }

    public int HeartbeatTimeout { get; set; }

    public string ErrorMessage { get; set; }

    public bool HadError => !string.IsNullOrWhiteSpace(this.ErrorMessage);

    public TimeSpan HeartbeatInterval => new TimeSpan(0, 0, this.HeartbeatTimeout);

    public int ConnectionTimeout { get; set; }

    public static SocketIOHandshake LoadFromString(string value)
    {
      SocketIOHandshake socketIoHandshake = new SocketIOHandshake();
      if (!string.IsNullOrEmpty(value))
      {
        string[] strArray = value.Split(':');
        if (((IEnumerable<string>) strArray).Count<string>() == 4)
        {
          int result1 = 0;
          int result2 = 0;
          socketIoHandshake.SID = strArray[0];
          if (int.TryParse(strArray[1], out result1))
          {
            int num = (int) ((double) result1 * 0.75);
            socketIoHandshake.HeartbeatTimeout = num;
          }
          if (int.TryParse(strArray[2], out result2))
            socketIoHandshake.ConnectionTimeout = result2;
          socketIoHandshake.Transports.AddRange((IEnumerable<string>) strArray[3].Split(','));
          return socketIoHandshake;
        }
      }
      return (SocketIOHandshake) null;
    }
  }
}
