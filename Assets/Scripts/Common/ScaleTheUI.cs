using UnityEngine;
using System.Collections;

namespace Common
{
    namespace Component
    {
        public class ScaleTheUI : MonoBehaviour
        {
            [SerializeField] private RectTransform RT_ = null;
            [SerializeField] private Vector3 targetScale_ = Vector2.zero;
            [SerializeField] private float time_ = 0.0f;
            [SerializeField] private bool playOnStart_ = false;

            private IEnumerator _coroutine = null;

            private void Start()
            {
                if (playOnStart_)
                {
                    ScaleTo();
                }
            }

            public void ScaleTo(UnityEngine.Events.UnityAction action = null)
            {
                ScaleTo(RT_, targetScale_, time_, action);
            }

            public void ScaleTo(RectTransform rt, Vector3 targetScale, float time, UnityEngine.Events.UnityAction action = null)
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }
                _coroutine = ScaleToTarget(rt, targetScale, time, action);
                StartCoroutine(_coroutine);
            }

            private IEnumerator ScaleToTarget(RectTransform rt, Vector3 targetScale, float time, UnityEngine.Events.UnityAction action)
            {
                float scaleDistance = Vector2.Distance(targetScale, rt.localScale);
                float speed = scaleDistance / time;
                float step = 0.0f;
                while (scaleDistance > 0.01f)
                {
                    step = speed * Time.deltaTime;
                    rt.localScale = Vector2.MoveTowards(rt.localScale, targetScale, step);
                    yield return null;
                    scaleDistance = Vector2.Distance(targetScale, rt.localScale);
                }
                rt.localScale = targetScale;
                if (action != null)
                {
                    action();
                }
            }
        }
    }
}
