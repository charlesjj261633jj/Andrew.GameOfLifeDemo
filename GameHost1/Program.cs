﻿using System;
using System.Threading;

namespace GameHost1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] matrix = new bool[50, 50];
            bool[,] area = new bool[3, 3];

            Init(matrix);

            for (int count = 0; count < 5000; count++)
            {
                int live_count = 0;
                Thread.Sleep(200);

                Console.SetCursorPosition(0, 0);
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    for (int x = 0; x < matrix.GetLength(1); x++)
                    {
                        // clone area
                        for (int ay = 0; ay < 3; ay++)
                        {
                            for (int ax = 0; ax < 3; ax++)
                            {
                                int cx = x - 1 + ax;
                                int cy = y - 1 + ay;

                                if (cx < 0) area[ax, ay] = false;
                                else if (cy < 0) area[ax, ay] = false;
                                else if (cx >= matrix.GetLength(1)) area[ax, ay] = false;
                                else if (cy >= matrix.GetLength(0)) area[ax, ay] = false;
                                else area[ax, ay] = matrix[cx, cy];
                            }
                        }
                        matrix[x, y] = TimePassRule(area);
                        Console.Write(matrix[x, y]? '★' : '☆');
                        if (matrix[x, y]) live_count++;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"total lives: {live_count}, round: {count} / 5000...");
            }
        }


        static void Init(bool[,] matrix)
        {
            Random rnd = new Random();
            int rate = 20;

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    matrix[x, y] = (rnd.Next(100) < rate);
                }
            }
        }


        /// <summary>
        /// 1. 每個細胞有兩種狀態 - 存活或死亡，每個細胞與以自身為中心的周圍八格細胞產生互動（如圖，黑色為存活，白色為死亡）
        /// 2. 當前細胞為存活狀態時，當周圍的存活細胞低於2個時（不包含2個），該細胞變成死亡狀態。（模擬生命數量稀少）
        /// 3. 當前細胞為存活狀態時，當周圍有2個或3個存活細胞時，該細胞保持原樣。
        /// 4. 當前細胞為存活狀態時，當周圍有超過3個存活細胞時，該細胞變成死亡狀態。（模擬生命數量過多）
        /// 5. 當前細胞為死亡狀態時，當周圍有3個存活細胞時，該細胞變成存活狀態。（模擬繁殖）
        /// 6. 可以把最初的細胞結構定義為種子，當所有在種子中的細胞同時被以上規則處理後，可以得到第一代細胞圖。按規則繼續處理當前的細胞圖，可以得到下一代的細胞圖，周而復始。 
        /// </summary>
        /// <param name="area">must be bool[3, 3]</param>
        /// <returns></returns>
        static bool TimePassRule(bool[,] area)
        {
            var center = area[1, 1];

            int alive = 0;

            for (int i = 0; i < area.GetLength(0); i++)
                for (int k = 0; k < area.GetLength(1); k++)
                    if (area[i, k]) alive++;

            if (center)
            {
                alive--;
                if (alive < 2 || alive > 3)
                    center = !center;
            }
            else
            {
                if (alive == 3)
                    center = !center;
            }
            return center;
        }
    }
}
