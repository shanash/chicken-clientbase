using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class JsonHelper
    {
        #region Serialization
        [Serializable]
        class Serialization<T>
        {
            [SerializeField] List<T> target;
            public List<T> ToList() { return target; }
            public T[] ToArray() { return target.ToArray(); }

            public Serialization(List<T> target)
            {
                this.target = target;
            }

            public Serialization(T[] array)
            {
                this.target = new List<T>(array);
            }
        }

        [Serializable]
        class Serialization<TKey, TValue> : ISerializationCallbackReceiver
        {
            [SerializeField] List<TKey> keys;
            [SerializeField] List<TValue> values;

            Dictionary<TKey, TValue> target;
            public Dictionary<TKey, TValue> ToDictionary() { return target; }

            public Serialization(Dictionary<TKey, TValue> target)
            {
                this.target = target;
            }

            public void OnBeforeSerialize()
            {
                keys = new List<TKey>(target.Keys);
                values = new List<TValue>(target.Values);
            }

            public void OnAfterDeserialize()
            {
                var count = Math.Min(keys.Count, values.Count);
                target = new Dictionary<TKey, TValue>(count);
                for (var i = 0; i < count; ++i)
                {
                    target.Add(keys[i], values[i]);
                }
            }
        }
        #endregion

        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static T[] ArrayFromJson<T>(string json)
        {
            string arrayJson = string.Format("{{ \"target\": {0} }}", json);
            return JsonUtility.FromJson<Serialization<T>>(arrayJson).ToArray();
        }

        public static List<T> ListFromJson<T>(string json)
        {
            return JsonUtility.FromJson<Serialization<T>>(json).ToList();
        }

        public static Dictionary<TKey, TValue> DicFromJson<TKey, TValue>(string json)
        {
            return JsonUtility.FromJson<Serialization<TKey, TValue>>(json).ToDictionary();
        }

        public static string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static string ToJson<T>(T obj)
        {
            return JsonUtility.ToJson(obj as object);
        }

        public static string ListToJson<T>(List<T> list)
        {
            return JsonUtility.ToJson(new Serialization<T>(list));
        }

        public static string DicToJson<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            return JsonUtility.ToJson(new Serialization<TKey, TValue>(dic));
        }
    }
}
