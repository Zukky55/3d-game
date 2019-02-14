using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReviewGames
{
    public class HemiSphereController : MonoBehaviour
    {
        [SerializeField] float m_rotatinaolSpeed = 0.5f;
        StateManager m_stateManager;

        private void Awake()
        {
            m_stateManager = StateManager.Instance;
        }
        private void Update()
        {
            if (m_stateManager.m_StateMachine.m_State == StateManager.StateMachine.State.InTheGame)
            {
                transform.Rotate(Vector3.up, m_rotatinaolSpeed);
                transform.RotateAround(Vector3.zero, transform.up, m_rotatinaolSpeed);
            }
        }
    }
}
