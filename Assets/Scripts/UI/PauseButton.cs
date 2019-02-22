using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReviewGames
{
    public class PauseButton : ButtonBase
    {
        [SerializeField] GameObject m_pauseMenu;
        [SerializeField] FloatingJoystick m_joystick;

        protected override void Activate()
        {
            switch (m_stateManager.m_StateMachine.m_State)
            {
                case StateManager.StateMachine.State.InTheGame:
                    m_pauseMenu.SetActive(true);
                    m_stateManager.TransitionState(StateManager.StateMachine.State.Pause);
                    m_joystick.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}