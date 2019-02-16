using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] StateManager.StateMachine.State m_state;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StateManager.Instance.TransitionState(m_state);
            }
        }
    }
}