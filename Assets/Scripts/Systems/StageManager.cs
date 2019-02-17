using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    /// <summary>
    /// Stage manager.
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        StateManager m_stateManager;
        [SerializeField] Timer m_timer;
        [SerializeField] FloatingJoystick m_joystick;

        private void Awake()
        {
            m_stateManager = StateManager.Instance;
            m_stateManager.TransitionState(StateManager.StateMachine.State.InitGame);
            m_stateManager.m_BehaviourByState.AddListener(state =>
            {
                switch (state)
                {
                    case StateManager.StateMachine.State.InitGame:
                    case StateManager.StateMachine.State.Pause:
                        m_joystick.gameObject.SetActive(false);
                        Time.timeScale = 0f;
                        break;
                    case StateManager.StateMachine.State.InTheGame:
                        m_joystick.gameObject.SetActive(true);
                        Time.timeScale = 1f;
                        break;
                    case StateManager.StateMachine.State.GameOver:
                    case StateManager.StateMachine.State.GameClear:
                        m_joystick.gameObject.SetActive(false);
                        break;
                }
            });
        }

        private void OnEnable()
        {
            Timer.OnEndCountDown += MigrateStateToGameOver;
        }

        private void OnDisable()
        {
            Timer.OnEndCountDown -= MigrateStateToGameOver;
        }

        /// <summary>
        /// Migrate state of State machine  to "GameOver"
        /// </summary>
        void MigrateStateToGameOver()
        {
            m_stateManager.TransitionState(StateManager.StateMachine.State.GameOver);
        }
    }
}