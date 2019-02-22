using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class DeleteButton : ButtonBase
    {
        [SerializeField] Canvas m_confirmationCanvas;

        protected override void Activate()
        {
            PlayerPrefs.DeleteAll();
            m_confirmationCanvas.gameObject.SetActive(false);
        }
    }
}