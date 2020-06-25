using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Financial.Events
{
    /// <summary>
    /// Central event dispatcher used to send application messages to registered handlers
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// Initializes a new instance of the EventAggregator class.
        /// </summary>
        public EventAggregator()
        {
            mSynchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Send a message instance for immediate delivery
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="message">Message to send</param>
        public void SendMessage<T>(T message)
        {
            if (message == null)
            {
                return;
            }

            if (mSynchronizationContext != null)
            {
                mSynchronizationContext.Send(
                    m => Dispatch<T>((T)m),
                    message);
            }
            else
            {
                Dispatch(message);
            }
        }

        /// <summary>
        /// Post a message instance for asynchronous delivery
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="message">Message to send</param>
        public void PostMessage<T>(T message)
        {
            if (message == null)
            {
                return;
            }

            if (mSynchronizationContext != null)
            {
                mSynchronizationContext.Post(
                    m => Dispatch<T>((T)m),
                    message);
            }
            else
            {
                Dispatch(message);
            }
        }

        /// <summary>
        /// Register a message handler
        /// </summary>
        /// <param name="eventHandler">Message handler to add.</param>
        public Action<T> RegisterHandler<T>(Action<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            mHandlers.Add(eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// Unregister a message handler
        /// </summary>
        /// <param name="eventHandler">Message handler to remove.</param>
        public void UnregisterHandler<T>(Action<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            mHandlers.Remove(eventHandler);
        }

        /// <summary>
        /// Dispatch a message to all appropriate handlers
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="message">Message to dispatch to registered handlers</param>
        private void Dispatch<T>(T message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var compatibleHandlers
                = mHandlers.OfType<Action<T>>()
                    .ToList();
            foreach (var h in compatibleHandlers)
            {
                h(message);
            }
        }

        /// <summary>
        /// Storage for all our registered handlers
        /// </summary>
        private readonly List<Delegate> mHandlers = new List<Delegate>();

        /// <summary>
        /// SynchronizationContext used to transition to the correct thread
        /// </summary>
        private readonly SynchronizationContext mSynchronizationContext;
    }
}
