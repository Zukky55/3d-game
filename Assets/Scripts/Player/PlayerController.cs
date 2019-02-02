using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        #region Properties
        /// <summary>Input direction. コントローラーから入力されたベクトル</summary>
        public Vector3 InputDir { get { return new Vector3(m_horizontal, 0, m_vertical); } }
        #endregion

        #region Field
        /// <summary>移動速度を調整するパラメータ</summary>
        [Header("Parameters")]
        [SerializeField] protected float m_moveSpeed = 5f;
        /// <summary>プレイヤーが回転する速度</summary>
        [SerializeField] float m_turnSpeed = 8f;
        /// <summary>身体の正面をなめらかに入力軸方向に向ける為の内挿量</summary>
        [SerializeField] float m_turnInterpolateAmount = 1f;
        /// <summary>ジャンプ力を調整するパラメータ</summary>
        [SerializeField] float m_jumpPower = 10f;
        // 重力加速度を調整するパラメーター
        [SerializeField] float m_gravityMultiplier = 1f;
        /// <summary>縦軸入力</summary>
        float m_horizontal;
        /// <summary>横軸入力</summary>
        float m_vertical;
        /// <summary>地面に着いているかどうか</summary>
        bool m_isGrounded;

        /// <summary>操作の標準とする向き(基本メインカメラ)</summary>
        [Header("Components")]
        [SerializeField] Transform m_directionalStandard;
        /// <summary>アタッチされているRigidbody</summary>
        Rigidbody m_rb;
        /// <summary>浮動ジョイスティック</summary>
        [SerializeField] FloatingJoystick m_FJoyStick;
        /// <summary>同じオブジェクトに追加された Animator への参照</summary>
        private Animator m_anim;
        #endregion

        private void OnEnable()
        {
            m_rb = GetComponent<Rigidbody>();
            m_anim = GetComponent<Animator>();
        }

        private void Update()
        {
            //方向の入力を取得する
            m_horizontal = (Input.GetAxis("Horizontal") == 0f) ? m_FJoyStick.Horizontal : Input.GetAxis("Horizontal"); //方向キーの入力が無い時はジョイスティックから入力をとる
            m_vertical = (Input.GetAxis("Vertical") == 0f) ? m_FJoyStick.Vertical : Input.GetAxis("Vertical"); // 方向キーの入力が無い時はジョイスティックから入力をとる
        }

        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void OnTriggerEnter(Collider other)
        {
            m_isGrounded = true;
            m_anim.SetBool(AnimParameter.IsGrounded.ToString(), true);
        }

        private void OnTriggerStay(Collider other)
        {
            m_isGrounded = true;
            m_anim.SetBool(AnimParameter.IsGrounded.ToString(), true);
        }

        private void OnTriggerExit(Collider other)
        {
            m_isGrounded = false;
            m_anim.SetBool(AnimParameter.IsGrounded.ToString(), false);
        }
        /// <summary>
        /// 入力に対する移動処理.
        /// </summary>
        private void Move()
        {
            // x-z 平面(地面と平行)の速度を求める
            var dir = InputDir; // 方向の入力で、x-z平面の移動方向が決まる。

            // 入力に合わせてアニメーションを制御する
            if (dir != Vector3.zero) // 移動している時
            {
                dir = m_directionalStandard.TransformDirection(dir);//カメラに対して正面の向きに変換する
                dir.y = 0;
                transform.forward = Vector3.Slerp(transform.forward, dir, m_turnInterpolateAmount); // 入力された向きに対して少し遅延しながら入力方向に向かせる
                m_rb.velocity = dir * m_moveSpeed;
                m_anim.SetFloat(AnimParameter.Speed.ToString(), dir.sqrMagnitude); // Walk or Run へ遷移
            }
            else // 移動していない時
            {
                m_anim.SetFloat(AnimParameter.Speed.ToString(), 0f); //Idleへ遷移させる
                if (m_anim.GetBool(AnimParameter.IsGrounded.ToString())) // 足が何かの面に着いている時は入力が無くなったら即時playerの動きを止める
                {
                    //m_rb.velocity = Vector3.zero;
                }
            }
        }

        /// <summary>
        /// 入力に合わせてPlayerをジャンプさせる
        /// </summary>
        void Jump()
        {

        }

        /// <summary>
        /// 入力に合わせて回転を加える
        /// </summary>
        void Turn()
        {
            Quaternion yAxisRot = Quaternion.AngleAxis(m_horizontal * m_turnSpeed * Time.deltaTime, transform.up);
            transform.rotation = yAxisRot * transform.rotation;  // 元の回転値と合成して上書き
        }



        /// <summary>
        /// animator state machineのステート一覧
        /// </summary>
        public enum AnimState
        {
            Idle,
            Walking,
            Running,
            Jumping,
        }

        /// <summary>
        /// animator controllerのパラメータ一覧
        /// </summary>
        public enum AnimParameter
        {
            Speed,
            Attack,
            IsRunning,
            IsGrounded,
            DebugTrigger,
        }
    }
}

