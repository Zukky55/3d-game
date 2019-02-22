using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ReviewGames
{
    /// <summary>
    /// Stage manager.
    /// </summary>
    public class StageManager : MonoSingleton<StageManager>
    {
        public int TotalPortalCount { get; private set; }
        public int CurrentPortalCount { get; private set; }

        [SerializeField] Timer m_timer;
        [SerializeField] FloatingJoystick m_joystick;
        [SerializeField] TextMeshProUGUI m_finishText;
        StateManager m_stateManager;
        ScoreManager m_scoreManager;
        DungeonGenerator m_dungeonGenerator;

        private void Awake()
        {
            m_stateManager = StateManager.Instance;
            m_scoreManager = ScoreManager.Instance;
            m_dungeonGenerator = DungeonGenerator.Instance;
            SetupEvents();
        }

        private void OnEnable()
        {
            Timer.OnEndCountDown += MigrateStateToGameOver;
        }

        private void OnDisable()
        {
            Timer.OnEndCountDown -= MigrateStateToGameOver;
        }

        void Initialize()
        {

        }

        void SetupEvents()
        {
            m_stateManager.m_BehaviourByState.AddListener(state =>
            {
                switch (state)
                {
                    case StateManager.StateMachine.State.InitGame:
                        SceneFader.Instance.FadeIn(3f);
                        Physics.gravity = new Vector3(0, -9.81f, 0);
                        m_joystick.gameObject.SetActive(false);
                        m_finishText.enabled = false;
                        break;
                    case StateManager.StateMachine.State.Pause:
                        m_joystick.gameObject.SetActive(false);
                        Time.timeScale = 0f;
                        break;
                    case StateManager.StateMachine.State.InTheGame:
                        m_joystick.gameObject.SetActive(true);
                        Time.timeScale = 1f;
                        break;
                    case StateManager.StateMachine.State.GameOver:
                    case StateManager.StateMachine.State.GameClear:
                        m_finishText.enabled = true;
                        m_joystick.gameObject.SetActive(false);
                        SceneFader.Instance.FadeOut(SceneFader.SceneTitle.Result, 3f);
                        break;
                }
            });

            m_stateManager.TransitionState(StateManager.StateMachine.State.InitGame);
        }

        public void SetupPortals(int totalPortals)
        {
            TotalPortalCount = totalPortals;
            ScoreManager.Instance.DisplayPortalCount(CurrentPortalCount, totalPortals);
        }

        public void AddPortals()
        {
            CurrentPortalCount++;
            ScoreManager.Instance.DisplayPortalCount(CurrentPortalCount, TotalPortalCount);

            if (CurrentPortalCount == TotalPortalCount)
            {
                m_stateManager.TransitionState(StateManager.StateMachine.State.GameClear);
            }
        }

        /// <summary>
        /// Migrate state of State machine  to "GameOver"
        /// </summary>
        void MigrateStateToGameOver()
        {
            m_stateManager.TransitionState(StateManager.StateMachine.State.GameOver);
        }
    }
}