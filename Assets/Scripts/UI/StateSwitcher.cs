using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    /// <summary>
    /// State 切り替え器
    /// </summary>
    public class StateSwitcher : MonoBehaviour
    {
        [SerializeField] StateManager.StateMachine.State m_state = StateManager.StateMachine.State.InTheGame;

        /// <summary>
        /// StateMachineをInspectorで設定したstateに切り替える
        /// </summary>
        public void StateSwitch()
        {
            StateManager.Instance.TransitionState(m_state);
            gameObject.SetActive(false);
        }
    }
}