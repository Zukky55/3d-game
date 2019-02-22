using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void Awake()
        {
            SceneManager.sceneLoaded += ((scene, mode) =>
            {
                // when loaded Stage scene
                if (scene.name == SceneFader.SceneTitle.Stage.ToString())
                {
                    m_portalCountTextBox = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
                    m_currentPortalCount = 0;

                    // Register an event in the state machine
                    StateManager.Instance.m_BehaviourByState.AddListener(state =>
                    {
                        switch (state)
                        {
                            case StateManager.StateMachine.State.InitGame:
                                // initialize
                                m_score = 0;
                                m_highScore = PlayerPrefs.GetInt("HighScore");
                                m_totalPortalCount = DungeonGenerator.Instance.AllPortals;
                                break;
                            case StateManager.StateMachine.State.GameOver:
                            case StateManager.StateMachine.State.GameClear:
                                // Save
                                if (m_highScore < m_score) // Saving that when score greater than high score
                                {
                                    PlayerPrefs.SetInt("HighScore", m_score);
                                }
                                PlayerPrefs.SetInt("Score", m_score);
                                PlayerPrefs.SetInt("TotalPortals", m_totalPortalCount);
                                PlayerPrefs.Save();
                                break;
                            default:
                                break;
                        }
                    });
                }

                // when loaded Title scene
                if (scene.name == SceneFader.SceneTitle.Title.ToString())
                {
                    m_highScore = PlayerPrefs.GetInt("HighScore", 0);
                }
            });
        }




        /// <summary>
        /// Display current acquired portals
        /// </summary>
        /// <param name="currentPotals"></param>
        /// <param name="totalPortals"></param>
        public void DisplayPortalCount(int currentPotals, int totalPortals)
        {
            m_currentPortalCount++;
            m_score++;
            m_portalCountTextBox.text = string.Format("{0:0}/{1:0}", currentPotals, totalPortals);
        }
    }
}