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
        PlayerController m_player;
        GravityController m_gravityController;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void Update()
        {
            if (m_player.GravityInverseCount > 0)
            {
                m_buttonText.text = string.Format("<b>Inverse!!</b>");
            }
            else
            {
                m_buttonText.text = "";
            }
            DisplayCount();
        }

        private void Awake()
        {
            m_gravityController = GravityController.Instance;
            m_player = PlayerController.Instance;
            DisplayCount();
        }

        protected override void Activate()
        {
            m_player.InverseGravity();
            DisplayCount();
        }

        public void DisplayCount()
        {
            m_countText.text = string.Format("x{0:0}", m_player.GravityInverseCount);
        }
    }
}