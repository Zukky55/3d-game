using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    /// <summary>
    /// Base class of the script that triggers an action by a button. ボタンでアクションを起こすスクリプトの基底クラス
    /// </summary>
    public abstract class ButtonBase : MonoBehaviour, IPointerDownHandler
    {
        protected StateManager m_stateManager;

        protected virtual void OnEnable()
        {
            m_stateManager = StateManager.Instance;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Activate();
        }

        protected abstract void Activate();
    }
}
