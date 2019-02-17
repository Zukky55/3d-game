using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class GoalPortal : MonoBehaviour
    {
        [SerializeField] int m_point = 2500;
        [SerializeField] StateManager.StateMachine.State m_state;
        AudioSource m_audioSource;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && StateManager.Instance.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                StateManager.Instance.TransitionState(m_state);
                ScoreManager.Instance.AddScore(m_point);
                m_audioSource.Play();
            }
        }
    }
}