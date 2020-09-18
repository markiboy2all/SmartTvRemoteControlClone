// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.ApplicationError
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;

namespace com.samsung.multiscreen.application
{
  public class ApplicationError
  {
    private long code = -1;
    private string message = "error";

    public ApplicationError()
    {
    }

    public ApplicationError(string message)
      : this(-1L, message)
    {
    }

    public ApplicationError(long code, string message)
    {
      this.code = code;
      this.message = message;
    }

    public string toString() => "[ApplicationError]" + " code: " + (object) this.code + ", message: " + this.message;

    public virtual long Code => this.code;

    public virtual string Message => this.message;

    public static ApplicationError CreateWithException(Exception e) => new ApplicationError(e.ToString());
  }
}
