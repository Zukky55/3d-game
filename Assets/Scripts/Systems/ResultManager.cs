using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ReviewGames
{
    public class ResultManager : MonoBehaviour
    {
        ScoreManager m_scoreManager;
        [SerializeField] TextMeshProUGUI m_scoreText;
        [SerializeField] TextMeshProUGUI m_highScoreText;
        [SerializeField] TextMeshProUGUI m_perfectText;
        [SerializeField] float m_textScale = 10;
        [SerializeField] float m_waitTime = 3f;

        private void Awake()
        {
            m_scoreManager = ScoreManager.Instance;
            m_perfectText.enabled = false;
        }

        private void Start()
        {
            var score = PlayerPrefs.GetInt("Score");
            var totalPortals = PlayerPrefs.GetInt("TotalPortals");
            var highScore = PlayerPrefs.GetInt("HighScore");
            m_scoreText.text = string.Format("{0:##0}/{1:##0}", score, totalPortals);
            m_highScoreText.text = string.Format("{0:##0}/{1:##0}", highScore, totalPortals);
            if (score == totalPortals)
            {
                StartCoroutine(DisplayPerfectText());
            }
        }

        IEnumerator DisplayPerfectText()
        {
            yield return new WaitForSeconds(m_waitTime);
            m_perfectText.enabled = true;
            Vector3 vec = new Vector3(m_textScale, m_textScale, m_textScale);
            while (m_textScale > 1f)
            {
                m_perfectText.rectTransform.localScale = vec;
                vec = Vector3.Slerp(vec, Vector3.one, 0.5f);
                yield return null;
            }
        }
    }
}