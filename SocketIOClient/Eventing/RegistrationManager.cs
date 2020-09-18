// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Eventing.RegistrationManager
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using SocketIOClient.Messages;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SocketIOClient.Eventing
{
  public class RegistrationManager : IDisposable
  {
    private ConcurrentDictionary<int, Action<object>> callBackRegistry;
    private ConcurrentDictionary<string, Action<IMessage>> eventNameRegistry;

    public RegistrationManager()
    {
      this.callBackRegistry = new ConcurrentDictionary<int, Action<object>>();
      this.eventNameRegistry = new ConcurrentDictionary<string, Action<IMessage>>();
    }

    public void AddCallBack(IMessage message)
    {
      EventMessage eventMessage = message as EventMessage;
      if (eventMessage == null)
        return;
      this.callBackRegistry.AddOrUpdate(eventMessage.AckId.Value, eventMessage.Callback, (Func<int, Action<object>, Action<object>>) ((key, oldValue) => eventMessage.Callback));
    }

    public void AddCallBack(int ackId, Action<object> callback) => this.callBackRegistry.AddOrUpdate(ackId, callback, (Func<int, Action<object>, Action<object>>) ((key, oldValue) => callback));

    public void InvokeCallBack(int? ackId, string value)
    {
      Action<object> action = (Action<object>) null;
      if (!ackId.HasValue || !this.callBackRegistry.TryRemove(ackId.Value, out action))
        return;
      action.BeginInvoke((object) value, new AsyncCallback(action.EndInvoke), (object) null);
    }

    public void InvokeCallBack(int? ackId, JsonEncodedEventMessage value)
    {
      Action<object> action = (Action<object>) null;
      if (!ackId.HasValue || !this.callBackRegistry.TryRemove(ackId.Value, out action))
        return;
      action((object) value);
    }

    public void AddOnEvent(string eventName, Action<IMessage> callback) => this.eventNameRegistry.AddOrUpdate(eventName, callback, (Func<string, Action<IMessage>, Action<IMessage>>) ((key, oldValue) => callback));

    public void AddOnEvent(string eventName, string endPoint, Action<IMessage> callback) => this.eventNameRegistry.AddOrUpdate(string.Format("{0}::{1}", (object) eventName, (object) endPoint), callback, (Func<string, Action<IMessage>, Action<IMessage>>) ((key, oldValue) => callback));

    public bool InvokeOnEvent(IMessage value)
    {
      bool flag = false;
      try
      {
        string key = value.Event;
        if (!string.IsNullOrWhiteSpace(value.Endpoint))
          key = string.Format("{0}::{1}", (object) value.Event, (object) value.Endpoint);
        Action<IMessage> action;
        if (this.eventNameRegistry.TryGetValue(key, out action))
        {
          flag = true;
          action(value);
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Exception on InvokeOnEvent: " + ex.Message);
      }
      return flag;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      this.callBackRegistry.Clear();
      this.eventNameRegistry.Clear();
    }
  }
}
