﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoSingleton<PlayerController>
    {
        #region Property
        /// <summary>Input direction. コントローラーから入力されたベクトル</summary>
        public Vector3 InputDir
        {
            get
            {
                return new Vector3(m_horizontal, 0, m_vertical);
            }
        }

        /// <summary>Jump可能かどうか</summary>
        public bool IsPossibleToJump
        {
            get
            {
                if (m_isGrounded)
                {
                    m_jumpCounter = m_possibleCountOfJump;
                }
                var result = m_jumpCounter > 0;
                m_jumpCounter--;
                return result;
            }
        }

        /// <summary> InverseGravityが可能かどうか</summary>
        public bool IsPossibleToInverseGravity
        {
            get
            {
                return m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame && m_possibleCountOfGravityInverse > 0;
            }
        }

        public int GravityInverseCount
        {
            get
            {
                return m_possibleCountOfGravityInverse;
            }
            set
            {
                m_possibleCountOfGravityInverse = value;
            }
        }

        public bool IsGrounded { get { return m_isGrounded; } }

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
        /// <summary>Jump出来る回数</summary>
        [SerializeField] int m_possibleCountOfJump;
        /// <summary>空中にいる時の落下速度</summary>
        [SerializeField] float m_fallingVelocity = 1f;
        /// <summary>Gravity inverse出来る回数</summary>
        [SerializeField] int m_possibleCountOfGravityInverse = 10;

        /// <summary>IsPossbleToJumpのバッキングフィールド</summary>
        int m_jumpCounter;
        /// <summary>縦軸入力</summary>
        float m_horizontal;
        /// <summary>横軸入力</summary>
        float m_vertical;
        /// <summary>地面に着いているかどうか</summary>
        bool m_isGrounded;
        /// <summary>同じColliderに接触している時間</summary>
        float m_collisionStaySeconds;
        GravityController.GravitySource m_targetGravitySource = GravityController.GravitySource.Down;


        /// <summary>操作の標準とする向き(基本メインカメラ)</summary>
        [Header("Components")]
        [SerializeField] Transform m_directionalStandard;
        /// <summary>浮動ジョイスティック</summary>
        [SerializeField] FloatingJoystick m_FJoyStick;
        /// <summary>dungeon generator</summary>
        [SerializeField] DungeonGenerator m_dungeonGenerator;
        /// <summary>audio source</summary>
        [SerializeField] AudioSource m_audioSource;
        /// <summary>jump sound effect</summary>
        [SerializeField] AudioClip m_jumpSE;
        /// <summary>アタッチされているRigidbody</summary>
        Rigidbody m_rb;
        /// <summary>同じオブジェクトに追加された Animator への参照</summary>
        Animator m_anim;
        /// <summary>重力操作クラス</summary>
        GravityController m_gravityController;
        /// <summary>State machine</summary>
        StateManager m_stateManager;

        #endregion

        private void OnEnable()
        {
            m_rb = GetComponent<Rigidbody>();
            m_anim = GetComponent<Animator>();
            m_gravityController = GravityController.Instance;
            m_stateManager = StateManager.Instance;
            GravityController.OnChangeGravity += SyncGravitySource;
        }

        private void OnDisable()
        {
            GravityController.OnChangeGravity -= SyncGravitySource;
        }

        /// <summary>
        /// Called by machine dependent FPS
        /// </summary>
        private void Update()
        {
            //方向の入力を取得する
            m_horizontal = (Input.GetAxisRaw("Horizontal") == 0f) ? m_FJoyStick.Horizontal : Input.GetAxisRaw("Horizontal"); //方向キーの入力が無い時はジョイスティックから入力をとる
            m_vertical = (Input.GetAxisRaw("Vertical") == 0f) ? m_FJoyStick.Vertical : Input.GetAxisRaw("Vertical"); // 方向キーの入力が無い時はジョイスティックから入力をとる

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            FallingAddForce();
        }

        /// <summary>
        /// Called with fixed 50 FPS.
        /// </summary>
        private void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// When touching the floor.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Cell")
            {
                m_isGrounded = true;
                m_anim.SetBool(AnimParameter.IsGrounded.ToString(), true);
            }
        }

        /// <summary>
        /// while touching the floor.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Cell")
            {
                m_isGrounded = true;
                m_anim.SetBool(AnimParameter.IsGrounded.ToString(), true);
            }
        }

        /// <summary>
        /// When leaving the floor.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Cell")
            {
                m_isGrounded = false;
                m_anim.SetBool(AnimParameter.IsGrounded.ToString(), false);
            }
        }

        /// <summary>
        /// 入力に対する移動処理.
        /// </summary>
        private void Move()
        {
            // x-z 平面(地面と平行)の速度を求める
            var dir = InputDir; // 方向の入力で、x-z平面の移動方向が決まる。

            if (dir == Vector3.zero) // 移動入力されていない時
            {
                m_anim.SetFloat(AnimParameter.Speed.ToString(), 0f); //Idleへ遷移させる
                if (m_anim.GetBool(AnimParameter.IsGrounded.ToString())) // 足が何かの面に着いている時は入力が無くなったら即時playerの動きを止める
                {
                    m_rb.velocity = Vector3.zero;
                }
                return;
            }

            dir = m_directionalStandard.TransformDirection(dir);//カメラに対して正面の向きに変換する
            var targetVec = Vector3.Slerp(transform.forward, dir, m_turnInterpolateAmount);
            switch (m_gravityController.CurrentGravitySource) // 向いている方角に対して頭上方向に向きを固定させる
            {
                case GravityController.GravitySource.Forward:
                case GravityController.GravitySource.Back:
                    targetVec.z = 0;
                    break;
                case GravityController.GravitySource.Right:
                case GravityController.GravitySource.Left:
                    targetVec.x = 0;
                    break;
                case GravityController.GravitySource.Up:
                case GravityController.GravitySource.Down:
                    targetVec.y = 0;
                    break;
                case GravityController.GravitySource.Other:
                default:
                    break;
            }

            if (targetVec != Vector3.zero) // 向いている方向と入力されている方向ベクトルが一緒でない時
            {
                var rot = Quaternion.LookRotation(targetVec, transform.up); // 入力ベクトルがさす方向へキャラクターを回転
                transform.rotation = rot;
            }
            m_anim.SetFloat(AnimParameter.Speed.ToString(), dir.sqrMagnitude); // Walk or Run へ遷移

            // 地面に着いている時はvelocity,そうでない時(空中を想定)はAddForceで移動させる
            if (m_anim.GetBool(AnimParameter.IsGrounded.ToString()))
            {
                m_rb.velocity = targetVec * m_moveSpeed;
            }
            else
            {
                m_rb.AddForce(targetVec * m_moveSpeed, ForceMode.Force);
            }
        }

        /// <summary>
        /// キャラの頭上の向きを重力方向と逆方向へ向ける
        /// </summary>
        void SyncGravitySource()
        {
            transform.up = -Physics.gravity;
        }

        /// <summary>
        /// ジャンプ処理
        /// </summary>
        public void Jump()
        {
            if (IsPossibleToJump && m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                m_rb.AddForce(-Physics.gravity * m_jumpPower, ForceMode.Impulse);
                m_audioSource.PlayOneShot(m_jumpSE);
            }
        }

        /// <summary>
        /// 重力を逆転させる
        /// </summary>
        public void InverseGravity()
        {
            if (!IsPossibleToInverseGravity)
            {
                return;
            }

            switch (m_targetGravitySource)
            {
                case GravityController.GravitySource.Up:
                    m_targetGravitySource = GravityController.GravitySource.Down;
                    break;
                case GravityController.GravitySource.Down:
                    m_targetGravitySource = GravityController.GravitySource.Up;
                    break;
                default:
                    break;
            }
            m_gravityController.ChangeGravitySource(m_targetGravitySource);
            m_possibleCountOfGravityInverse--;
        }

        /// <summary>
        /// 空中にいる時落下加速度を追加する
        /// </summary>
        void FallingAddForce()
        {
            if (m_isGrounded)
            {
                return;
            }
            m_rb.AddForce(Physics.gravity * m_fallingVelocity);
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

