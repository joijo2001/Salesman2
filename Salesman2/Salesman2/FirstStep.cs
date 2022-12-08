using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Salesman2;
using System.Reflection;

namespace Saleman1
{
    public class Program1
    {
        static int iterations;
        static int record;
        static Dictionary<int, int> recordPath;
        static int[,] sourcePrices;
        public void Starts()
        {

            string[] lines = File.ReadAllLines("kom.in");
            record = int.MaxValue;

            //n городов для посещения
            int n = Int32.Parse(lines[0]);

            // Матрица стоимостей проезда
            sourcePrices = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                var matrixLine = new int[n];
                matrixLine = lines[i + 1].Split(' ').Select(int.Parse).ToArray();
                for (int j = 0; j < n; j++)
                {
                    sourcePrices[i, j] = matrixLine[j];
                    if (matrixLine[j] == -1)        // Переменные = -1 заменяются на максимальные значения
                        sourcePrices[i, j] = int.MaxValue;
                }
            }

            var prices = (int[,])sourcePrices.Clone(); // Клонируем матрицу стоимостей

            //для поиска клетки, куда надо поставить бесконечность
            iterations = 0;

            // алгоритм Литтла
            handleMatrix(prices, new Dictionary<int, int>(), 0);

            Write();
        }

        // алгоритм Литтла
        static void handleMatrix(int[,] prices, Dictionary<int, int> path, int bottomLimit)
        {
            // проверка на конец решения
            var finalPoints = new List<int>();
            int num = 0;
            for (int i = 0; i < prices.GetLength(0); i++)
                for (int j = 0; j < prices.GetLength(0); j++)
                {
                    if (prices[i, j] != int.MaxValue)
                    {
                        num++;
                        finalPoints.Add(prices.GetLength(0) * i + j);
                    }
                }

            // если матрица 2x2
            if (num == 2)
            {
                var result = new Dictionary<int, int>(path);

                var row1 = finalPoints[0] / prices.GetLength(0);
                var col1 = finalPoints[0] - prices.GetLength(0) * row1;

                var row2 = finalPoints[1] / prices.GetLength(0);
                var col2 = finalPoints[1] - prices.GetLength(0) * row2;

                result.Add(row2, col2);
                result.Add(row1, col1);

                // сравнение пути с минимальным
                candidateSolution(result);

                return;
            }

            // сумма всех вычтенных значений
            int subtractSum = 0;

            // массивы с минимальными элементами строк и столбцов
            var minRow = new int[prices.GetLength(0)];
            var minColumn = new int[prices.GetLength(0)];

            for (int k = 0; k < prices.GetLength(0); k++)
            {
                minRow[k] = int.MaxValue;
                minColumn[k] = int.MaxValue;
            }

            // обход всей матрицы
            for (int i = 0; i < prices.GetLength(0); i++)
            {
                // поиск минимального элемента в строке
                for (int j = 0; j < prices.GetLength(0); j++)
                    if (prices[i, j] < minRow[i])
                        minRow[i] = prices[i, j];

                for (int j = 0; j < prices.GetLength(0); ++j)
                {
                    // вычитание минимальных элементов из всех
                    // элементов строки, кроме бесконечностей
                    if (prices[i, j] < int.MaxValue)
                        prices[i, j] -= minRow[i];

                    // поиск минимального элемента в столбце после вычитания строк
                    if ((prices[i, j] < minColumn[j]))
                        minColumn[j] = prices[i, j];
                }
            }

            // вычитание минимальных элементов из всех
            // элементов столбца, кроме бесконечностей
            for (int j = 0; j < prices.GetLength(0); ++j)
                for (int i = 0; i < prices.GetLength(0); i++)
                    if (prices[i, j] < int.MaxValue)
                        prices[i, j] -= minColumn[j];

            // суммирование вычтенных значений
            foreach (var min in minRow)
                if (min < int.MaxValue)
                    subtractSum += min;

            foreach (var min in minColumn)
                if (min < int.MaxValue)
                    subtractSum += min;

            // вычитание минимальных элементов строк и столбцов
            // увеличение нижней границы
            bottomLimit += subtractSum;

            // сравнение верхней и нижней границ
            if (bottomLimit >= record)
                return;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Finder fbz = new Finder();
            //находим элемент с наибольшей нулевой степенью
            List<int> zeros = Finder.findBestZeros(prices, null);

            if (zeros.Count == 0)
                return;

            // новая матрица
            int[,] newMatrix = (int[,])prices.Clone();

            int row = zeros[0] / prices.GetLength(0); // Строка с максимальным коэф.
            int col = zeros[0] - prices.GetLength(0) * row; // Столбик с макс. коэф.
            zeros.RemoveAt(0);

            // из матрицы удаляются строка и столбец, соответствующие вершинам ребра
            for (int j = 0; j < prices.GetLength(0); j++)
                newMatrix[row, j] = int.MaxValue;

            for (int i = 0; i < prices.GetLength(0); i++)
                newMatrix[i, col] = int.MaxValue;

            iterations++;

            // не допускаем образование цикла
            // массивы с информацией о том, в каких столбцах и строках содержится бесконечность
            var infRow = new int[prices.GetLength(0)];
            var infColumn = new int[prices.GetLength(0)];

            // обход всей матрицы, нахождение кол-ва бесконечностей
            for (int i = 0; i < prices.GetLength(0); i++)
                for (int j = 0; j < prices.GetLength(0); j++)
                {
                    if (newMatrix[i, j] == int.MaxValue)
                    {
                        infRow[i]++;
                        infColumn[j]++;
                    }
                }

            int r = 0; int c = 0;
            for (int k = 0; k < prices.GetLength(0); k++)
            {
                if (infRow[k] == iterations)
                    r = k;
                if (infColumn[k] == iterations)
                    c = k;
            }
            newMatrix[r, c] = int.MaxValue;

            //добавление в путь ребра
            Dictionary<int, int> newPath = new Dictionary<int, int>(path);
            newPath.Add(row, col);

            // обработка множества, содержащего ребро edge
            handleMatrix(newMatrix, newPath, bottomLimit);

            // переход к множеству, не соержащему ребро edge
            // снова копирование матрицы текущего шага

            Array.Copy(prices, newMatrix, prices.Length);

            // добавление бесконечности на место ребра
            newMatrix[row, col] = int.MaxValue;
            // обработка множества, не сожержащего ребро edge
            iterations--;
            handleMatrix(newMatrix, path, bottomLimit);
        }

        static void candidateSolution(Dictionary<int, int> result)
        {
            int curCost = GetCost(result, null);

            // сравнение рекорда со стоимостью текущего пути
            if (record < curCost)
                return;

            // копирование стоимости и пути
            record = curCost;
            recordPath = new Dictionary<int, int>(result);
        }

        public static int GetCost(Dictionary<int, int> result, int[,] testSources)
        {
            int cost = 0;
            foreach (var edge in result)
            {
                if (testSources == null)
                    cost += sourcePrices[edge.Key, edge.Value];
                else
                    cost += testSources[edge.Key, edge.Value];
            }

            return cost;
        }

        static void Write()
        {
            StreamWriter f = new StreamWriter("kom.out");
            f.WriteLine(record);

            int startPoint = 0;
            while (true)
            {
                f.WriteLine((startPoint + 1) + " " + (recordPath[startPoint] + 1));

                // переходим в следующую вершину
                startPoint = recordPath[startPoint];

                // если вернулись в начало
                if (startPoint == 0)
                    break;
            }
            f.Close();
        }
    }
}