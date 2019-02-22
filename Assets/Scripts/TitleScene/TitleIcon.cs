using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class TitleIcon : MonoBehaviour
    {
        [SerializeField] Rigidbody m_rb;
        [SerializeField] Vector3 m_torque = Vector3.up;

        private void Awake()
        {
            Debug.Log("awake 直下");
            var touchGesture = TouchGestureDetector.Instance;
            touchGesture.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.TouchBegin:
                    case TouchGestureDetector.Gesture.TouchMove:
                    case TouchGestureDetector.Gesture.TouchStationary:
                    case TouchGestureDetector.Gesture.TouchEnd:
                    case TouchGestureDetector.Gesture.Click:
                    case TouchGestureDetector.Gesture.FlickTopToBottom:
                    case TouchGestureDetector.Gesture.FlickBottomToTop:
                    case TouchGestureDetector.Gesture.FlickLeftToRight:
                    case TouchGestureDetector.Gesture.FlickRightToLeft:
                        Debug.Log("aaaaaaaa");
                        break;
                    default:
                        break;
                }



                //    Debug.Log("awake tgd");

                //    GameObject go;
                //    if (touchInfo.HitDetection(out go))
                //    {
                //        if(go.tag == "GravityBall")
                //        {
                //            m_rb.AddTorque(m_torque, ForceMode.Impulse);
                //        }
                //}
            });
        }

    }
}