using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Indicator : UnitySingleton<Indicator>
    {
        private GameObject _indicator = null;

        public void Show(bool active)
        {
            _indicator.SetActive(active);
        }

        private GameObject MakeIndicator()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Commons/Indicator") as GameObject;
            GameObject go = Instantiate<GameObject>(prefab) as GameObject;
            go.transform.SetParent(null);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            return go;
        }

        protected override void Awake()
        {
            _indicator = MakeIndicator();
            DontDestroyOnLoad(_indicator);
        }
    }
}
