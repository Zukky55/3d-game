using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class FieldmapController : MonoBehaviour
    {
        public Vector2 InputDeltaPosition
        {
            get
            {
                var vec = Input.GetTouch(0).deltaPosition;
                var buffer = vec.x;
                vec.x = vec.y;
                vec.y = -buffer;
                return vec;
            }
        }

        [Header("Components")]
        TouchGestureDetector m_touchGestureDetector;
        Rigidbody m_rb;

        [Header("Parameters")]
        [SerializeField] float m_intensity = 90f;
        [SerializeField] float m_interpolateRatio = 0.1f;

        private void OnEnable()
        {
            m_touchGestureDetector = TouchGestureDetector.Instance;
            m_rb = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchMove)
                {
                    var targetRot = Quaternion.AngleAxis(m_intensity, InputDeltaPosition);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_interpolateRatio);
                }
            });
        }

        private void Update()
        {
        }
    }
}

