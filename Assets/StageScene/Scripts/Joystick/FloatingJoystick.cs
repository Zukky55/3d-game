using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    public class FloatingJoystick : Joystick
    {
        #region Field
        Vector2 joystickCenter = Vector2.zero;

        #endregion

        #region Method
        void Start()
        {
            background.gameObject.SetActive(false);
        }

        /// <summary>
        /// タッチしている座標をjoystickの中心にする。
        /// joystickの背景との距離が背景の半径を超している間はそれ以上背景から遠くならない様制限を掛ける
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - joystickCenter;
            inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);

            ClampJoystick();
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        }

        /// <summary>
        /// タッチされた時joystickを有効にし、タッチした座標をjoystickの座標に代入する
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            background.gameObject.SetActive(true);
            background.position = eventData.position;
            handle.anchoredPosition = Vector2.zero;
            joystickCenter = eventData.position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            inputVector = Vector2.zero;
        }
        #endregion
    }
}