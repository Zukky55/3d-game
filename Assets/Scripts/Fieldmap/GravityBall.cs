using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class GravityBall : MonoBehaviour
    {
        [SerializeField] AudioSource m_audioSource;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                m_audioSource.Play();
                Destroy(transform.GetChild(0).gameObject);
                PlayerController.Instance.GravityInverseCount += 5;
                Destroy(gameObject, 3f);
            }
        }
    }
}