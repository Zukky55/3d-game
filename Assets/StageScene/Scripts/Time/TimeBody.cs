using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間を保持する
/// </summary>
public class TimeBody : MonoBehaviour
{
    #region Field

    /// <summary>Frame毎の座標と回転を保持</summary>
    List<PointInTime> pointsInTime;
    /// <summary>Rigidbody</summary>
    Rigidbody rb;


    /// <summary>巻き戻し中かどうか</summary>
    bool isRewinding = false;
    /// <summary>過去のPointInTimeを保持する秒数</summary>
    [SerializeField] float recordTime = 5f;

    #endregion
    #region Method

    private void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }
    }

    /// <summary>
    /// 1frame0.02秒固定でtransformの登録と巻き戻しを行う
    /// </summary>
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    /// <summary>
    /// 巻き戻し
    /// </summary>
    private void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    /// <summary>
    /// 登録
    /// </summary>
    private void Record()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    /// <summary>
    /// 巻き戻し開始
    /// </summary>
    private void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    /// <summary>
    /// 巻き戻し停止
    /// </summary>
    private void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }
    #endregion
}
