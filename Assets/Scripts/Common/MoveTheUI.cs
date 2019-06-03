using UnityEngine;
using System.Collections;

namespace Common
{
    namespace Component
    {
        public class MoveTheUI : MonoBehaviour
        {
            [SerializeField] private RectTransform RT_ = null;
            [SerializeField] private Vector2 targetPosition_ = Vector2.zero;
            [SerializeField] private float time_ = 0.0f;
            [SerializeField] private bool playOnStart_ = false;

            private IEnumerator _coroutine = null;

            private void Start()
            {
                if (playOnStart_)
                {
                    MoveTo();
                }
            }

            public void MoveTo(UnityEngine.Events.UnityAction action = null)
            {
                MoveTo(RT_, targetPosition_, time_, action);
            }

            public void MoveTo(RectTransform rt, Vector2 targetPos, float time, UnityEngine.Events.UnityAction action = null)
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }
                _coroutine = MoveToTarget(rt, targetPos, time, action);
                StartCoroutine(_coroutine);
            }

            private IEnumerator MoveToTarget(RectTransform rt, Vector2 targetPos, float time, UnityEngine.Events.UnityAction action)
            {
                float distance = Vector2.Distance(targetPos, rt.anchoredPosition);
                float speed = distance / time;
                float step = 0.0f;
                while (distance > 0.01f)
                {
                    step = speed * Time.deltaTime;
                    rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, targetPos, step);
                    yield return null;
                    distance = Vector2.Distance(targetPos, rt.anchoredPosition);
                }
                rt.anchoredPosition = targetPos;
                if (action != null)
                {
                    action();
                }
            }
        }
    }
}