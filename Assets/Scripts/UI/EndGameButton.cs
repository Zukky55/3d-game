using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class EndGameButton : ButtonBase
    {
        [SerializeField] float m_fadingSpeed = 1f;

        protected override void Activate()
        {
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Title, m_fadingSpeed);
        }
    }   
}