using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReviewGames
{
    /// <summary>
    /// Countdown.
    /// </summary>
    public class Timer : MonoBehaviour
    {
        #region Field
        /// <summary>イベントに使用するデリゲート</summary>
        public delegate void CountDownEvent();

        /// <summary>カウントダウン開始時に発行されるイベント</summary>
        public static event CountDownEvent OnStartCountDown = () => { };
        /// <summary>カウントダウン中毎フレーム発行されるイベント</summary>
        public static event CountDownEvent OnDuringCountDown = () => { };
        /// <summary>カウントダウン終了時に発行されるイベント</summary>
        public static event CountDownEvent OnEndCountDown = () => { };

        [Header("Parameters")]

        /// <summary>初期化時に代入する分数</summary>
        [SerializeField] int m_setMinute;
        /// <summary>初期化時に代入する秒数</summary>
        [SerializeField] float m_setSeconds;
        /// <summary>残り分数</summary>
        int m_minute;
        /// <summary>残り秒数</summary>
        float m_seconds;
        /// <summary>タイマーのスイッチングフラグ</summary>
        bool m_toggle;

        [Header("Components")]

        /// <summary>残り時間を表示するテキスト</summary>
        [SerializeField] TextMeshProUGUI m_timerText;
        /// <summary>StateMachine</summary>
        StateManager m_stateManager;

        #endregion
        #region Method
        private void Awake()
        {
            m_stateManager = StateManager.Instance;

            m_stateManager.m_BehaviourByState.AddListener(state =>
            {
                switch (state)
                {
                    case StateManager.StateMachine.State.InitGame:
                        m_timerText.enabled = false;
                        break;

                    case StateManager.StateMachine.State.InTheGame:
                        switch (m_stateManager.m_StateMachine.m_PreviousState)
                        {
                            // Timer開始処理
                            case StateManager.StateMachine.State.InitGame:
                                m_timerText.enabled = true;
                                StartCountDown();
                                break;
                            // Pauseから遷移してきた時は再開処理
                            case StateManager.StateMachine.State.Pause:
                                m_toggle = true;
                                break;
                            default:
                                break;
                        }
                        break;

                    case StateManager.StateMachine.State.Pause:
                        m_toggle = false;
                        break;
                    default:
                        break;
                }
            });
        }

        void Update()
        {
            if (m_toggle)
            {
                DuringCountDown();
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void InitTimer()
        {
            m_minute = m_setMinute;
            m_seconds = m_setSeconds;
        }

        /// <summary>
        /// 指定した時間で初期化
        /// </summary>
        /// <param name="minute"></param>
        /// <param name="seconds"></param>
        public void InitTimer(int minute, float seconds)
        {
            m_minute = minute;
            m_seconds = seconds;
        }

        /// <summary>
        /// カウントダウン開始
        /// </summary>
        public void StartCountDown()
        {
            InitTimer();
            m_toggle = true;

            // =============
            // Event call
            // =============
            OnStartCountDown();
        }

        /// <summary>
        /// 時間を指定してカウントダウン開始
        /// </summary>
        public void StartCountDown(int minute, float seconds)
        {
            InitTimer(minute, seconds);
            m_toggle = true;

            // =============
            // Event call
            // =============
            OnStartCountDown();
        }

        /// <summary>
        /// カウントダウンが行われている間の処理
        /// </summary>
        void DuringCountDown()
        {
            // =============
            // Event call
            // =============
            OnDuringCountDown();

            //  カウントダウン処理を行う. 残り時間が0になったら終了しイベントを発行する
            m_seconds -= Time.deltaTime;
            if (m_seconds <= 0)
            {
                if (m_minute == 0)
                {
                    m_seconds = 0;
                    DisplayText();
                    m_toggle = false;

                    // =============
                    // Event call
                    // =============
                    OnEndCountDown();
                }

                if (m_minute > 0)
                {
                    m_minute--;
                    m_seconds += 60;
                }
            }
            DisplayText();
        }

        /// <summary>
        /// 画面にテキスト表示を行う
        /// </summary>
        void DisplayText()
        {
            m_timerText.text = string.Format("{0:00}:{1:00}", m_minute, m_seconds);
        }
        #endregion
    }
}
