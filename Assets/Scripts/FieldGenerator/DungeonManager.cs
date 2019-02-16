using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReviewGames
{
    public class DungeonManager : MonoBehaviour
    {
        /// <summary>スタート地点</summary>
        public Cell StartCell { get; private set; }
        /// <summary>ゴール地点</summary>
        public Cell GoalCell { get; private set; }

        /// <summary>DungeonMapのオブジェクト群</summary>
        List<GameObject> m_dungeonMapObjects;
        /// <summary>DungeonMapのコンポーネント群</summary>
        List<Cell> m_dungeonMapModules;
        /// <summary>道の偶数地点</summary>
        List<Cell> m_roadEvenCell;
        /// <summary>ゴール地点候補</summary>
        List<Cell> m_goalCandidate;
        /// <summary>StateMachine</summary>
        StateManager m_stateManager;

        /// <summary>幅</summary>
        const int m_width = 10;
        /// <summary>高さ</summary>
        const int m_height = 10;
        /// <summary>奥行</summary>
        const int m_depth = 10;
        /// <summary>迷路の外周分の余白</summary>
        const int m_margin = 2;
        /// <summary>ターゲットにする範囲</summary>
        const int targetRange = 2;
        /// <summary>サブターゲットにする範囲</summary>
        const int subTargetRange = 1;

        /// <summary>ゴール地点を設定しているかどうか</summary>
        bool m_isGoalSet;

        /// <summary>map size</summary>
        [Header("Can be changed during execution.")]
        [SerializeField]
        float m_mapSize = 1f;

        /// <summary>DungeonMapの透明度</summary>
        [Header("Can't be changed during execution.")]
        [Range(0f, 1f)]
        [SerializeField]
        public float m_alpha = 1f;

        private void Awake()
        {
            m_stateManager = StateManager.Instance;
            m_dungeonMapObjects = new List<GameObject>();
            m_dungeonMapModules = new List<Cell>();
            m_roadEvenCell = new List<Cell>();

            m_stateManager.m_BehaviourByState.AddListener(state =>
            {
                if (state == StateManager.StateMachine.State.InitGame)
                {
                    Setup();
                }
            });
        }

        /// <summary>
        /// Inspectorで値を変更した時に呼ばれるCallback method
        /// </summary>
        private void OnValidate()
        {
            // Play中以外スルー
            if (!Application.isPlaying)
            {
                return;
            }
            // mapSizeに合わせてScale変更
            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);
        }

        /// <summary>
        /// Set the Dungeon map
        /// </summary>
        public void Setup()
        {
            // Scene上のModelを取得する
            m_dungeonMapObjects = GameObject.FindGameObjectsWithTag("Cell").ToList();
            m_dungeonMapObjects.ForEach(module =>
            {
                var component = module.GetComponent<Cell>();
                component.Index = new DungeonMapIndex((int)module.transform.localPosition.x, (int)module.transform.localPosition.y, (int)module.transform.localPosition.z);
                //Debug.Log(component.Index + "x" + component.Index.x + "y" + component.Index.y + "z" + component.Index.z);
                m_dungeonMapModules.Add(component);
            });

            // 採掘開始セルを設定し,再帰処理で道を掘る
            var startCell = GetStartCell();
            DiggingInRecursively(startCell);
        }

        /// <summary>
        /// スタート地点を選定する
        /// </summary>
        /// <returns></returns>
        Cell GetStartCell()
        {
            // 指定範囲内の偶数座標をランダムに指定
            var target = new DungeonMapIndex
                (
                GetRandomEvenIndex(m_margin, m_width - m_margin),
                GetRandomEvenIndex(m_margin, m_height - m_margin),
                GetRandomEvenIndex(m_margin, m_depth - m_margin)
                );

            // 指定した座標と同じ座標のmoduleを取得してスタート地点とする
            var startCell = m_dungeonMapModules.Find(item => item.Index == target);
            startCell.Status = CellStatus.Start;
            m_roadEvenCell.Add(startCell);
            StartCell = startCell;

            // playerをスタート地点に立たせる
            var pos = startCell.gameObject.transform.position;
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(pos.x, pos.y, pos.z);

            return startCell;
        }

        /// <summary>
        /// 再帰呼び出しで道を掘る
        /// </summary>
        /// <param name="startCell"></param>
        public void DiggingInRecursively(Cell startCell)
        {

            // 掘る方向
            var dir = GetDirection(startCell);
            // 掘る2マス先のポイントとその中間地点
            DungeonMapIndex target, subTarget;
            subTarget = target = startCell.Index;

            // 観測点からみて掘り進める方向の2cell分取得
            switch (dir)
            {
                case Direction.Right:
                    target.x += targetRange;
                    subTarget.x += subTargetRange;
                    break;
                case Direction.Left:
                    target.x -= targetRange;
                    subTarget.x -= subTargetRange;
                    break;
                case Direction.Top:
                    target.y += targetRange;
                    subTarget.y += subTargetRange;
                    break;
                case Direction.Bottom:
                    target.y -= targetRange;
                    subTarget.y -= subTargetRange;
                    break;
                case Direction.Forward:
                    target.z += targetRange;
                    subTarget.z += subTargetRange;
                    break;
                case Direction.Backward:
                    target.z -= targetRange;
                    subTarget.z -= subTargetRange;
                    break;
                case Direction.None:
                    // もう掘れない場合は自分自身を掘削リストから削除し既存のポイントから掘削を再開する
                    // 最初に壁に当たったCellをゴールとする
                    if (!m_isGoalSet)
                    {
                        m_isGoalSet = true;
                        startCell.Status = CellStatus.Goal;
                        GoalCell = startCell;

                        // Goal地点にPortalを設置する
                        var pos = startCell.gameObject.transform.localPosition;
                        var portal = Instantiate(Resources.Load<GameObject>("GoalPortal"),transform);
                        portal.transform.SetParent(transform);
                        portal.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
                    }
                    m_roadEvenCell.Remove(startCell);

                    if (m_roadEvenCell.Count > 0)
                    {
                        // 再帰呼び出し
                        DiggingInRecursively(m_roadEvenCell[GetRandomEvenIndex(0, m_roadEvenCell.Count)]);
                    }

                    // 掘削ポイントがもうない場合は終了し結果をModelに同期
                    Sync();
                    return;
            }

            // 対象と中間を掘る
            var targetCell = m_dungeonMapModules.Find(cell => cell.Index == target);
            var subTargetCell = m_dungeonMapModules.Find(cell => cell.Index == subTarget);
            targetCell.Status = CellStatus.Road;
            subTargetCell.Status = CellStatus.Road;

            // 掘削リストに追加
            m_roadEvenCell.Add(targetCell);
            // 掘った先を中心に再帰呼び出し
            DiggingInRecursively(targetCell);
        }

        /// <summary>
        ///ダンジョン生成した情報とScene上のオブジェクト群を同期する
        /// </summary>
        private void Sync()
        {
            m_dungeonMapModules.ForEach(cell =>
            {
                switch (cell.Status)
                {
                    case CellStatus.Road:
                    case CellStatus.Goal:
                    case CellStatus.Start:
                        cell.gameObject.SetActive(false);
                        break;
                    case CellStatus.DigableWall:
                    case CellStatus.UndigableWall:
                        // cellのカラーをランダムに設定
                        cell.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),m_alpha);
                        break;
                }
            });
        }

        /// <summary>
        /// Pointを起点に周囲が既に道になっていないか調べる
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        Direction GetDirection(Cell cell)
        {
            var dir = new List<Direction>();
            dir.Add(Direction.Forward);
            dir.Add(Direction.Backward);
            dir.Add(Direction.Right);
            dir.Add(Direction.Left);
            dir.Add(Direction.Top);
            dir.Add(Direction.Bottom);

            while (dir.Count > 0)
            {
                var checkDir = dir[Random.Range(0, dir.Count)];
                var checkIndex = new DungeonMapIndex();

                checkIndex = cell.Index;
                switch (checkDir) // その方向の2つ先のマスをcheckPointに指定
                {
                    case Direction.Right:
                        checkIndex.x += 2;
                        break;
                    case Direction.Left:
                        checkIndex.x -= 2;
                        break;
                    case Direction.Top:
                        checkIndex.y += 2;
                        break;
                    case Direction.Bottom:
                        checkIndex.y -= 2;
                        break;
                    case Direction.Forward:
                        checkIndex.z += 2;
                        break;
                    case Direction.Backward:
                        checkIndex.z -= 2;
                        break;
                    default:
                        break;
                }
                var targetCell = m_dungeonMapModules.Find(item => item.Index == checkIndex);

                if (targetCell.Status == CellStatus.DigableWall) // 対象のマスのステータスが掘削可能ならその向きを返す
                {
                    return checkDir;
                }
                // 対象の方向が掘削不可能だった場合その方向を削除してもう一度チェックする
                dir.Remove(checkDir);
            }
            // 全方向掘削不可能だった場合Noneを返す
            return Direction.None;
        }

        /// <summary>
        /// 範囲内でランダムな偶数値を取得
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        int GetRandomEvenIndex(int min, int max)
        {
            int evenNum = 1;
            while (evenNum % 2 == 1)
            {
                evenNum = Random.Range(min, max);
            }
            return evenNum;
        }
    }

    /// <summary>
    /// そのmoduleのindex
    /// </summary>
    public struct DungeonMapIndex
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public DungeonMapIndex(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合true
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(DungeonMapIndex a, DungeonMapIndex b)
        {

            // x,y,z全ての要素が一致していた場合true
            if (a.x == b.x & a.y == b.y && a.z == b.z)
            {
                return true;
            }
            // でなければfalse
            return false;
        }

        /// <summary>
        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合false
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(DungeonMapIndex a, DungeonMapIndex b)
        {
            return !(a == b);
        }
    }

    /// <summary>
    /// 掘削方向
    /// </summary>
    public enum Direction
    {
        Right,
        Left,
        Top,
        Bottom,
        Forward,
        Backward,
        None,
    }

    /// <summary>
    /// そのCellのステータス
    /// </summary>

    public enum CellStatus
    {
        Road,
        DigableWall,
        UndigableWall,
        Goal,
        Start,
    }
}