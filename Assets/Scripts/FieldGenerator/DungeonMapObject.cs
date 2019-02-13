using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class DMObject : MonoBehaviour
    {
        public CellStatus m_status { get; set; }
        public Vector3 m_position { get; set; }
        private void OnEnable()
        {
            transform.position = m_position;
        }
    }
}