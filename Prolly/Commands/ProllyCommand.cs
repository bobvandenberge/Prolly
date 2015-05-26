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
    /// <summary>
    /// Used to execute functionality that might go wrong. By default this class implements
    /// the CircuitBreaker pattern and the Timeout pattern.
    /// </summary>
    /// <typeparam name="T">The return type of the risky functionality</typeparam>
    public abstract class ProllyCommand<T>
    {
        private static object _lock = new object();
        private AbstractTimeout _timeout;

        public CommandGroupKey CommandGroupKey { get; private set; }
        public CommandKey CommandKey { get; private set; }
        public AbstractCircuitBreaker CircuitBreaker { get; private set; }
        public AbstractBulkhead Bulkhead { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProllyCommand{T}"/> class.
        /// Uses the <see cref="Patterns.Timeout.SimpleTimeout"/> as implementation of the <see cref="Patterns.AbstractTimeout"/>
        /// </summary>
        /// <param name="commandGroupkey">The command group.</param>
        public ProllyCommand(CommandGroupKey commandGroupkey)
            : this(commandGroupkey, AbstractTimeout.Factory.Resolve(commandGroupkey))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProllyCommand{T}"/> class.
        /// </summary>
        /// <param name="commandGroupkey">The command group.</param>
        /// <param name="timeout">The timeout.</param>
        public ProllyCommand(CommandGroupKey commandGroupkey, AbstractTimeout timeout)
        {
            CommandGroupKey = commandGroupkey;
            CircuitBreaker = AbstractCircuitBreaker.Factory.Resolve(commandGroupkey);
            Bulkhead = AbstractBulkhead.Factory.Resolve(commandGroupkey);
            _timeout = timeout;
        }

        /// <summary>
        /// Executes the command. This wil call the Run method. The command will not
        /// be execute if the CircuitBreaker is not allowing requests or if there are no
        /// available compartments. 
        /// </summary>
        /// <returns>{T}</returns>
        /// <exception cref="CircuitBreakerOpenException">Cannot execute. The CircuitBreaker is Open.</exception>
        public T Execute()
        {
            try
            {
                Task<T> task = new Task<T>(() => { return Run(); });

                lock ( _lock )
                {
                    if ( !CircuitBreaker.AllowRequests )
                        throw new CircuitBreakerOpenException("Cannot execute. The CircuitBreaker is Open.");

                    if ( !Bulkhead.HasRoom )
                        throw new MaximumAllowedTasksReachedException("Cannot execute. Maximum amount of tasks already reached.");

                    Bulkhead.TaskStarted();
                    task.Start();
                }

                _timeout.Monitor(task);

                CircuitBreaker.MarkSucces();
                return task.Result;
            }
            catch ( AggregateException ex )
            {
                throw ex.InnerException;              
            }
            catch ( Exception ex )
            {
                if (ex is CircuitBreakerIgnoreException)
                {
                    throw ex;
                }

                CircuitBreaker.MarkFailure();

                return TryFallback(ex);
            }
            finally
            {
                Bulkhead.TaskFinished();
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
        /// Executes the command asynchronous. The command will not
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

        private T TryFallback(Exception ex)
        {
            try
            {
                return Fallback();
            }
            catch ( FallbackNotSpecifiedException ) 
            {
                // If fallback is not overwritten 
                // throw original exception
                throw ex;
            }
        }
    }
}
