using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Mover : MonoBehaviour
    {
        //        /// <summary>
        ///// Movement
        ///// </summary>
        //private void Move()
        //{
        //    //方向の入力を取得する
        //    float h = (Input.GetAxisRaw("Horizontal") == 0f) ? m_FJoyStick.Horizontal : Input.GetAxis("Horizontal"); //方向キーの入力が無い時はジョイスティックから入力をとる
        //    float v = (Input.GetAxis("Vertical") == 0f) ? m_FJoyStick.Vertical : Input.GetAxis("Vertical"); // 方向キーの入力が無い時はジョイスティックから入力をとる
        //    // x-z 平面(地面と平行)の速度を求める
        //    var dir = new Vector3(h, 0, v) * m_moveSpeed; // 方向の入力で、x-z平面の移動方向が決まる。
        //    Debug.Log(dir);
        //    m_rb.velocity = dir * m_moveSpeed;

        //    if (dir != Vector3.zero)
        //    {
                
        //        m_animator.SetFloat(AnimParameter.Speed.ToString(), dir.sqrMagnitude);
        //    }
        //    else // 移動していない時
        //    {
        //        m_animator.SetFloat(AnimParameter.Speed.ToString(), 0f); //Idleへ遷移させる
        //    }
        //}
    }
}