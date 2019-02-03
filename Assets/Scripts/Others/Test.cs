using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Test : MonoBehaviour
    {
        [SerializeField] GameObject m_player;

        private void Update()
        {
            var diff = transform.position - m_player.transform.position;
            Debug.Log("diff" + diff);
        }
    }
}