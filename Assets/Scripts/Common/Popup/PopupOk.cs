using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class PopupOk : MonoBehaviour, IPopupWindow, IBackEventReceiver
    {
        [SerializeField] private Text title_ = null;
        [SerializeField] private Text message_ = null;
        [SerializeField] private Text okButtonText_ = null;
        [SerializeField] private Button buttonOk_ = null;
        private Action _okAction = null;

        public void ReceiveBackButton()
        {
            OnClick(buttonOk_.gameObject);
        }

        public bool Open(string title, string message, string okButtonText, Action okAction)
        {
            this.title_.text = title;
            this.message_.text = message;
            this.okButtonText_.text = okButtonText;
            this._okAction = okAction;
            PopupManager.Instance.Push(this);
            return true;
        }

        public bool Show()
        {
            if (this.gameObject == null) return false;

            Debug.Assert(!this.gameObject.activeSelf);
            this.gameObject.SetActive(true);
            return this.gameObject.activeSelf;
        }

        public void Close()
        {
            Destroy(this.gameObject);
        }

        public void OnClick(GameObject button)
        {
            switch (button.name)
            {
                case "Button-Ok":
                    PopupManager.Instance.Close();
                    if (_okAction != null)
                    {
                        _okAction();
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            SoundManager.Instance.PlayFx("button");
        }

        private void OnEnable()
        {
            BackEventManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            BackEventManager.Instance.UnRegister(this);
        }
    }
}
