using Microsoft.AspNetCore.Components;
using SmartView2.Core;
using System;
using System.Threading.Tasks;

namespace UIFoundation
{
    public class DispatcherWrapper : IBaseDispatcher
    {
        private readonly Dispatcher dispatcher;

        public DispatcherWrapper(Dispatcher dispatcher) => this.dispatcher = dispatcher;

        public void Invoke(Action callback) => this.dispatcher.InvokeAsync(callback);

        public async Task<TResult> Invoke<TResult>(Func<TResult> callback) => await this.dispatcher.InvokeAsync<TResult>(callback);

        //public void Invoke(Action callback, DispatcherPriority priority) => this.dispatcher.InvokeAsync(callback, priority);

        //public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority) => this.dispatcher.Invoke<TResult>(callback, priority);

        //public DispatcherOperation InvokeAsync(Action callback) => this.dispatcher.InvokeAsync(callback);

        //public DispatcherOperation<TResult> InvokeAsync<TResult>( Func<TResult> callback) => this.dispatcher.InvokeAsync<TResult>(callback);

        //public DispatcherOperation InvokeAsync( Action callback, DispatcherPriority priority) => this.dispatcher.InvokeAsync(callback, priority);
    }
}
