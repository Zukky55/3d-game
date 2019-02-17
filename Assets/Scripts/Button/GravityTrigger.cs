using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class GravityTrigger : MonoBehaviour
    {
        [Header("Componets")]
        GravityController m_gravityController;
        StateManager m_stateManager;

        [Header("Parameters")]
        [SerializeField] GravityController.GravitySource m_targetGravitySource;


        void OnEnable()
        {
            m_gravityController = GravityController.Instance;
            m_stateManager = StateManager.Instance;
        }

        void Activate()
        {
            if (m_stateManager.m_StateMachine.m_State != StateManager.StateMachine.State.InTheGame)
            {
                return;
            }

            m_gravityController.ChangeGravitySource(m_targetGravitySource);
        }
    }
}
