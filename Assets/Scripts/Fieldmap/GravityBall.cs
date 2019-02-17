using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class GravityBall : MonoBehaviour
    {
        [SerializeField] GravityButton m_gravityButton;
        [SerializeField] AudioSource m_audioSource;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                m_audioSource.Play();
                Destroy(GetComponent<Renderer>());
                Destroy(GetComponent<Collider>());
                Destroy(gameObject, 2f);
            }
        }
    }
}