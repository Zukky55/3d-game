using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int m_point = 1000;
        [SerializeField] float m_range = 5f;
        [SerializeField] AudioSource m_audioSource;
        [SerializeField] float m_amount = 0.1f;

        StateManager m_stateManager;
        GameObject m_player;


        private void OnEnable()
        {
            m_stateManager = StateManager.Instance;
            m_player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                StageManager.Instance.AddPortals();
                Destroy(GetComponent<Collider>());
                Destroy(GetComponent<Renderer>());
                Destroy(transform.GetChild(1).gameObject);
                Destroy(transform.GetChild(0).gameObject);
                m_audioSource.Play();
                Destroy(gameObject, 3f);
            }
        }

        private void Update()
        {
            if (Vector3.Distance(m_player.transform.position, transform.position) <= m_range && m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                transform.position = Vector3.Slerp(transform.position, m_player.transform.position, m_amount);
            }
        }
    }
}