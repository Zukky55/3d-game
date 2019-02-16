using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ReviewGames
{
    /// <summary>
    /// State machine.
    /// </summary>
    public class StateManager : MonoSingleton<StateManager>
    {
        /// <summary>State machine</summary>
        public StateMachine m_StateMachine = new StateMachine();
        /// <summary>Custom UnityEvent</summary>
        public StateMachineEvent m_BehaviourByState = new StateMachineEvent();

        /// <summary>
        /// State machine event.
        /// </summary>
        public class StateMachineEvent : UnityEvent<StateMachine.State>
        {
            public StateMachineEvent() { }
        }

        private void Start()
        {
            TransitionState(StateMachine.State.InitGame);
        }

        /// <summary>
        /// Issue an event after changing the state. ステートを変更した後イベントを発行する
        /// </summary>
        /// <param name="state"></param>
        public void TransitionState(StateMachine.State state)
        {
            m_StateMachine.m_PreviousState = m_StateMachine.m_State; // ステート遷移前のステートを保存 
            m_StateMachine.m_State = state; // stateをセット
            // ==================================
            //                                          Event call.                                         
            // ==================================
            m_BehaviourByState.Invoke(state);
        }

        /// <summary>
        /// State machine.
        /// </summary>
        [System.Serializable]
        public class StateMachine
        {
            /// <summary>Represents the current state.</summary>
            [SerializeField] public State m_State;
            /// <summary>The previous state.</summary>
            [SerializeField] public State m_PreviousState;

            /// <summary>
            /// States.
            /// </summary>
            public enum State
            {
                InitGame,
                InTheGame,
                GameOver,
                GameClear,
                Pause,
                NextStage,
            }
        }
    }
}

