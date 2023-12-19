using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.Event
{
    public interface IEventBus
    {
        bool RegisterEventHandler<TEventArgs>(IEventHandler<TEventArgs> handler);
        bool UnregisterEventHandler<TEventArgs>(IEventHandler<TEventArgs> handler);
        void RaiseEvent<TEventArgs>(object sender, TEventArgs args);
        void ClearEventHandlers<TEventArgs>();
        void ClearAllEventHandlers();
    }

    public class DefaultEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<IEventHandler>> _handleTable = new();


        public bool RegisterEventHandler<TEventArgs>(IEventHandler<TEventArgs> handler)
        {
            List<IEventHandler> handlers = GetEventHandlers<TEventArgs>(true);
            if (handlers.Contains(handler))
            {
                return false;
            }

            handlers.Add(handler);
            return true;
        }

        public bool UnregisterEventHandler<TEventArgs>(IEventHandler<TEventArgs> handler)
        {
            List<IEventHandler> handlers = GetEventHandlers<TEventArgs>(false);
            if (handlers == null)
            {
                return false;
            }

            return handlers.Remove(handler);
        }

        public void RaiseEvent<TEventArgs>(object sender, TEventArgs args)
        {
            List<IEventHandler> handlers = GetEventHandlers<TEventArgs>(false);
            if (handlers == null)
            {
                return;
            }

            foreach (IEventHandler handler in handlers)
            {
                ((IEventHandler<TEventArgs>)handler).HandleEvent(sender, args);
            }
        }

        public void ClearEventHandlers<TEventArgs>()
        {
            List<IEventHandler> handlers = GetEventHandlers<TEventArgs>(false);
            if (handlers != null)
            {
                handlers.Clear();
            }
        }

        public void ClearAllEventHandlers()
        {
            _handleTable.Clear();
        }

        private List<IEventHandler> GetEventHandlers<T>(bool autoCreateNew)
        {
            if (_handleTable.TryGetValue(typeof(T), out List<IEventHandler> handlers))
            {
                return handlers;
            }

            if (autoCreateNew)
            {
                handlers = new List<IEventHandler>();
                _handleTable.Add(typeof(T), handlers);
            }

            return handlers;
        }
    }
}
