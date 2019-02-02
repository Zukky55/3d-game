using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        /// <summary>ジョイスティックハンドルの可動範囲</summary>
        [Header("Options")]
        [Range(0f, 2f)] public float handleLimit = 1f;
        /// <summary>ジョイスティックモード</summary>
        public JoystickMode joystickMode = JoystickMode.Both;

        /// <summary>入力されたジョイスティックの方向</summary>
        protected Vector2 inputVector = Vector2.zero;

        /// <summary>ジョイスティックの背景</summary>
        [Header("Components")]
        public RectTransform background;
        /// <summary>ジョイスティックのハンドル</summary>
        public RectTransform handle;

        /// <summary>x成分</summary>
        public float Horizontal { get { return inputVector.x; } }
        /// <summary>y成分</summary>
        public float Vertical { get { return inputVector.y; } }
        /// <summary>ハンドルのベクトル</summary>
        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        /// <summary>
        /// ドラッグされた時のコールバック
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDrag(PointerEventData eventData) { }

        /// <summary>
        /// タッチした時のコールバック
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData) { }

        /// <summary>
        /// タッチを離した時のコールバック
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerUp(PointerEventData eventData) { }

        /// <summary>
        /// JoystickModeに合わせて稼働方向に制限を与える
        /// </summary>
        protected void ClampJoystick()
        {
            if (joystickMode == JoystickMode.Horizontal)
                inputVector = new Vector2(inputVector.x, 0f);
            if (joystickMode == JoystickMode.Vertical)
                inputVector = new Vector2(0f, inputVector.y);
        }
    }

    /// <summary>
    /// ジョイスティックのモード
    /// </summary>
    public enum JoystickMode
    {
        /// <summary>両方</summary>
        Both,
        /// <summary>水平のみ</summary>
        Horizontal,
        /// <summary>垂直のみ</summary>
        Vertical,
    }
}