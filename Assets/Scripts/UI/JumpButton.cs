using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    /// <summary>
    /// Jump button
    /// </summary>
    public class JumpButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] PlayerController m_playerCtrl;
        public void OnPointerDown(PointerEventData eventData)
        {
            m_playerCtrl.Jump();
        }
    }
}