using Prolly.Exceptions;
using Prolly.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prolly.Commands
{
    public abstract class ProllyCommand<T>
    {
        private Patterns.Timeout _timeoutPolicy = new Patterns.Timeout();

        public CommandGroup CommandGroup { get; private set; }

        public ProllyCommand(string commandGroupName)
        {
            CommandGroup = CommandGroupFactory.Resolve(commandGroupName);
        }

        public T Execute()
        {
            try
            {
                if ( !CommandGroup.CircuitBreaker.AllowRequest )
                    throw new CircuitBreakerOpenException("Cannot execute. The CircuitBreaker is Open.");

                Task<T> task = RunAsync();
            
                _timeoutPolicy.Monitor(task);
                CommandGroup.CircuitBreaker.MarkSucces();
                return task.Result;
            }
            catch ( Exception ex )
            {
                CommandGroup.CircuitBreaker.TryBreak();

                return TryFallback(ex);
            }
        }

        public Task<T> ExecuteAsync()
        {
            return Task.Factory.StartNew<T>(() =>
            {
                return Execute();
            });
        }

        protected abstract T Run();

        protected virtual T Fallback()
        {
            throw new FallbackNotSpecifiedException("No fallback specified.");
        }

        private Task<T> RunAsync()
        {
            return Task.Factory.StartNew<T>(() =>
            {
                return Run();
            });
        }

        private T TryFallback(Exception ex)
        {
            try
            {
                return Fallback();
            }
            catch ( FallbackNotSpecifiedException )
            {
                throw ex;
            }
        }
    }
}
