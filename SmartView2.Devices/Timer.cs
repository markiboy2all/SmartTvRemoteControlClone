// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.Timer
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartView2.Devices
{
  internal sealed class Timer : CancellationTokenSource, IDisposable
  {
    internal Timer(TimerCallback callback, object state, int dueTime, int period)
    {
      Timer timer = this;
      Task.Delay(dueTime, this.Token).ContinueWith<Task>((Func<Task, object, Task>) (async (t, s) =>
      {
        Tuple<TimerCallback, object> tuple = (Tuple<TimerCallback, object>) s;
        while (!timer.IsCancellationRequested)
        {
          Action action = (Action) (() => tuple.Item1(tuple.Item2));
          await Task.Run(action);
          await Task.Delay(period);
        }
      }), (object) Tuple.Create<TimerCallback, object>(callback, state), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
    }

    public new void Dispose() => this.Cancel();
  }
}
