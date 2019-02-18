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
            Debug.Log("JumpButton");
            m_playerCtrl.Jump();
        }
    }
}