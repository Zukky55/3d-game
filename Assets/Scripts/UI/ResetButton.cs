using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class ResetButton : ButtonBase
    {
        [SerializeField] Canvas m_confirmationCanvas;
        protected override void Activate()
        {
            m_confirmationCanvas.gameObject.SetActive(true);
            m_confirmationCanvas.enabled = true ;
        }
    }
}