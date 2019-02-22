using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ReviewGames
{
    public class CountDown : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_countDownText;
        [SerializeField] TextMeshProUGUI m_discriptionText;
        [SerializeField] int m_setTimeLimit;
        float m_timeLimit;

        PlayerController m_player;

        private void Awake()
        {
            m_player = PlayerController.Instance;
        }

        private void Update()
        {
            if (m_player.IsPossibleToInverseGravity || StateManager.Instance.m_StateMachine.m_State != StateManager.StateMachine.State.InTheGame)
            {
                m_timeLimit = m_setTimeLimit;
                m_countDownText.enabled = false;
                m_discriptionText.enabled = false;
                return;
            }

            m_countDownText.enabled = true;
            m_discriptionText.enabled = true;
            m_timeLimit -= Time.deltaTime;
            m_countDownText.text = string.Format("Last{0:0}sec", m_timeLimit);

            if (m_timeLimit <= 0f)
            {
                StateManager.Instance.TransitionState(StateManager.StateMachine.State.GameOver);
                m_countDownText.text = "";
            }
        }
    }
}