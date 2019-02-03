using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    /// <summary>
    /// 重力操作を行う
    /// </summary>
    public class GravityController : MonoSingleton<GravityController>
    {
        #region Properties
        /// <summary>現在の重力源</summary>
        public GravitySource m_CurrentGravitySource { get; private set; }
        #endregion

        #region Fields
        /// <summary>重力加速度</summary>
        [SerializeField] float m_gravitationalAcceleration = 9.18f;

        #endregion

        #region Methods
        private void OnEnable()
        {
            m_CurrentGravitySource = GravitySource.Down;
        }

        /// <summary>
        /// タッチした箇所にRayを飛ばし、重力源対象オブジェクトが返ってきた場合そのオブジェクトを重力源にする
        /// </summary>
        /// <param name="searhCoordinate"></param>
        public void ChangeGravitySource(Vector3 searhCoordinate)
        {
            Ray ray = Camera.main.ScreenPointToRay(searhCoordinate);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100f))
            {
                GameObject go = hit.collider.gameObject;
                if (go.tag == "GravitySource")
                {
                    Physics.gravity = go.transform.position;
                    m_CurrentGravitySource = GravitySource.Other;
                }
            }
        }

        /// <summary>
        /// 指定したTransform対して指定した相対的な方角へ重力を働かせる
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        public void ChangeGravitySource(Transform target, GravitySource direction)
        {
            Vector3 targetSource;
            switch (direction)
            {
                case GravitySource.Forward:
                    targetSource = target.forward * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Forward;
                    break;
                case GravitySource.Back:
                    targetSource = -target.forward * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Back;
                    break;
                case GravitySource.Right:
                    targetSource = target.right * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Right;
                    break;
                case GravitySource.Left:
                    targetSource = -target.right * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Left;
                    break;
                case GravitySource.Up:
                    targetSource = target.up * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Up;
                    break;
                case GravitySource.Down:
                    targetSource = -target.up * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Down;
                    break;
                default:
                    throw new System.ArgumentException("Invalid argument");
            }
            Physics.gravity = targetSource;
            m_CurrentGravitySource = GravitySource.Other;
        }

        /// <summary>
        /// 指定された方向に重力原を変更する
        /// </summary>
        /// <param name="direction"></param>
        public void ChangeGravitySource(GravitySource direction)
        {
            Vector3 targetSource;
            switch (direction)
            {
                case GravitySource.Forward:
                    targetSource = Vector3.forward * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Forward;
                    break;
                case GravitySource.Back:
                    targetSource = Vector3.back * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Back;
                    break;
                case GravitySource.Right:
                    targetSource = Vector3.right * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Right;
                    break;
                case GravitySource.Left:
                    targetSource = Vector3.left * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Left;
                    break;
                case GravitySource.Up:
                    targetSource = Vector3.up * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Up;
                    break;
                case GravitySource.Down:
                    targetSource = Vector3.down * m_gravitationalAcceleration;
                    m_CurrentGravitySource = GravitySource.Down;
                    break;
                default:
                    throw new System.ArgumentException("Invalid argument");
            }
            Physics.gravity = targetSource;
        }

        /// <summary>
        /// 指定されたGameObjectのPivot座標を重力原にする
        /// </summary>
        /// <param name="gravitySourceObject"></param>
        public void ChangeGravitySource(GameObject gravitySourceObject)
        {
            Physics.gravity = gravitySourceObject.transform.position;
        }

        /// <summary>
        /// 重力源を初期化する
        /// </summary>
        public void ResetGravitySource()
        {
            Physics.gravity = Vector3.down * m_gravitationalAcceleration;
        }
        #endregion

        #region Enum
        /// <summary>
        /// Gravity source.
        /// </summary>
        public enum GravitySource
        {
            Forward,
            Back,
            Right,
            Left,
            Up,
            Down,
            Other,
        }
        #endregion
    }
}