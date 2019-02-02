using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    /// <summary>
    /// 重力操作を行う
    /// </summary>
    public class GravityController : MonoBehaviour
    {
        /// <summary>重力加速度</summary>
        [SerializeField] float m_gravitationalAcceleration = 9.18f;

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
                }
            }
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
                    break;
                case GravitySource.Back:
                    targetSource = Vector3.back * m_gravitationalAcceleration;
                    break;
                case GravitySource.Right:
                    targetSource = Vector3.right * m_gravitationalAcceleration;
                    break;
                case GravitySource.Left:
                    targetSource = Vector3.left * m_gravitationalAcceleration;
                    break;
                case GravitySource.Up:
                    targetSource = Vector3.up * m_gravitationalAcceleration;
                    break;
                case GravitySource.Down:
                    targetSource = Vector3.down * m_gravitationalAcceleration;
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
        }
    }
}