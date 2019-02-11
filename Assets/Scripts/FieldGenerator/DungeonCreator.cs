using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{

    /// <summary>
    /// MineClaftの様な3Dマップ自動生成をやってみせる
    /// </summary>
    public class DungeonCreator : MonoBehaviour
    {
        public LoadStatus[,,] DungeonMap { get; set; }
        public List<MapPoint> LoadEvenPoint { get; set; }
        public MapPoint StartPoint { get; set; }
        public MapPoint GoalPoint { get; set; }
        private int sizeX { get; set; }
        private int sizeY { get; set; }
        private int sizeZ { get; set; }
        private bool isGoalSet { get; set; }

        public DungeonCreator(int x, int y, int z)
        {
            sizeX = x + 2;
            sizeY = y + 2;
            sizeZ = z + 2;
            DungeonMap = new LoadStatus[sizeX, sizeY, sizeZ];
            LoadEvenPoint = new List<MapPoint>();
            isGoalSet = false;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // 配列全てを壁に設定
            for (int i = 0; i < sizeZ; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    for (int k = 0; k < sizeY; k++)
                    {
                        DungeonMap[j, k, i] = LoadStatus.Wall;
                    }
                }

                // 外周を道に設定
                // 左辺
                for (int j = 0; j < sizeY; j++)
                {
                    DungeonMap[0, j, i] = LoadStatus.Road;

                }


            }
        }
    }

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

    public enum LoadDirection
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
        Up,
        Down,
    }

    public enum LoadStatus
    {
        Road = 1,
        Wall,
    }
}
