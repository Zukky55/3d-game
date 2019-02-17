using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] float m_xAxis;
        [SerializeField] float m_yAxis;
        [SerializeField] float m_zAxis;

        void Update()
        {
            transform.Rotate(new Vector3(m_xAxis, m_yAxis, m_zAxis) * Time.deltaTime);
        }
    }
}