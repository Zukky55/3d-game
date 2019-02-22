using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class SceneFaderButton : MonoBehaviour
    {
        [SerializeField] SceneFader.SceneTitle m_nextScene;
        [SerializeField] float m_fadeSpeed = 3f;
        public void FadingScene()
        {
            SceneFader.Instance.FadeOut(m_nextScene, m_fadeSpeed);
        }
    }
}