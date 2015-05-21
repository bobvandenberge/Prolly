using Prolly.Configuration;
using Prolly.Exceptions;
using Prolly.Patterns;
using Prolly.Patterns.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prolly.Commands
{
    /// <summary>
    /// Used to execute functionality that might go wrong. By default this class implements
    /// the CircuitBreaker pattern and the Timeout pattern.
    /// </summary>
    /// <typeparam name="T">The return type of the risky functionality</typeparam>
    public abstract class ProllyCommand<T>
    {
        private ITimeout _timeout;

        /// <summary>
        /// The command group that this command was placed in
        /// </summary>
        public CommandGroup CommandGroup { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProllyCommand{T}"/> class.
        /// Uses the <see cref="Patterns.Timeout.SimpleTimeout"/> as implementation of the <see cref="Patterns.ITimeout"/>
        /// </summary>
        /// <param name="commandGroupName">Name of the command group.</param>
        public ProllyCommand(string commandGroupName)
            : this(commandGroupName, new SimpleTimeout(TimeoutConfiguration.WaitingTime))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProllyCommand{T}"/> class.
        /// </summary>
        /// <param name="commandGroupName">Name of the command group.</param>
        /// <param name="timeout">The timeout.</param>
        public ProllyCommand(string commandGroupName, ITimeout timeout)
        {
            CommandGroup = CommandGroupFactory.Resolve(commandGroupName);
            _timeout = timeout;
        }

        /// <summary>
        /// Executes the command. This wil start the Run method. The command will not
        /// be execute if the CircuitBreaker is not allowing requests. 
        /// </summary>
        /// <returns>{T}</returns>
        /// <exception cref="CircuitBreakerOpenException">Cannot execute. The CircuitBreaker is Open.</exception>
        public T Execute()
        {
            try
            {
                if ( !CommandGroup.CircuitBreaker.AllowRequest )
                    throw new CircuitBreakerOpenException("Cannot execute. The CircuitBreaker is Open.");

                Task<T> task = RunAsync();
            
                _timeout.Monitor(task);
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

        /// <summary>
        /// Executes the command asynchronous. This wil start the Run method. The command will not
        /// be execute if the CircuitBreaker is not allowing requests. 
        /// </summary>
        /// <returns>{T}</returns>
        /// <exception cref="CircuitBreakerOpenException">Cannot execute. The CircuitBreaker is Open.</exception>
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
