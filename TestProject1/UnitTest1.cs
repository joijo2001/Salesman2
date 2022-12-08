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
        // ������� ���� ��� ������� GetCoefficient (���������� ������� ����)
        [Test]
        public void GetCoefficientTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 0 },
                                  { 0, int.MaxValue, 0 },
                                  { 0, 4, int.MaxValue } };

            int expected = 4; // ��������� �������� ��� ������ [0,1]

            int actual = Program2.getCoefficient(testArray, 0, 1);

            Assert.AreEqual(expected, actual);
        }

        // ������� ���� ��� ������� GetCost (���������� ����� ����)
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

            int expected = 7; // ��������� ��������

            int actual = Program1.GetCost(testDict, testSources);

            Assert.AreEqual(expected, actual);
        }


        // �������������� ���� ��� ������� FindBestZeros (���������� ������� ����� � ���������� �������� ����)
        [Test]
        public void FindBestZerosTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 0 },
                                  { 0, int.MaxValue, 0 },
                                  { 0, 4, int.MaxValue } };

            List<int> expectedList = new List<int>() { 1, 6 }; // ��� ������� �� ������
            List<int> coeffList = new List<int>(); // ���� ��� ����������� �������� ��� ���������� �������


            for (int i = 0; i < testArray.GetLength(0); i++)
                for (int j = 0; j < testArray.GetLength(0); j++)
                {
                    if (testArray[i, j] == 0)
                        coeffList.Add(Program2.getCoefficient(testArray, i, j)); // ����� ���������� �������
                }

            List<int> actualList2 = Finder.findBestZeros(testArray, coeffList); // ����� ������� �������

            CollectionAssert.AreEqual(expectedList, actualList2); // ��������� �����������
        }


        // ���������� ���� ��� ������� GetCoefficient (���������� ������� ����)
        [Test]
        public void GetCoefficientNegativeTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 1 },
                                  { 0, int.MaxValue, 2 },
                                  { 3, 0, int.MaxValue } };

            int unexpected = int.MaxValue; // �������� �������� ��� ������ [1,0]

            int actual = Program2.getCoefficient(testArray, 1, 0);

            Assert.AreNotEqual(unexpected, actual);
        }


        // ���������� ���� ��� ������� GetCost (���������� ����� ����)
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

            int unexpected = -1; // �������� ��������

            int actual = Program1.GetCost(testDict, testSources);

            Assert.AreNotEqual(unexpected, actual);
        }


        // ���������� ���� ��� ������� FindBestZeros (���������� ������� ����� � ���������� �������� ����)
        [Test]
        public void FindBestZerosNegativeTest()
        {
            int[,] testArray = { { int.MaxValue, 0, 0 },
                                  { 0, int.MaxValue, 0 },
                                  { 0, 4, int.MaxValue } };

            List<int> expectedList = new List<int>() { -1, 6 }; // ��� ����� �� ������� �� ������
            List<int> coeffList = new List<int>(); // ���� ��� ����������� �������� ��� ���������� �������


            for (int i = 0; i < testArray.GetLength(0); i++)
                for (int j = 0; j < testArray.GetLength(0); j++)
                {
                    if (testArray[i, j] == 0)
                        coeffList.Add(Program2.getCoefficient(testArray, i, j)); // ����� ���������� �������
                }

            List<int> actualList2 = Finder.findBestZeros(testArray, coeffList); // ����� ������� �������

            CollectionAssert.AreNotEqual(expectedList, actualList2); // ��������� �����������
        }
    }
}