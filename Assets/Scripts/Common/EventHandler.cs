using System;
using System.Collections.Generic;

namespace Common
{
    public class EventHandler<Type>
    {
        public delegate void Action(object sender, object[] args);

        private Dictionary<Type, List<Action>> _actions = new Dictionary<Type, List<Action>>();
        private Queue<Event> _eventQueue = new Queue<Event>();

        private class Event
        {
            public Type type = default(Type);
            public object sender = null;
            public object[] args = null;

            public Event(Type type, object sender, object[] args)
            {
                this.type = type;
                this.sender = sender;
                this.args = args;
            }
        }

        public void Register(Type type, Action action)
        {
            if (!_actions.ContainsKey(type))
            {
                _actions.Add(type, new List<Action>());
            }

            _actions[type].Add(action);
        }

        public void UnRegister(Type type, Action action)
        {
            if (_actions.ContainsKey(type))
            {
                _actions[type].Remove(action);
            }
        }

        public void Send(Type type, object sender, params object[] args)
        {
            _eventQueue.Enqueue(new Event(type, sender, args));
        }

        public void Produceed()
        {
            while (_eventQueue.Count > 0)
            {
                Event e = _eventQueue.Dequeue();

                if (_actions.ContainsKey(e.type))
                {
                    _actions[e.type].ForEach((Action action) =>
                    {
                        action(e.sender, e.args);
                    });
                }
            }
        }
    }
}
