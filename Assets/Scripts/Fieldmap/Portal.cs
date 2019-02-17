using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int m_point = 1000;
        [SerializeField] AudioSource m_audioSource;
        StateManager m_stateManager;

        private void OnEnable()
        {
            m_stateManager = StateManager.Instance;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player" && m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                ScoreManager.Instance.AddScore(m_point);
                Destroy(GetComponent<Renderer>());
                Destroy(GetComponent<Collider>());
                m_audioSource.Play();
                Destroy(gameObject, 3f);
            }
        }
    }
}