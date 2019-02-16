using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class PlayerCamera : MonoBehaviour
    {
        /// <summary>カメラの親オブジェクト。注視点とその点への回り込み角度を管理する</summary>
        private Transform m_cameraParent;
        /// <summary>カメラの子オブジェクト。注視点からの距離を管理する</summary>
        private Transform m_cameraChild;
        /// <summary>カメラの孫オブジェクト。カメラコンポーネントを持つ。視界オフセット座標、回転を管理する</summary>
        private Transform m_camera;

        /// <summary>注視点(CameraParentの座標)</summary>
        [SerializeField] Transform m_lookTarget;
        /// <summary>注視点からの距離(CameraChildのローカル座標)</summary>
        [SerializeField] float m_distance;
        /// <summary>視界オフセット座標(MainCameraのローカル座標)</summary>
        [SerializeField] Vector2 m_offsetPosition;
        /// <summary>カメラの座標移動を滑らかにするパラメータ</summary>
        [SerializeField] float m_positionInterpolate = 0.1f;
        /// <summary>カメラの回り込みを滑らかにするパラメータ</summary>
        [SerializeField] float m_turnInterpolate = 0.05f;
        /// <summary>追従するカメラの向きに角度をつけるパラメータ</summary>
        [SerializeField] Vector2 m_offsetEulerAngle;

        private void OnEnable()
        {
            m_cameraParent = transform; // カメラのpivot
            m_cameraChild = m_cameraParent.GetChild(0); // pivotからの距離を管理するオブジェクト
            m_camera = m_cameraChild.GetChild(0); // カメラ自身
        }

        private void Awake()
        {
            if(!m_lookTarget)
            {
                m_lookTarget = GameObject.FindWithTag("Player").transform;
            }
        }

        private void Update()
        {
            m_cameraParent.position = Vector3.Slerp(m_cameraParent.position, m_lookTarget.position, m_positionInterpolate); //カメラの座標をターゲットの座標へ滑らかに動かす
            m_cameraChild.localPosition = new Vector3(0, 0, -m_distance); //pivotからの距離
            var diffRot = m_lookTarget.rotation * Quaternion.Inverse(m_cameraParent.rotation);
            if (diffRot.y != 1f && diffRot.y != -1f) // 完全にplayerとカメラの向きが正反対になった時は動かさない
            {
                m_cameraParent.rotation = Quaternion.Slerp(m_cameraParent.rotation, m_lookTarget.rotation, m_turnInterpolate); //カメラの向きをターゲットの向きへ滑らかに動かす
            }
            m_camera.localRotation = Quaternion.Euler(m_offsetEulerAngle); // ターゲットに追従しているカメラの角度をここで任意にずらす
            m_camera.localPosition = m_offsetPosition; // 追従しているカメラの位置から任意の位置にずらす
        }
    }
}