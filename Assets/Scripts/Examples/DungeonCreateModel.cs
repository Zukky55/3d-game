using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonCreateModel
{
    public LoadStatus[,] DungeonMap { get; set; }
    public List<MapPoint> LoadEvenPoint { get; set; }
    public MapPoint StartPoint { get; set; }
    public MapPoint GoalPoint { get; set; }
    private System.Random rnd { get; set; }
    private int SizeX { get; set; }
    private int SizeY { get; set; }
    private bool isGoalSet { get; set; }
    /// <summary>
    /// 迷路のサイズを指定するコンストラクタ
    /// </summary>
    /// <param name="x">X方向の大きさ</param>
    /// <param name="y">Y方向の大きさ</param>
    public DungeonCreateModel(int x, int y)
    {
        //マップとなる2次元配列を作る
        //2多くして最外周をつくる
        SizeX = x + 2;
        SizeY = y + 2;
        DungeonMap = new LoadStatus[SizeX, SizeY];
        LoadEvenPoint = new List<MapPoint>();
        isGoalSet = false;
        //乱数の生成
        rnd = new System.Random((int)(DateTime.Now.Ticks % Int32.MaxValue));
    }
    /// <summary>
    /// 迷路作成のセットアップ処理
    /// </summary>
    public void Setup()
    {
        //配列すべてを壁に設定
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                DungeonMap[i, j] = LoadStatus.Wall;
            }
        }
        //外周を道に設定
        //左辺
        for (int j = 0; j < SizeY; j++)
        {
            DungeonMap[0, j] = LoadStatus.Load;
        }
        //右辺
        for (int j = 0; j < SizeY; j++)
        {
            DungeonMap[SizeX - 1, j] = LoadStatus.Load;
        }
        //上辺
        for (int i = 0; i < SizeX; i++)
        {
            DungeonMap[i, 0] = LoadStatus.Load;
        }
        //下辺
        for (int i = 0; i < SizeX; i++)
        {
            DungeonMap[i, SizeY - 1] = LoadStatus.Load;
        }
        //最初のポイントを作成
        GetFirstPoint();
        //再帰処理で道の作成を開始
        RecursiveLoad(StartPoint);
    }
    /// <summary>
    /// 再帰呼び出しで道を作る
    /// </summary>
    /// <param name="startPointIndex">掘削の起点ポイント</param>
    public void RecursiveLoad(MapPoint startPointIndex)
    {
        //掘る方向を決める
        LoadDirection dir = GetDirection(startPointIndex);
        //掘る2マス先のポイントと中間地点を格納する変数
        MapPoint target, subTarget;

        //2マス先と中間マスを取得する
        switch (dir)
        {
            case LoadDirection.Top:
                target = new MapPoint(startPointIndex.X, startPointIndex.Y - 2);
                subTarget = new MapPoint(startPointIndex.X, startPointIndex.Y - 1);
                break;
            case LoadDirection.Bottom:
                target = new MapPoint(startPointIndex.X, startPointIndex.Y + 2);
                subTarget = new MapPoint(startPointIndex.X, startPointIndex.Y + 1);
                break;
            case LoadDirection.Left:
                target = new MapPoint(startPointIndex.X - 2, startPointIndex.Y);
                subTarget = new MapPoint(startPointIndex.X - 1, startPointIndex.Y);
                break;
            case LoadDirection.Right:
                target = new MapPoint(startPointIndex.X + 2, startPointIndex.Y);
                subTarget = new MapPoint(startPointIndex.X + 1, startPointIndex.Y);
                break;
            default:
                //もう掘れない場合は自分を掘削リストから削除して
                //既存のポイントから掘削を再開する
                //最初の一回目をゴールとする
                if (isGoalSet == false)
                {
                    isGoalSet = true;
                    GoalPoint = new MapPoint(startPointIndex.X, startPointIndex.Y);
                }
                LoadEvenPoint.Remove(startPointIndex);
                if (LoadEvenPoint.Count > 0)
                {
                    RecursiveLoad(LoadEvenPoint[rnd.Next(LoadEvenPoint.Count)]);
                }
                //掘削ポイントがもうない場合は終了する
                return;
                break;
        }
        //掘る先を掘削リストに追加
        LoadEvenPoint.Add(target);
        //対象と中間を掘る
        DungeonMap[target.X, target.Y] = LoadStatus.Load;
        DungeonMap[subTarget.X, subTarget.Y] = LoadStatus.Load;
        //堀った先を中心に再帰
        RecursiveLoad(target);
    }
    /// <summary>
    /// 迷路の開始地点を決める
    /// </summary>
    /// <remarks>
    /// 開始地点は外周以外の偶数を選択
    /// </remarks>
    private void GetFirstPoint()
    {
        StartPoint = new MapPoint(GetRandomEvenPoint(2, SizeX - 3),
                        GetRandomEvenPoint(2, SizeY - 3));
        DungeonMap[StartPoint.X, StartPoint.Y] = LoadStatus.Load;
        LoadEvenPoint.Add(StartPoint);
    }
    /// <summary>
    /// ランダムで範囲内の偶数を取得
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private int GetRandomEvenPoint(int min, int max)
    {
        int val = 1;
        while (val % 2 == 1)
        {
            val = rnd.Next(min, max);
        }
        return val;
    }
    /// <summary>
    /// ポイントを起点に上下左右がすでに道化していないか調べる
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private LoadDirection GetDirection(MapPoint point)
    {
        List<LoadDirection> dir = new List<LoadDirection>();
        dir.Add(LoadDirection.Top);
        dir.Add(LoadDirection.Bottom);
        dir.Add(LoadDirection.Left);
        dir.Add(LoadDirection.Right);
        //上下左右4回チェック
        while (dir.Count > 0)
        {
            //調べる方向をランダム取得
            LoadDirection checkDir = dir[rnd.Next(dir.Count - 1)];
            //実際に調べるポイントを宣言
            MapPoint checkPoint = new MapPoint();
            //2マス先がまだ壁だったらその方向を返す
            switch (checkDir)
            {
                case LoadDirection.Top:
                    checkPoint.X = point.X;
                    checkPoint.Y = point.Y - 2;
                    break;
                case LoadDirection.Bottom:
                    checkPoint.X = point.X;
                    checkPoint.Y = point.Y + 2;
                    break;
                case LoadDirection.Left:
                    checkPoint.X = point.X - 2;
                    checkPoint.Y = point.Y;
                    break;
                case LoadDirection.Right:
                    checkPoint.X = point.X + 2;
                    checkPoint.Y = point.Y;
                    break;
                default:
                    break;
            }
            if (DungeonMap[checkPoint.X, checkPoint.Y] == LoadStatus.Wall)
            {
                return checkDir;
            }
            //対象の方向が道だったらその方向を削除してもう一度
            dir.Remove(checkDir);
        }
        //全方向進めなかった場合
        return LoadDirection.None;
    }

    public struct MapPoint
    {
        public int X;
        public int Y;

        public MapPoint(int x,int y)
        {
            X = x;
            Y = y;
        }
    }

    public enum LoadDirection
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
    }

    public enum LoadStatus
    {
        Load = 1,
        Wall,
    }
}
