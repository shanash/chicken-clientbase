using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface IBackEventReceiver
    {
        void ReceiveBackButton();
    }

    public class BackEventManager : UnitySingleton<BackEventManager>
    {
        private Stack<IBackEventReceiver> _receivers = new Stack<IBackEventReceiver>();

        public void Reboot()
        {
            _receivers.Clear();
        }

        public void Register(IBackEventReceiver receiver)
        {
            _receivers.Push(receiver);
        }

        public void UnRegister(IBackEventReceiver receiver)
        {
            if (_receivers.Count <= 0)
            {
                return;
            }
            _receivers.Pop();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_receivers.Count > 0)
                {
                    Notify();
                }
            }
        }

        private void Notify()
        {
            IBackEventReceiver receiver = _receivers.Peek();
            receiver.ReceiveBackButton();
        }

        protected override void OnDestroy()
        {
            _receivers.Clear();
        }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}
