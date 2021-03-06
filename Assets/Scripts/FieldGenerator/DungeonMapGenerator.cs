﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{

    /// <summary>
    /// 3D迷路の自動生成
    /// </summary>
    public class DungeonMapGenerator : MonoBehaviour
    {
        //        /// <summary>DungeonMapのオブジェクト群</summary>
        //        public List<GameObject> DungeonMapObjects { get; private set; }
        //        /// <summary>DungeonMapのコンポーネント群</summary>
        //        public List<DungeonMapObject> DungeonMapComponents { get; private set; }
        //        /// <summary>道の偶数地点</summary>
        //        public List<DungeonMapObject> RoadEvenCell { get; private set; }
        //        /// <summary>スタート地点</summary>
        //        public DungeonMapObject StartPoint { get; private set; }
        //        /// <summary>ゴール地点</summary>
        //        public DungeonMapObject GoalPoint { get; private set; }

        //        StateManager m_stateManager;
        //        GameObject m_glassPrefab;
        //        GameObject m_brickPrefab;

        //        /// <summary>map size</summary>
        //        [Header("Can be changed during execution.")]
        //        [SerializeField] float m_mapSize;

        //        /// <summary>幅</summary>
        //        [Header("Can't be changed during execution.")]
        //        [SerializeField] int m_width;
        //        /// <summary>高さ</summary>
        //        [SerializeField] int m_height;
        //        /// <summary>奥行</summary>
        //        [SerializeField] int m_depth;
        //        /// <summary>コライダーを必要とするかどうか</summary>
        //        [SerializeField] bool m_needToCollider;
        //        /// <summary>迷路の外周分の余白</summary>
        //        [SerializeField] int m_margin = 2;
        //        /// <summary>ゴール地点を設定しているかどうか</summary>
        //        bool m_isGoalSet;
        //        /// <summary>ターゲットにする範囲</summary>
        //        const int targetRange = 2;
        //        /// <summary>サブターゲットにする範囲</summary>
        //        const int subTargetRange = 1;

        //        private void Awake()
        //        {
        //            //外周分を加算
        //            m_width += m_margin;
        //            m_height += m_margin;
        //            m_depth += m_margin;

        //            // Loading prefab from Resources folder.
        //            m_glassPrefab = Resources.Load<GameObject>("Glass");
        //            m_brickPrefab = Resources.Load<GameObject>("BrickTile");

        //            RoadEvenCell = new List<DungeonMapObject>();
        //            DungeonMapObjects = new List<GameObject>();
        //            DungeonMapComponents = new List<DungeonMapObject>();
        //            m_stateManager = StateManager.Instance;


        //            m_stateManager.m_BehaviourByState.AddListener(state =>
        //            {
        //                if (state == StateManager.StateMachine.State.InitGame)
        //                {
        //                    Setup();
        //                }
        //            });
        //        }

        //        private void OnValidate()
        //        {
        //            // Play中以外スルー
        //            if (!Application.isPlaying)
        //            {
        //                return;
        //            }
        //            // mapSizeに合わせてScale変更
        //            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);
        //        }



        //        /// <summary>
        //        /// マップ配列の各要素に役割を割り当てる
        //        /// </summary>
        //        public void Setup()
        //        {
        //            // 各面において両端の場合は掘削不可能な壁,それ以外は掘削可能な壁とする
        //            for (int y = 0; y < m_height; y++)
        //            {
        //                for (int z = 0; z < m_depth; z++)
        //                {
        //                    for (int x = 0; x < m_width; x++)
        //                    {
        //                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //                        cube.transform.SetParent(transform);
        //                        cube.transform.localScale = Vector3.one;
        //                        cube.transform.localPosition = new Vector3(x, y, z);
        //                        // もしコライダーが必要なければ消す
        //                        if (!m_needToCollider)
        //                        {
        //                            Destroy(cube.GetComponent<BoxCollider>());
        //                        }
        //                        // Componentを追加,Listに追加,各要素初期化
        //                        var component = cube.AddComponent<DungeonMapObject>();
        //                        DungeonMapObjects.Add(cube);
        //                        DungeonMapComponents.Add(component);
        //                        // 各面において余白の場合掘削不可能壁に. それ以外は掘削可能壁に設定.
        //                        if (y < m_margin | y >= m_height - m_margin
        //                            | z < m_margin | z >= m_depth - m_margin
        //                            | x < m_margin | x >= m_width - m_margin)
        //                        {
        //                            component.Status = CellStatus.UndigableWall;
        //                        }
        //                        else
        //                        {
        //                            component.Status = CellStatus.DigableWall;
        //                        }
        //                        component.Index = new DungeonMapIndex(x, y, z);
        //                    }
        //                }
        //            }
        //            // 最初のポイントを作成し,再帰処理で道の作成を開始
        //            DiggingInRecursively(GetFirstPoint());
        //        }

        //        /// <summary>
        //        /// 迷路の開始地点を決める. 開始地点は外周以外の偶数を選択
        //        /// </summary>
        //        DungeonMapObject GetFirstPoint()
        //        {
        //            var target = new DungeonMapIndex
        //                (
        //                GetRandomEvenIndex(m_margin, m_width - m_margin),
        //                GetRandomEvenIndex(m_margin, m_height - m_margin),
        //                GetRandomEvenIndex(m_margin, m_depth - m_margin)
        //                );

        //            var startCell = DungeonMapComponents.Find(item => item.Index == target);
        //            startCell.Status = CellStatus.Start;
        //            RoadEvenCell.Add(startCell);
        //            StartPoint = startCell;
        //            // playerをスタート地点に立たせる
        //            var pos = startCell.gameObject.transform.position;
        //            var player = GameObject.FindGameObjectWithTag("Player");
        //            player.transform.position = new Vector3(pos.x, pos.y, pos.z);
        //            return startCell;
        //        }

        //        /// <summary>
        //        /// 再帰呼び出しで道を掘る
        //        /// </summary>
        //        /// <param name="startPoint"></param>
        //        public void DiggingInRecursively(DungeonMapObject startPoint)
        //        {

        //            // 掘る方向
        //            var dir = GetDirection(startPoint);
        //            // 掘る2マス先のポイントとその中間地点
        //            DungeonMapIndex target, subTarget;
        //            subTarget = target = startPoint.Index;
        //            Debug.Log("startCell is : " + target.x + "," + target.y + "," + target.z);

        //            switch (dir)
        //            {
        //                case Direction.Right:
        //                    target.x += targetRange;
        //                    subTarget.x += subTargetRange;
        //                    break;
        //                case Direction.Left:
        //                    target.x -= targetRange;
        //                    subTarget.x -= subTargetRange;
        //                    break;
        //                case Direction.Top:
        //                    target.y += targetRange;
        //                    subTarget.y += subTargetRange;
        //                    break;
        //                case Direction.Bottom:
        //                    target.y -= targetRange;
        //                    subTarget.y -= subTargetRange;
        //                    break;
        //                case Direction.Forward:
        //                    target.z += targetRange;
        //                    subTarget.z += subTargetRange;
        //                    break;
        //                case Direction.Backward:
        //                    target.z -= targetRange;
        //                    subTarget.z -= subTargetRange;
        //                    break;
        //                case Direction.None:
        //                    // もう掘れない場合は自分自身を掘削リストから削除し、既存のポイントから掘削を再開する
        //                    // 最初に壁に当たった一回目をゴールとする
        //                    if (!m_isGoalSet)
        //                    {
        //                        m_isGoalSet = true;
        //                        startPoint.Status = CellStatus.Goal;
        //                        GoalPoint = startPoint;
        //                    }
        //                    RoadEvenCell.Remove(startPoint);

        //                    if (RoadEvenCell.Count > 0)
        //                    {
        //                        // 再帰呼び出し
        //                        DiggingInRecursively(RoadEvenCell[GetRandomEvenIndex(0, RoadEvenCell.Count)]);
        //                    }
        //                    // 掘削ポイントがもうない場合は終了し結果をオブジェクトに同期
        //                    Sync();

        //                    return;
        //            }
        //            // 対象と中間を掘る
        //            var targetCell = DungeonMapComponents.Find(item => item.Index == target);
        //            var subTargetCell = DungeonMapComponents.Find(item => item.Index == subTarget);
        //            targetCell.Status = CellStatus.Road;
        //            subTargetCell.Status = CellStatus.Road;

        //            // 掘削リストに追加
        //            RoadEvenCell.Add(targetCell);
        //            // 掘った先を中心に再帰呼び出し
        //            DiggingInRecursively(targetCell);
        //        }

        //        private void Sync()
        //        {
        //            DungeonMapComponents.ForEach(item =>
        //            {
        //                switch (item.Status)
        //                {
        //                    case CellStatus.Road:
        //                    case CellStatus.Goal:
        //                    case CellStatus.Start:
        //                        item.gameObject.SetActive(false);
        //                        break;
        //                    case CellStatus.DigableWall:
        //                    case CellStatus.UndigableWall:
        //                        item.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.1f);
        //                        break;
        //                }
        //            });
        //        }



        //        /// <summary>
        //        /// 範囲内でランダムな偶数値を取得
        //        /// </summary>
        //        /// <param name="min"></param>
        //        /// <param name="max"></param>
        //        /// <returns></returns>
        //        int GetRandomEvenIndex(int min, int max)
        //        {
        //            int evenNum = 1;
        //            while (evenNum % 2 == 1)
        //            {
        //                evenNum = Random.Range(min, max);
        //            }
        //            return evenNum;
        //        }

        //        /// <summary>
        //        /// Pointを起点に周囲が既に道になっていないか調べる
        //        /// </summary>
        //        /// <param name="dmo"></param>
        //        /// <returns></returns>
        //        Direction GetDirection(DungeonMapObject dmo)
        //        {
        //            var dir = new List<Direction>();
        //            dir.Add(Direction.Forward);
        //            dir.Add(Direction.Backward);
        //            dir.Add(Direction.Right);
        //            dir.Add(Direction.Left);
        //            dir.Add(Direction.Top);
        //            dir.Add(Direction.Bottom);

        //            while (dir.Count > 0)
        //            {
        //                var checkDir = dir[Random.Range(0, dir.Count)];
        //                var checkIndex = new DungeonMapIndex();

        //                checkIndex = dmo.Index;
        //                switch (checkDir) // その方向の2つ先のマスをcheckPointに指定
        //                {
        //                    case Direction.Right:
        //                        checkIndex.x += 2;
        //                        break;
        //                    case Direction.Left:
        //                        checkIndex.x -= 2;
        //                        break;
        //                    case Direction.Top:
        //                        checkIndex.y += 2;
        //                        break;
        //                    case Direction.Bottom:
        //                        checkIndex.y -= 2;
        //                        break;
        //                    case Direction.Forward:
        //                        checkIndex.z += 2;
        //                        break;
        //                    case Direction.Backward:
        //                        checkIndex.z -= 2;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                Debug.Log("checkIndex is : " + checkIndex.x + "," + checkIndex.y + "," + checkIndex.z);
        //                var targetCell = DungeonMapComponents.Find(item => item.Index == checkIndex);

        //                if (targetCell.Status == CellStatus.DigableWall) // 対象のマスのステータスが掘削可能ならその向きを返す
        //                {
        //                    return checkDir;
        //                }
        //                // 対象の方向が掘削不可能だった場合その方向を削除してもう一度チェックする
        //                dir.Remove(checkDir);
        //            }
        //            // 全方向掘削不可能だった場合Noneを返す
        //            return Direction.None;
        //        }
        //    }

        //    public struct DungeonMapIndex
        //    {
        //        public int x { get; set; }
        //        public int y { get; set; }
        //        public int z { get; set; }

        //        public DungeonMapIndex(int x, int y, int z)
        //        {
        //            this.x = x;
        //            this.y = y;
        //            this.z = z;
        //        }

        //        /// <summary>
        //        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合true
        //        /// </summary>
        //        /// <param name="a"></param>
        //        /// <param name="b"></param>
        //        /// <returns></returns>
        //        public static bool operator ==(DungeonMapIndex a, DungeonMapIndex b)
        //        {

        //            // x,y,z全ての要素が一致していた場合true
        //            if (a.x == b.x & a.y == b.y && a.z == b.z)
        //            {
        //                return true;
        //            }
        //            // でなければfalse
        //            return false;
        //        }

        //        /// <summary>
        //        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合false
        //        /// </summary>
        //        /// <param name="a"></param>
        //        /// <param name="b"></param>
        //        /// <returns></returns>
        //        public static bool operator !=(DungeonMapIndex a, DungeonMapIndex b)
        //        {
        //            return !(a == b);
        //        }
        //    }

        //    // 掘削方向
        //    public enum Direction
        //    {
        //        Right,
        //        Left,
        //        Top,
        //        Bottom,
        //        Forward,
        //        Backward,
        //        None,
        //    }

        //    /// <summary>
        //    /// そのマスのステータス
        //    /// </summary>

        //    public enum CellStatus
        //    {
        //        Road,
        //        DigableWall,
        //        UndigableWall,
        //        Goal,
        //        Start,
    }
}
