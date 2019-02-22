using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class ColorGradient : MonoBehaviour
    {
        MeshRenderer m_renderer;
        [SerializeField] float speed = .5f;

        private void Awake()
        {
            m_renderer = GetComponent<MeshRenderer>();
        }


        private void Update()
        {
            var value = Mathf.Sin(Time.realtimeSinceStartup);
            m_renderer.material.color = new Color(Mathf.Sin(value * speed), Mathf.Sin(value * speed), Mathf.Sin(value * speed),Mathf.Sin(value) * speed);
        }
    }
}