using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class GravityBall : MonoBehaviour
    {
        [SerializeField] AudioSource m_audioSource;
        [SerializeField] int m_gravityPower = 3;
        [SerializeField] float m_range = 3f;
        [SerializeField] float m_amount = 0.1f;

        GameObject m_player;
        StateManager m_stateManager;

        private void OnEnable()
        {
            m_stateManager = StateManager.Instance;
            m_player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Destroy(GetComponentInChildren<Collider>());
                m_audioSource.Play();
                Destroy(transform.GetChild(0).gameObject);
                PlayerController.Instance.GravityInverseCount += m_gravityPower;
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