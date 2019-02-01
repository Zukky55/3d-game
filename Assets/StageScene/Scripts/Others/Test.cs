using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    [RequireComponent(typeof(Rigidbody))]
    public class Test : MonoBehaviour
    {
        /// <summary>移動速度を調整するパラメータ</summary>
        [Header("Parameters")]
        [SerializeField] protected float m_moveSpeed = 5f;
        /// <summary>プレイヤーが回転する速度</summary>
        [SerializeField] float m_turnSpeed = 1f;
        /// <summary>縦軸入力</summary>
        float m_horizontal;
        /// <summary>横軸入力</summary>
        float m_vertical;

        /// <summary>操作の標準とする向き</summary>
        [Header("Components")]
        [SerializeField] Transform m_directionalStandard;
        /// <summary>アタッチされているRigidbody</summary>
        Rigidbody m_rb;


        private void OnEnable()
        {
            m_rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            //方向の入力を取得する
            m_horizontal = Input.GetAxis("Horizontal");
            m_vertical = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// Movement
        /// </summary>
        private void Move()
        {
            // x-z 平面(地面と平行)の速度を求める
            var dir = new Vector3(m_horizontal, 0, m_vertical); // 方向の入力で、x-z平面の移動方向が決まる。

            // 入力に合わせてアニメーションを制御する
            if (dir != Vector3.zero) // 移動している時
            {
                dir = m_directionalStandard.TransformDirection(dir);//カメラに対して正面の向きに変換する
                dir.y = 0;
                transform.forward = Vector3.Slerp(transform.forward, dir, m_turnSpeed); // 入力された向きに対して少し遅延しながら入力方向に向かせる
                m_rb.velocity = dir * m_moveSpeed;
            }
            else
            {
                m_rb.velocity = Vector3.zero;
            }
        }
    }
}