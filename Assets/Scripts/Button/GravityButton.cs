using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace ReviewGames
{
    /// <summary>
    /// Change the gravity source in the direction set by the inspector. インスペクタによって設定された方向に重力源を変更する
    /// </summary>
    public class GravityButton : ButtonBase
    {
        [Header("Componets")]
        [SerializeField] TextMeshProUGUI m_countText;
        [SerializeField] TextMeshProUGUI m_buttonText;
        GravityController m_gravityController;

        [Header("Parameters")]
        [SerializeField] int m_gravityCount = 5;
        GravityController.GravitySource m_targetGravitySource = GravityController.GravitySource.Down;


        protected override void OnEnable()
        {
            base.OnEnable();
            m_gravityController = GravityController.Instance;
        }

        private void Update()
        {
            if (m_gravityCount > 0)
            {
                m_buttonText.text = string.Format("<b>Push!!</b>");
            }
            else
            {
                m_buttonText.text = "";
            }
        }

        private void Awake()
        {
            m_countText.text = string.Format("x{0:0}", m_gravityCount);
        }

        public void AddGravityBall()
        {
            m_gravityCount++;
        }

        protected override void Activate()
        {
            if (m_stateManager.m_StateMachine.m_State != StateManager.StateMachine.State.InTheGame || m_gravityCount < 1)
            {
                return;
            }

            switch (m_targetGravitySource)
            {
                case GravityController.GravitySource.Up:
                    m_targetGravitySource = GravityController.GravitySource.Down;
                    break;
                case GravityController.GravitySource.Down:
                    m_targetGravitySource = GravityController.GravitySource.Up;
                    break;
                default:
                    break;
            }

            m_gravityCount--;
            m_countText.text = string.Format("x{0:0}", m_gravityCount);
            m_gravityController.ChangeGravitySource(m_targetGravitySource);
        }
    }
}