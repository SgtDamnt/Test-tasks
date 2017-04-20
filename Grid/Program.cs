using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    class Program
    {
        public static void Main()
        {
            //Пример с выводом на консоль.

            //Ручное создание объекта, заполнение случайными числами и тестирование
            Console.WriteLine("Grid manual test:");
            Console.WriteLine("\nRandom Grid (0 - closed cell, 1 - open cell):\n");

            Grid grid = new Grid(20, 20);
            grid.SetGridRandom();

            grid.GetGridConsole();
            Console.WriteLine("\nGrid passed = {0}", grid.GetGridPassing());

            //Автоматическое создание объекта и его тестирование
            Console.WriteLine("\n\nGrid autotest:");
            Console.WriteLine("\nGrid 20x20, all elements true. Result must be true: \nResult: {0}\n", Grid.Test_GetTrueGridPassing(20, 20, true).ToString());
            Console.WriteLine("Grid 20x20, all elements false. Result must be true: \nResult: {0}\n", Grid.Test_GetTrueGridPassing(20, 20, false).ToString());
            Console.WriteLine("The worst case. Grid 20x20, all elements,except 19-th line, true,\nthe 19-th line - is false. Result must be false: \nResult: {0}\n", Grid.Test_GetFalseGridPassing(20, 20, true).ToString());
            Console.WriteLine("Grid 20x20, all elements,except 19-th line, false,\nthe 19-th line - is true. Result must be false: \nResult: {0}\n", Grid.Test_GetFalseGridPassing(20, 20, false).ToString());

            Console.WriteLine("Grid 20x20, all elements random,\nthere should be no exception and must be some result: \nResult:\n");
            Grid.Test_GetGridRandomPassing(20, 20, true);

            Console.Read();
        }
    }
}