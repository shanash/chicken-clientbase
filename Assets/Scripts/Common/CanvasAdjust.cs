using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    namespace Component
    {
        [ExecuteInEditMode]
        public class CanvasAdjust : MonoBehaviour
        {
            private void Awake()
            {
                CanvasScaler scaler = GetComponent<CanvasScaler>();
                Rect rect = GetComponent<RectTransform>().rect;
                float screenRatio = scaler.referenceResolution.y / scaler.referenceResolution.x;
                scaler.matchWidthOrHeight = ((rect.height / rect.width) < screenRatio) ? 1 : 0;
            }
        }
    }
}
