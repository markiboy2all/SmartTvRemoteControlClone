// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.Commands.Command
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.Windows.Input;

namespace SmartView2.Core.Commands
{
  public sealed class Command : ICommand
  {
    private readonly Action<object> action;
    private readonly Predicate<object> canExecute;

    public Command(Action<object> action)
      : this(action, (Predicate<object>) (arg => true))
    {
    }

    public Command(Action<object> action, Predicate<object> canExecute)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (canExecute == null)
        throw new ArgumentNullException(nameof (canExecute));
      this.action = action;
      this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => this.canExecute(parameter);

    public void Execute(object parameter)
    {
      if (!this.CanExecute(parameter))
        return;
      this.action(parameter);
    }

    public event EventHandler CanExecuteChanged;

    private void OnCanExecuteChanged(object sender, EventArgs e)
    {
      EventHandler canExecuteChanged = this.CanExecuteChanged;
      if (canExecuteChanged == null)
        return;
      canExecuteChanged(sender, e);
    }
  }
}
