using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReviewGames
{
    public class PauseButton : MonoBehaviour, IPointerDownHandler
    {
        StateManager m_stateManager;
        [SerializeField] Image m_pauseMenu;
        [SerializeField] FloatingJoystick m_joystick;

        private void Awake()
        {
            m_stateManager = StateManager.Instance;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (m_stateManager.m_StateMachine.m_State)
            {
                case StateManager.StateMachine.State.InTheGame:
                    m_stateManager.TransitionState(StateManager.StateMachine.State.Pause);
                    m_joystick.gameObject.SetActive(false);

                    m_pauseMenu.enabled = true;
                    break;
                case StateManager.StateMachine.State.Pause:
                    m_pauseMenu.enabled = false;
                    m_stateManager.TransitionState(m_stateManager.m_StateMachine.m_PreviousState);
                    m_joystick.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}