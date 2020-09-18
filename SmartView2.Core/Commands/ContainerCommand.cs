// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.Commands.ContainerCommand
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.Windows.Input;

namespace SmartView2.Core.Commands
{
  public class ContainerCommand : ICommand
  {
    private readonly ICommand command;

    public ContainerCommand(ICommand command) => this.command = command != null ? command : throw new ArgumentNullException(nameof (command));

    public bool CanExecute(object parameter) => this.command.CanExecute(parameter);

    public void Execute(object parameter)
    {
      this.BeforeExecute();
      this.command.Execute(parameter);
      this.AfterExecute();
    }

    protected virtual void BeforeExecute()
    {
    }

    protected virtual void AfterExecute()
    {
    }

    public event EventHandler CanExecuteChanged
    {
      add => this.command.CanExecuteChanged += value;
      remove => this.command.CanExecuteChanged -= value;
    }
  }
}
