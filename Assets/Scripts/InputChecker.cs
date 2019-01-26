using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    Vector3 acceleration;
    Compass compass;
    Quaternion gyro;
    GUIStyle labelStyle;

    [SerializeField] float xCul = 10;
    [SerializeField] float yCul = 12;
    [SerializeField] float wCul1 = 8;
    [SerializeField] float wCul2 = 10;
    [SerializeField] float hCul1 = 20;
    [SerializeField] float hCul2 = 10;

    private void Start()
    {
        // Create Font
        labelStyle = new GUIStyle();
        labelStyle.fontSize = Screen.height / 22;
        labelStyle.normal.textColor = Color.white;

        Input.compass.enabled = true;

        Debug.Log(string.Format("<b>精度</b> : {0}", Input.compass.headingAccuracy));
        Debug.Log(string.Format("<b>タイムスタンプ</b> : {0}", Input.compass.timestamp));
        Input.gyro.enabled = true;
    }
    private void Update()
    {
        acceleration = Input.acceleration;
        compass = Input.compass;
        gyro = Input.gyro.attitude;
    }

    private void OnGUI()
    {
        if (acceleration != null)
        {
            float x = Screen.width / xCul;
            float y = 0;
            float w = Screen.width * wCul1 / wCul2;
            float h = Screen.height / hCul1;

            for (int i = 0; i < yCul; i++)
            {
                y = Screen.height / hCul2 + h * i;
                string text = string.Empty;

                switch (i)
                {
                    case 0://X
                        text = string.Format("accel-X:{0}", this.acceleration.x);
                        break;
                    case 1://Y
                        text = string.Format("accel-Y:{0}", this.acceleration.y);
                        break;
                    case 2://Z
                        text = string.Format("accel-Z:{0}", this.acceleration.z);
                        break;
                    case 3://X
                        text = string.Format("comps-X:{0}", this.compass.rawVector.x);
                        break;
                    case 4://Y
                        text = string.Format("comps-Y:{0}", this.compass.rawVector.y);
                        break;
                    case 5://Z
                        text = string.Format("comps-Z:{0}", this.compass.rawVector.z);
                        break;
                    case 6://Z
                        text = string.Format("magneticHeading:{0}", this.compass.magneticHeading);
                        break;
                    case 7://Z
                        text = string.Format("trueHeading:{0}", this.compass.trueHeading);
                        break;
                    case 8://Y
                        text = string.Format("gyro-x:{0}", this.gyro.x);
                        break;
                    case 9://Y
                        text = string.Format("gyro-y:{0}", this.gyro.y);
                        break;
                    case 10://Y
                        text = string.Format("gyro-z:{0}", this.gyro.z);
                        break;
                    case 11://Y
                        text = string.Format("gyro-w:{0}", this.gyro.w);
                        break;
                    default:
                        throw new System.InvalidOperationException();
                }
                GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
            }
        }
    }
}
