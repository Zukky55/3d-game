using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class SpherePlayerCamera : MonoBehaviour
    {

        /// <summary>カメラの孫オブジェクト。カメラコンポーネントを持つ。視界オフセット座標、回転を管理する</summary>
        [Header("Components")]
        Transform m_camera;
        [SerializeField] SpherePlayerController m_playerCtrl;

        /// <summary>注視点(CameraParentの座標)</summary>
        [Header("Parameters")]
        [SerializeField] Transform m_lookTarget;
        /// <summary>カメラの座標移動を滑らかにするパラメータ</summary>
        [SerializeField] float m_positionInterpolate = 1f;
        /// <summary>カメラの回り込みを滑らかにするパラメータ</summary>
        [SerializeField] float m_rotateInterpolate = 0.05f;
        Vector3 m_previousPosition;

        private void OnEnable()
        {
            m_camera = transform.GetChild(0); // カメラ自身
            if (m_playerCtrl == null)
            {
                m_playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<SpherePlayerController>();
            }
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, m_lookTarget.position, m_positionInterpolate);
            if (m_playerCtrl.InputDir != Vector3.zero)
            {
                var dir = transform.TransformDirection(m_playerCtrl.InputDir);
                var targetRot = Quaternion.LookRotation(dir, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_rotateInterpolate);
            }
        }
    }
}
