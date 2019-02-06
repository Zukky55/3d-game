using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    /// <summary>
    /// Change the gravity source in the direction set by the inspector. インスペクタによって設定された方向に重力源を変更する
    /// </summary>
    public class GravityButton : ButtonBase
    {
        [Header("Componets")]
        GravityController m_gravityController;

        [Header("Parameters")]
        [SerializeField] GravityController.GravitySource m_targetGravitySource;
        

        protected override void OnEnable()
        {
            base.OnEnable();
            m_gravityController = GravityController.Instance;
        }

        protected override void Activate()
        {
            if (m_stateManager.m_StateMachine.m_State != StateManager.StateMachine.State.InTheGame)
            {
                return;
            }

            m_gravityController.ChangeGravitySource(m_targetGravitySource);
        }

    }
}