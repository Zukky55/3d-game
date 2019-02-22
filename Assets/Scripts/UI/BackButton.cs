using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class BackButton : ButtonBase
    {
        [SerializeField] GameObject m_pauseMenu;
        [SerializeField] FloatingJoystick m_joystick;

        protected override void Activate()
        {
            Debug.Log("back button called.   ");
            m_stateManager.TransitionState(m_stateManager.m_StateMachine.m_PreviousState);
            m_pauseMenu.SetActive(false);
            m_joystick.gameObject.SetActive(true);
        }
    }
}