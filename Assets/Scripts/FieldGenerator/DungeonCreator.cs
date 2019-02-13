using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{

    /// <summary>
    /// 3D迷路の自動生成
    /// </summary>
    public class DungeonCreator : MonoBehaviour
    {
        /// <summary>mapの配列[x,y,z]</summary>
        public Status[,,] DungeonMap { get; }
        /// <summary></summary>
        public List<MapPoint> LoadEvenPoint { get; set; }
        /// <summary></summary>
        public MapPoint StartPoint { get; set; }
        /// <summary></summary>
        public MapPoint GoalPoint { get; set; }
        /// <summary>列</summary>
        private int column { get; set; }
        /// <summary>行</summary>
        private int row { get; set; }
        /// <summary>奥行</summary>
        private int depth { get; set; }
        /// <summary></summary>
        private bool isGoalSet { get; set; }

        public DungeonCreator(int x, int y, int z)
        {
            column = x + 2;
            row = y + 2;
            depth = z + 2;
            DungeonMap = new Status[column, row, depth];
            LoadEvenPoint = new List<MapPoint>();
            isGoalSet = false;
        }

        /// <summary>
        /// マップ配列の各要素に役割を割り当てる
        /// </summary>
        public void Setup()
        {
            // 配列全てを壁に設定
            // z軸
            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    for (int k = 0; k < row; k++)
                    {
                        DungeonMap[k, j, i] = Status.Wall;
                    }
                }
                // 外周を道に設定
                // x軸
                for (int k = 0; k < row; k++)
                {
                    DungeonMap[k, 0, i] = Status.Road;
                    DungeonMap[k, column - 1, i] = Status.Road;
                }
                // y軸
                for (int j = 0; j < column; j++)
                {
                    DungeonMap[0, j, i] = Status.Road;
                    DungeonMap[row - 1, j, i] = Status.Road;
                }
            }
            // 最初のポイントを作成
            GetFirstPoint();
            // 再帰処理で道の作成を開始
            RecursiveLoad(StartPoint);
        }


        /// <summary>
        /// 再帰呼び出しで道を掘る
        /// </summary>
        /// <param name="startPointIndex"></param>
        public void RecursiveLoad(MapPoint startPointIndex)
        {
            // 掘る方向
            LoadDirection dir = GetDirection(startPointIndex);
            // 掘る2マス先のポイントとその中間地点
            MapPoint target, subTarget;
            subTarget = target = new MapPoint(startPointIndex.x, startPointIndex.y, startPointIndex.z);

            switch (dir)
            {
                case LoadDirection.Right:
                    target.x += 2;
                    subTarget.x += 1;
                    break;
                case LoadDirection.Left:
                    target.x -= 2;
                    subTarget.x -= 1;
                    break;
                case LoadDirection.Top:
                    target.y += 2;
                    subTarget.y += 1;
                    break;
                case LoadDirection.Bottom:
                    target.y -= 2;
                    subTarget.y -= 1;
                    break;
                case LoadDirection.Forward:
                    target.z += 2;
                    subTarget.z += 1;
                    break;
                case LoadDirection.Backward:
                    target.z -= 2;
                    subTarget.z -= 1;
                    break;
                case LoadDirection.None:
                    // もう掘れない場合は自分自身を掘削リストから削除し、既存のポイントから掘削を再開する
                    // 最初に壁に当たった一回目をゴールとする
                    if(!isGoalSet)
                    {
                        isGoalSet = false;
                        GoalPoint = startPointIndex;
                    }
                    LoadEvenPoint.Remove(startPointIndex);

                    if(LoadEvenPoint.Count > 0)
                    {
                        // 再帰呼び出し
                        RecursiveLoad(LoadEvenPoint[GetRandomEvenPoint(0, LoadEvenPoint.Count)]);
                    }
                    // 掘削ポイントがもうない場合は終了する
                    return;
            }
            // 掘削リストに追加
            LoadEvenPoint.Add(target);
            // 対象と中間を掘る
            DungeonMap[target.x, target.y, target.z] = Status.Road;
            DungeonMap[subTarget.x, subTarget.y, subTarget.z] = Status.Road;
            // 掘った先を中心に再帰呼び出し
            RecursiveLoad(target);
        }

        /// <summary>
        /// 迷路の開始地点を決める. 開始地点は外周以外の偶数を選択
        /// </summary>
        void GetFirstPoint()
        {
            StartPoint = new MapPoint(GetRandomEvenPoint(2, row - 3),
                GetRandomEvenPoint(2, column - 3),
                GetRandomEvenPoint(2, depth - 3));
            DungeonMap[StartPoint.x, StartPoint.y, StartPoint.z] = Status.Road;
            LoadEvenPoint.Add(StartPoint);
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
        LoadDirection GetDirection(MapPoint point)
        {
            List<LoadDirection> dir = new List<LoadDirection>();
            dir.Add(LoadDirection.Forward);
            dir.Add(LoadDirection.Backward);
            dir.Add(LoadDirection.Right);
            dir.Add(LoadDirection.Left);
            dir.Add(LoadDirection.Top);
            dir.Add(LoadDirection.Bottom);

            while (dir.Count > 0)
            {
                var checkDir = dir[Random.Range(0, dir.Count - 1)];
                var checkPoint = new MapPoint();

                checkPoint = point;
                switch (checkDir)
                {
                    case LoadDirection.Right:
                        checkPoint.x += 2;
                        break;
                    case LoadDirection.Left:
                        checkPoint.x -= 2;
                        break;
                    case LoadDirection.Top:
                        checkPoint.y += 2;
                        break;
                    case LoadDirection.Bottom:
                        checkPoint.y -= -2;
                        break;
                    case LoadDirection.Forward:
                        checkPoint.z += 2;
                        break;
                    case LoadDirection.Backward:
                        checkPoint.z -= 2;
                        break;
                    default:
                        break;
                }
                if(DungeonMap[checkPoint.x,checkPoint.y,checkPoint.z] == Status.Wall)
                {
                    return checkDir;
                }
                // 対象の方向が道だった場合その方向を削除してもう一度チェックする
                dir.Remove(checkDir);
            }
            // 全方向進めなかった場合
            return LoadDirection.None;
        }
    }

    /// <summary>
    /// Mapの座標
    /// </summary>
    public struct MapPoint
    {
        public int x;
        public int y;
        public int z;

        public MapPoint(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    // 読み込む向き
    public enum LoadDirection
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
    public enum Status
    {
        Road = 1,
        Wall,
    }
}
