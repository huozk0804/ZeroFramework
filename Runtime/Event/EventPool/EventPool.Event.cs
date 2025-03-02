//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class Event : IReference
        {
            private object _sender;
            private T _eventArgs;

            public Event()
            {
                _sender = null;
                _eventArgs = null;
            }

            public object Sender => _sender;

            public T EventArgs => _eventArgs;

            public static Event Create(object sender, T e)
            {
                Event eventNode = ReferencePool.Acquire<Event>();
                eventNode._sender = sender;
                eventNode._eventArgs = e;
                return eventNode;
            }

            public void Clear()
            {
                _sender = null;
                _eventArgs = null;
            }
        }
    }
}