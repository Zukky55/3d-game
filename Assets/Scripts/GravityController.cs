using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityController : MonoBehaviour
{
    [SerializeField] float m_accelerationOfGravity = 9.18f;
    [SerializeField] Transform m_centerOfGravity;
    Transform m_player;

    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100f))
            {
                GameObject go = hit.collider.gameObject;
                if (go.tag == "GravitySource")
                {
                    m_centerOfGravity = go.transform;
                }
            }
        }

        Vector3 dir = m_centerOfGravity.position - m_player.position;


        Physics.gravity = dir.normalized * m_accelerationOfGravity;
        m_player.up = -1 * dir;
    }

    /// <summary>
    /// Maxs the vec.
    /// vectorを渡して成分の最大値のみ残したvectorを返す
    /// </summary>
    /// <returns>The vec.</returns>
    /// <param name="vec">Vec.</param>
    //Vector3 MaxVec(Vector3 vec) 
    //{
    //    var x = vec.x;
    //    var y = vec.y;
    //    var z = vec.z;
    //}
}
