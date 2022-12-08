using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Salesman2
{
    public class Program2
    {
        protected static int getCoefficient(int[,] prices, int r, int c)
        {
            // расчет коэффициентов
            int rmin, cmin;
            rmin = cmin = int.MaxValue;
            // обход строки и столбца
            for (int i = 0; i < prices.GetLength(0); ++i)
            {
                if (i != r)
                    rmin = Math.Min(rmin, prices[i, c]);

                if (i != c)
                    cmin = Math.Min(cmin, prices[r, i]);
            }

            return rmin + cmin;
        }
    }

    public class Finder : Program2
    {
        public static List<int> findBestZeros(int[,] prices, List<int> TestList)
        {
            // список координат нулевых элементов
            List<int> zeros = new List<int>();

            // список их коэффициентов
            List<int> coeffList = new List<int>();

            // максимальный коэффициент
            double maxCoeff = 0;

            if (TestList != null)
                coeffList = TestList;

            // поиск нулевых элементов
            for (int i = 0; i < prices.GetLength(0); ++i)
            {
                for (int j = 0; j < prices.GetLength(0); ++j)
                    // если равен нулю
                    if (prices[i, j] == 0)
                    {
                        // добавление в список координат
                        zeros.Add(prices.GetLength(0) * i + j);

                        // расчет коэффициента и добавление в список
                        if (TestList == null) // Заглушка для интеграционного тестирования
                            coeffList.Add(getCoefficient(prices, i, j));

                        // сравнение с максимальным
                        //maxCoeff = Math.Max(maxCoeff, coeffList[coeffList.Count - 1]);
                    }
            }

            maxCoeff = coeffList.Max();

            int k = 0;
            while (k < coeffList.Count)
            {
                if (coeffList[k] != maxCoeff)
                {
                    coeffList.RemoveAt(k);
                    zeros.RemoveAt(k);
                }
                else
                {
                    k++;
                }
            }

            return zeros;
        }
    }
}