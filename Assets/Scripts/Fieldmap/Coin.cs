using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] int m_point = 100;
        [SerializeField]AudioSource m_audioSource;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player"  )
            {
                ScoreManager.Instance.AddScore(m_point);
                m_audioSource.Play();
                Destroy(GetComponent<Renderer>());
                Destroy(GetComponent<Collider>());
                Destroy(gameObject, 2f);
            }
        }
    }
}