using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var resultVec = GetDirection(dir);
        Debug.Log(resultVec);

        Physics.gravity = dir.normalized * m_accelerationOfGravity;
        m_player.up = -1 * resultVec;
    }

    /// <summary>
    /// 引数の<param name="vec">Vector3</param>から各成分の絶対値を取り、最大値の成分のみ入った<returns>Vector3</returns>を返す
    /// </summary>
    Vector3 GetDirection(Vector3 vec)
    {
        var x = vec.x >= 0f ? vec.x : -vec.x;
        var y = vec.y >= 0f ? vec.y : -vec.y;
        var z = vec.z >= 0f ? vec.z : -vec.z;

        var result = new Vector3
            (
             x > y && x > z ? vec.x : 0f,
             y > x && y > z ? vec.y : 0f,
             z > x && z > y ? vec.z : 0f
             );

        //if (result == Vector3.zero)
        //    Debug.Log("成分値が同値でした");

        return result;
    }
}
