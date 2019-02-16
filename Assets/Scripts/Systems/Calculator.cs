using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReviewGames
{
    public static class Calculator
    {
        /// <summary>
        /// 0f~1f間を往復する値
        /// </summary>
        public static float WaveValue
        {
            get
            {
                return Mathf.Sin(Time.realtimeSinceStartup * 1f);
            }
        }
        /// <summary>
        /// 範囲内でランダムな偶数値を取得
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomEvenIndex(int min, int max)
        {
            int evenNum = 1;
            while (evenNum % 2 == 1)
            {
                evenNum = Random.Range(min, max);
            }
            return evenNum;
        }
    }
}