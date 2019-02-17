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
        /// <summary>score</summary>
        [SerializeField] int m_score;
        /// <summary>high score</summary>
        [SerializeField] int m_highScore;
        /// <summary>残り時間を表示するテキスト</summary>
        [SerializeField] TextMeshProUGUI m_scoreText;

        private void Awake()
        {
            m_scoreText.text = string.Format("{0:000000}P", m_score);
        }

        public void AddScore(int point)
        {
            m_score += point;
            m_scoreText.text = string.Format("{0:000000}P", m_score);
        }
    }
}