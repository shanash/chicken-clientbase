using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public interface IPopupWindow
    {
        bool Show();
        void Close();
        void OnClick(GameObject Button);
    }

    public class PopupManager : UnitySingleton<PopupManager>
    {
        private const string kPrefabPath = "Prefabs/Commons/";
        private Stack<IPopupWindow> _popupStack = new Stack<IPopupWindow>();
        private bool _isOpened = false;
        private Canvas _canvas = null;
        private GameObject _darkCurtain = null;

        public T Get<T>() where T : IPopupWindow 
        {
            GameObject popup = this.Instantiate(typeof(T).Name);
            T t = popup.GetComponent<T>();
            return t;
        }

        public void Push(IPopupWindow popup)
        {
            _popupStack.Push(popup);
        }

        public void Close()
        {
            if (!_isOpened) return;

            IPopupWindow popup = _popupStack.Pop();
            DarkCurtainActive(false);
            _isOpened = false;
            popup.Close();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            DarkCurtainActive(false);
            _isOpened = false;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DarkCurtainActive(false);
            _isOpened = false;
        }

        private void Update()
        {
            if (_popupStack.Count <= 0 || _isOpened)
            {
                return;
            }

            IPopupWindow data = _popupStack.Peek();
            _isOpened = data.Show();
            DarkCurtainActive(_isOpened);
        }

        private GameObject Instantiate(string prefabName)
        {
            GameObject popup = null;
            GameObject prefab = null;

            prefab = Resources.Load<GameObject>(kPrefabPath + prefabName);
            Debug.AssertFormat(prefab != null, "Popupmanager Not Define Case PopupType : {0}", prefabName);
            popup = Instantiate<GameObject>(prefab);
            popup.transform.SetParent(_canvas.transform);
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localRotation = Quaternion.identity;
            popup.transform.localScale = Vector3.one;
            popup.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            popup.gameObject.SetActive(false);
            return popup;
        }

        protected override void Awake()
        {
            base.Awake();
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Commons/Canvas-Popup");
            GameObject go = Instantiate<GameObject>(prefab) as GameObject;
            go.transform.SetParent(this.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            _canvas = go.GetComponent<Canvas>();
            go.AddComponent<Component.CanvasAdjust>();
            _darkCurtain = _canvas.transform.Find("Image-DarkCurtain").gameObject;
        }

        private void DarkCurtainActive(bool active)
        {
            _darkCurtain.SetActive(active);
        }
    }
}