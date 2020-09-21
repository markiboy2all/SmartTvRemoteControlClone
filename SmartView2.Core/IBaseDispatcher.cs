using System;

namespace SmartView2.Core
{
  public interface IBaseDispatcher
  {
    void Invoke(Action action);
  }
}
