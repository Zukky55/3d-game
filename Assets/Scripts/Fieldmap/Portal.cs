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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                StageManager.Instance.AddPortals();
                Destroy(GetComponent<Collider>());
                Destroy(transform.GetChild(0).gameObject);
                m_audioSource.Play();
                Destroy(gameObject, 3f);
            }
        }
    }
}