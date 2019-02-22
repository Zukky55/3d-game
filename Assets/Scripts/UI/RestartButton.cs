using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class RestartButton : ButtonBase
    {
        [SerializeField] float m_fadingSpeed = 1f;

        protected override void Activate()
        {
            SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Stage, m_fadingSpeed);
        }
    }
}