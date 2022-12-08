using Salesman2;
using Saleman1;
using NUnit.Compatibility;

namespace TestProject1
{
    public class Tests : Program2
    {
        [SetUp]
        public void Setup()
        {

        }
        // Блочный тест для функции GetCoefficient (нахождение степени нуля)
        [Test]
        public void GetCoefficientTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 0 },
                                  { 0, int.MaxValue, 0 },
                                  { 0, 4, int.MaxValue } };

            int expected = 4; // Ожидаемое значение для ячейки [0,1]

            int actual = Program2.getCoefficient(testArray, 0, 1);

            Assert.AreEqual(expected, actual);
        }

        // Блочный тест для функции GetCost (нахождение длины пути)
        [Test]
        public void GetCostTest()
        {
            var testDict = new Dictionary<int, int>()
            {
                {0, 1},
                {1, 2},
                {2, 0}
            };

            int[,] testSources = { {int.MaxValue, 3, 4},
                                   {2, int.MaxValue, 3},
                                   {1, 5, int.MaxValue} };

            int expected = 7; // Ожидаемое значение

            int actual = Program1.GetCost(testDict, testSources);

            Assert.AreEqual(expected, actual);
        }


        // Интеграционный тест для функции FindBestZeros (нахождение номеров ячеек с наибольшей степенью нуля)
        [Test]
        public void FindBestZerosTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 0 },
                                  { 0, int.MaxValue, 0 },
                                  { 0, 4, int.MaxValue } };

            List<int> expectedList = new List<int>() { 1, 6 }; // Что ожидаем на выходе
            List<int> coeffList = new List<int>(); // Лист для подстановки значений для внутренней функции


            for (int i = 0; i < testArray.GetLength(0); i++)
                for (int j = 0; j < testArray.GetLength(0); j++)
                {
                    if (testArray[i, j] == 0)
                        coeffList.Add(Program2.getCoefficient(testArray, i, j)); // Вызов внутренней функции
                }

            List<int> actualList2 = Finder.findBestZeros(testArray, coeffList); // Вызов главной функции

            CollectionAssert.AreEqual(expectedList, actualList2); // Сравнение результатов
        }


        // Негативный тест для функции GetCoefficient (нахождение степени нуля)
        [Test]
        public void GetCoefficientNegativeTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 1 },
                                  { 0, int.MaxValue, 2 },
                                  { 3, 0, int.MaxValue } };

            int unexpected = int.MaxValue; // неверное значение для ячейки [1,0]

            int actual = Program2.getCoefficient(testArray, 1, 0);

            Assert.AreNotEqual(unexpected, actual);
        }


        // Негативный тест для функции GetCost (нахождение длины пути)
        [Test]
        public void GetCostNegativeTest()
        {
            var testDict = new Dictionary<int, int>()
            {
                {0, 1},
                {2, 0},
                {1, 2}
            };

            int[,] testSources = { {int.MaxValue, 3, 4},
                                   {2, int.MaxValue, 3},
                                   {1, 5, int.MaxValue} };

            int unexpected = -1; // неверное значение

            int actual = Program1.GetCost(testDict, testSources);

            Assert.AreNotEqual(unexpected, actual);
        }


        // Негативный тест для функции FindBestZeros (нахождение номеров ячеек с наибольшей степенью нуля)
        [Test]
        public void FindBestZerosNegativeTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 0 },
                                  { 0, int.MaxValue, 0 },
                                  { 0, 4, int.MaxValue } };

            List<int> expectedList = new List<int>() { -1, 6 }; // Что точно не ожидаем на выходе
            List<int> coeffList = new List<int>(); // Лист для подстановки значений для внутренней функции


            for (int i = 0; i < testArray.GetLength(0); i++)
                for (int j = 0; j < testArray.GetLength(0); j++)
                {
                    if (testArray[i, j] == 0)
                        coeffList.Add(Program2.getCoefficient(testArray, i, j)); // Вызов внутренней функции
                }

            List<int> actualList2 = Finder.findBestZeros(testArray, coeffList); // Вызов главной функции

            CollectionAssert.AreNotEqual(expectedList, actualList2); // Сравнение результатов
        }
    }
}