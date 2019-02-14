using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class StateToggle : MonoBehaviour
    {
        [SerializeField] StateManager.StateMachine.State m_state = StateManager.StateMachine.State.InTheGame;

        public void StateSwitch()
        {
            StateManager.Instance.TransitionState(m_state);
            gameObject.SetActive(false);
        }
    }
}