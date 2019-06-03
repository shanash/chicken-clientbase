using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance = default(T);

        public static T Instance
        {
            get
            {
                if (_instance == default(T))
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        protected static T self { get { return _instance; } }
    }
}