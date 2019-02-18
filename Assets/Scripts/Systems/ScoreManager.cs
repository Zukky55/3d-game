using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ReviewGames
{
    /// <summary>
    /// Score manager
    /// </summary>
    [System.Serializable]
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        /// <summary>Text to display the current number of acquired portals</summary>
        [SerializeField] TextMeshProUGUI m_portalCountTextBox;

        /// <summary>score</summary>
        [SerializeField] int m_score;
        /// <summary>high score</summary>
        [SerializeField] int m_highScore;
        /// <summary>Current portal count.</summary>
        [SerializeField] int m_currentPortalCount;
        /// <summary>Total portal count</summary>
        [SerializeField] int m_totalPortalCount;

        public void DisplayPortalCount(int currentPotals, int totalPortals)
        {
            m_currentPortalCount++;
            m_portalCountTextBox.text = string.Format("{0:0}/{1:0}", currentPotals, totalPortals);
        }
    }
}