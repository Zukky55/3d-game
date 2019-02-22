using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReviewGames
{
    /// <summary>
    /// Game manager
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] float m_fadeInSpeed = 3f;

        /// <summary>
        /// Scene遷移で破壊しないオブジェクトに設定
        /// </summary>
        public override void OnInitialize()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Awake()
        {
            SceneManager.sceneLoaded += ((scene, mode) =>
            {
                SceneFader.Instance.FadeIn(m_fadeInSpeed);
            });
        }
    }
}