using Prolly.Configuration;
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
        private Patterns.Timeout _timeoutPolicy;

        public CommandGroup CommandGroup { get; private set; }

        public ProllyCommand(string commandGroupName)
            : this(commandGroupName, TimeoutConfiguration.WaitingTime)
        { }

        public ProllyCommand(string commandGroupName, TimeSpan timeout)
        {
            CommandGroup = CommandGroupFactory.Resolve(commandGroupName);
            _timeoutPolicy = new Patterns.Timeout(timeout);
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
                if(ShouldBypassCircuitBreaker(ex))
                {
                    throw ( ex as AggregateException ).InnerException;
                }

                CommandGroup.CircuitBreaker.TryBreak();

                return TryFallback(ex);
            }
        }

        private bool ShouldBypassCircuitBreaker(Exception ex)
        {
            if ( ex is AggregateException )
            {
                return ( ex as AggregateException ).InnerException is CircuitBreakerIgnoreException;
            }

            return false;
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
