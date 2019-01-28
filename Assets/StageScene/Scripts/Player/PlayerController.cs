using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Field
    /// <summary>移動速度を調整するパラメータ</summary>
    [Header("Parameters")]
    [SerializeField] protected float m_moveSpeed = 5f;
    /// <summary>プレイヤーが回転する速度</summary>
    [SerializeField] float m_turnSpeed = 8f;
    /// <summary>ジャンプ力を調整するパラメータ</summary>
    [SerializeField] float m_jumpPower = 10f;
    // 重力加速度を調整するパラメーター
    [SerializeField] float m_gravityMultiplier = 1f;

    /// <summary>操作の標準とする向き</summary>
     [Header("Components")]
    [SerializeField] Transform m_directionalStandard;
    /// <summary>浮動ジョイスティック</summary>
    [SerializeField] FloatingJoystick m_FJoyStick;
    /// <summary>同じオブジェクトに追加された Animator への参照</summary>
    private Animator m_anim;
    /// <summary>Animator Controller のステート名</summary>
    //private AnimState m_animState;
    #endregion

}

