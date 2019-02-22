using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class BgmManager : MonoBehaviour
    {
        [SerializeField] AudioSource m_audioSource;
        [SerializeField] [Range(0f, 1f)] float m_volume = 0.5f;
        StateManager m_stateManager;

        private void Awake()
        {
            m_stateManager = StateManager.Instance;

            m_stateManager.m_BehaviourByState.AddListener(state =>
            {
                switch (state)
                {
                    case StateManager.StateMachine.State.InTheGame:
                        switch (m_stateManager.m_StateMachine.m_PreviousState)
                        {
                            case StateManager.StateMachine.State.InitGame:
                                m_audioSource.Play();
                                break;
                            case StateManager.StateMachine.State.Pause:
                                m_audioSource.volume = 1f;
                                break;
                        }
                        break;
                    case StateManager.StateMachine.State.Pause:
                        m_audioSource.volume = m_volume;
                        break;
                    case StateManager.StateMachine.State.GameClear:
                    case StateManager.StateMachine.State.GameOver:
                        StartCoroutine(FadeOutBGM());
                        break;
                }
            });
        }

        IEnumerator FadeOutBGM()
        {
            while (m_volume > 0f)
                m_volume -= Time.unscaledDeltaTime;
            yield return null;
        }

        private void Update()
        {
            if (m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                m_audioSource.volume = m_volume;
            }
        }
    }
}