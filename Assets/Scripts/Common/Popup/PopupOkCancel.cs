using System;
using UnityEngine;
using UnityEngine.UI;


namespace Common
{
    public class PopupOkCancel : MonoBehaviour, IPopupWindow, IBackEventReceiver
    {
        [SerializeField] protected Text title_ = null;
        [SerializeField] protected Text message_ = null;
        [SerializeField] private Text okButtonText_ = null;
        [SerializeField] private Text cancelButtonText_ = null;

        [SerializeField] private Button buttonOK_ = null;
        [SerializeField] private Button buttonCancel_ = null;

        protected Action _okAction = null;
        protected Action _cancelAction = null;

        public virtual void Open(string title, string message, string okButtonText, string cancelButtonText, Action okAction, Action cancelAction)
        {
            this.title_.text = title;
            this.message_.text = message;
            this.okButtonText_.text = okButtonText;
            this.cancelButtonText_.text = cancelButtonText;
            this._okAction = okAction;
            this._cancelAction = cancelAction;
            PopupManager.Instance.Push(this);
        }

        public void ReceiveBackButton()
        {
            this.OnClick(buttonCancel_.gameObject);
        }

        public bool Show()
        {
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

                case "Button-Cancel":
                    PopupManager.Instance.Close();
                    if (_cancelAction != null)
                    {
                        _cancelAction();
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