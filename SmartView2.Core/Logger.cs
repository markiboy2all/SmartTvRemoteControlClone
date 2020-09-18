// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.Logger
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

namespace SmartView2.Core
{
  public class Logger
  {
    private static Logger instance;

    public static Logger Instance
    {
      get
      {
        if (Logger.instance == null)
          Logger.instance = new Logger();
        return Logger.instance;
      }
      set => Logger.instance = value;
    }

    public virtual void LogError(string message)
    {
    }

    public void LogError(object value) => this.LogError(value.ToString());

    public void LogErrorFormat(string format, params object[] args) => this.LogError(string.Format(format, args));

    public virtual void LogMessage(string message)
    {
    }

    public void LogMessage(object value) => this.LogMessage(value.ToString());

    public void LogMessageFormat(string format, params object[] args) => this.LogMessage(string.Format(format, args));
  }
}
