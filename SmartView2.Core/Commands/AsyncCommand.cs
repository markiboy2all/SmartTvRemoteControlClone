// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.Commands.AsyncCommand
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartView2.Core.Commands
{
  public class AsyncCommand : ICommand
  {
    private readonly Func<object, Task> action;
    private readonly Predicate<object> canExecute;
    private bool isExecuting;

    public AsyncCommand(Func<object, Task> action)
      : this(action, (Predicate<object>) (arg => true))
    {
    }

    public AsyncCommand(Func<object, Task> action, Predicate<object> canExecute)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (canExecute == null)
        throw new ArgumentNullException(nameof (canExecute));
      this.action = action;
      this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => !this.isExecuting && this.canExecute(parameter);

    public async void Execute(object parameter)
    {
      if (!this.CanExecute(parameter))
        return;
      try
      {
        this.isExecuting = true;
        this.OnCanExecuteChanged((object) this, EventArgs.Empty);
        await this.action(parameter);
      }
      finally
      {
        this.isExecuting = false;
        this.OnCanExecuteChanged((object) this, EventArgs.Empty);
      }
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
