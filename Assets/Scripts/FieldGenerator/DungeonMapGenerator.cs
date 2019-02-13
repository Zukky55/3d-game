using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{

    /// <summary>
    /// 3D迷路の自動生成
    /// </summary>
    public class DungeonMapGenerator : MonoBehaviour
    {
        /// <summary>DungeonMapの状態を表す三次元配列[x,y,z]</summary>
        public DMCompont[,,] DungeonMap { get; private set; }
        /// <summary>DungeonMapの各要素</summary>
        public List<DMObject> DMComponents { get; private set; }
        /// <summary>DungeonMapのオブジェクト群</summary>
        public List<GameObject> DungeonMapObjects { get; private set; }
        /// <summary>道の偶数地点</summary>
        public List<Vector3> RoadEvenPoint { get; private set; }
        /// <summary>開始地点</summary>
        public Vector3 StartPoint { get; private set; }
        /// <summary>ゴール地点</summary>
        public Vector3 GoalPoint { get; private set; }

        /// <summary>幅</summary>
        [SerializeField] int m_width;
        /// <summary>高さ</summary>
        [SerializeField] int m_height;
        /// <summary>奥行</summary>
        [SerializeField] int m_depth;
        /// <summary>コライダーを必要とするかどうか</summary>
        [SerializeField] bool m_needToCollider;
        [SerializeField] float m_mapSize = 1f;
        /// <summary>ゴール地点を設定しているかどうか</summary>
        bool m_isGoalSet;

        void OnEnable()
        {
            DungeonMap = new DMCompont[m_width, m_height, m_depth];
            DMComponents = new List<DMObject>();
            RoadEvenPoint = new List<Vector3>();

            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);
        }

        /// <summary>
        /// マップ配列の各要素に役割を割り当てる
        /// </summary>
        public void Setup()
        {
            // 各面において両端の場合は掘削不可能な壁,それ以外は掘削可能な壁とする
            for (int y = 0; y < m_height; y++)
            {
                for (int z = 0; z < m_depth; z++)
                {
                    for (int x = 0; x < m_width; x++)
                    {
                        if (y == 0 | y == m_height - 1
                            | z == 0 | z == m_depth - 1
                            | x == 0 | x == m_width - 1)
                        {
                            DungeonMap[x, y, z].Status = CellStatus.UndigableWall;
                        }
                        else
                        {
                            DungeonMap[x, y, z].Status = CellStatus.DigableWall;
                        }
                        DungeonMap[x,y,z].Position = new Vector3(x, y, z);


                    }
                }
            }

            // 最初のポイントを作成
            GetFirstPoint();
            // 再帰処理で道の作成を開始
            DiggingInRecursively(StartPoint);
        }

        void CreateMapObjects()
        {

        }

        /// <summary>
        /// 迷路の開始地点を決める. 開始地点は外周以外の偶数を選択
        /// </summary>
        void GetFirstPoint()
        {
            StartPoint = new Vector3(GetRandomEvenPoint(1, m_width - 2),
                GetRandomEvenPoint(1, m_height - 2),
                GetRandomEvenPoint(1, m_depth - 2));
            DungeonMap[(int)StartPoint.x, (int)StartPoint.y, (int)StartPoint.z].Status = CellStatus.Start;
            RoadEvenPoint.Add(StartPoint);
        }

        /// <summary>
        /// 再帰呼び出しで道を掘る
        /// </summary>
        /// <param name="startPointIndex"></param>
        public void DiggingInRecursively(Vector3 startPointIndex)
        {
            // 掘る方向
            Direction dir = GetDirection(startPointIndex);
            // 掘る2マス先のポイントとその中間地点
            Vector3 target, subTarget;
            subTarget = target = new Vector3(startPointIndex.x, startPointIndex.y, startPointIndex.z);

            switch (dir)
            {
                case Direction.Right:
                    target.x += 2;
                    subTarget.x += 1;
                    break;
                case Direction.Left:
                    target.x -= 2;
                    subTarget.x -= 1;
                    break;
                case Direction.Top:
                    target.y += 2;
                    subTarget.y += 1;
                    break;
                case Direction.Bottom:
                    target.y -= 2;
                    subTarget.y -= 1;
                    break;
                case Direction.Forward:
                    target.z += 2;
                    subTarget.z += 1;
                    break;
                case Direction.Backward:
                    target.z -= 2;
                    subTarget.z -= 1;
                    break;
                case Direction.None:
                    // もう掘れない場合は自分自身を掘削リストから削除し、既存のポイントから掘削を再開する
                    // 最初に壁に当たった一回目をゴールとする
                    if (!m_isGoalSet)
                    {
                        m_isGoalSet = false;
                        GoalPoint = startPointIndex;
                    }
                    RoadEvenPoint.Remove(startPointIndex);

                    if (RoadEvenPoint.Count > 0)
                    {
                        // 再帰呼び出し
                        DiggingInRecursively(RoadEvenPoint[GetRandomEvenPoint(0, RoadEvenPoint.Count)]);
                    }
                    // 掘削ポイントがもうない場合は終了する
                    return;
            }
            // 掘削リストに追加
            RoadEvenPoint.Add(target);
            // 対象と中間を掘ってその座標を登録
            DungeonMap[(int)target.x, (int)target.y, (int)target.z].Status = CellStatus.Road;
            DungeonMap[(int)subTarget.x, (int)subTarget.y, (int)subTarget.z].Status = CellStatus.Road;
            // 掘った先を中心に再帰呼び出し
            DiggingInRecursively(target);
        }



        /// <summary>
        /// 範囲内でランダムな偶数値を取得
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        int GetRandomEvenPoint(int min, int max)
        {
            int evenNum = 1;
            while (evenNum % 2 == 1)
            {
                evenNum = Random.Range(min, max);
            }
            return evenNum;
        }

        /// <summary>
        /// Pointを起点に周囲が既に道になっていないか調べる
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        Direction GetDirection(Vector3 point)
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
                var checkDir = dir[Random.Range(0, dir.Count - 1)];
                var checkPoint = new Vector3();

                checkPoint = point;
                switch (checkDir) // その方向の2つ先のマスをcheckPointに指定
                {
                    case Direction.Right:
                        checkPoint.x += 2;
                        break;
                    case Direction.Left:
                        checkPoint.x -= 2;
                        break;
                    case Direction.Top:
                        checkPoint.y += 2;
                        break;
                    case Direction.Bottom:
                        checkPoint.y -= -2;
                        break;
                    case Direction.Forward:
                        checkPoint.z += 2;
                        break;
                    case Direction.Backward:
                        checkPoint.z -= 2;
                        break;
                    default:
                        break;
                }
                if (DungeonMap[(int)checkPoint.x, (int)checkPoint.y, (int)checkPoint.z].Status == CellStatus.DigableWall) // 対象のマスのステータスが掘削可能ならその向きを返す
                {
                    return checkDir;
                }
                // 対象の方向が掘削不可能だった場合その方向を削除してもう一度チェックする
                dir.Remove(checkDir);
            }
            // 全方向掘削不可能だった場合Noneを返す
            return Direction.None;
        }
    }

    public struct DMCompont
    {
        public CellStatus Status { get; set; }
        public Vector3 Position { get; set; }
    }

    // 掘削方向
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
    /// そのマスのステータス
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
