using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    //Класс сетки из ячеек.
    //Экземпляр представляет собой объект, содержащий двумерный логический массив.
    class Grid
    {
        //Состояние ячеек, где true - открыта, false - закрыта.
        public Boolean[,] cell;

        //Конструктор сетки, которому передаём кол-во строк и кол-во столбцов,
        //ячейки поля при создании будут заполняться нулями.
        public Grid(Int32 height, Int32 width)
        {
            cell = new Boolean[height, width];
        }

        //Экземплярный метод, проверяющий 
        //возможно ли пройти от самой верхней ячейки до самой нижней
        //путём движения вниз, налево и направо в пределах откртых ячеек.
        public Boolean GetGridPassing()
        {
            //Список, содержащий уже проверенные значения.
            List<Int32[]> alreadyChecked = new List<Int32[]>();
            //Рекурсивная функция,
            //принимающий на вход номер строки и номер стоблца true-элемента сетки.
            //Далее она проверяет можно ли из этого элемента перейти вниз и, если да, то
            //определяет диапазон true-элементов и в цикле для каждого элемента исполняемый метод вызывает сам себя (передавая в параметр элемент цикла).
            //Возвращает true, если нижний элемент - последний и открытый (true).
            Func<Int32, Int32, Boolean> GoDownYesNo = null;

            GoDownYesNo = new Func<Int32, Int32, Boolean>((Int32 cellLine, Int32 cellColumn) =>
            {
                if ((alreadyChecked.Any(element => element[0] == cellLine && element[1] == cellColumn)) == false)
                {
                    alreadyChecked.Add(new Int32[] { cellLine, cellColumn });
                    //Если ячейка ниже существует и открыта, то
                    //определяем границы её "непрерывных" открытых соседей (левую и правую границы, получим диапазон из true элементов).
                    if ((cellLine + 1) < this.cell.GetLength(0))
                    {
                        //Если ячейка открыта.
                        if (this.cell[cellLine + 1, cellColumn] == true)
                        {
                            Int32 cellLeftBorder = cellColumn;
                            Int32 cellRightBorder = cellColumn;

                            //Левая граница
                            while ((cellLeftBorder - 1) >= 0 &&
                                this.cell[(cellLine + 1), (cellLeftBorder - 1)] != false)
                            {
                                cellLeftBorder--;
                            }

                            //Правая граница
                            while ((cellRightBorder + 1) < this.cell.GetLength(1) &&
                                this.cell[(cellLine + 1), (cellRightBorder + 1)] != false)
                            {
                                cellRightBorder++;
                            }

                            //Для полученного диапазона выполняем данную функцию
                            for (; cellLeftBorder <= cellRightBorder; cellLeftBorder++)
                            {
                                if (GoDownYesNo.Invoke(cellLine + 1, cellLeftBorder) == true)
                                {
                                    return true;
                                }

                                if (cellLeftBorder == cellRightBorder)
                                {
                                    return false;
                                }
                            }
                        }
                        //Если ячейка закрыта.
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            });

            Boolean result = false;

            //Проверка каждой верхней ячейки, i - номер столбца.
            for (Int32 i = 0; i < this.cell.GetLength(1); i++)
            {
                //Если клетка открытая, то
                if (this.cell[0, i] == true)
                {
                    //Клетка идёт на вход рекурсивному методу и её возврат идёт в result.
                    result = GoDownYesNo.Invoke(0, i);
                    //Если метод вернул true, то прерываем выполнение метода и возвращаем result.
                    if (result == true)
                    {
                        return result;
                    }
                }
            }
            return result;
        }

        //Экземплярный метод, заполняющий ячейки сетки случайными значениями (true, false).
        public void SetGridRandom()
        {
            Random rand = new Random();
            //Для каждой строки.
            for (Int32 i = 0; i < cell.GetLength(0); i++)
            {
                //Для каждой столбца.
                for (Int32 j = 0; j < cell.GetLength(1); j++)
                {
                    cell[i, j] = Convert.ToBoolean(rand.Next(2));
                }
            }
        }

        //Статический метод для тестирования:
        //1)создаёт экземпляр класса Grid заданной размерности,
        //2)вызывает метод SetGridRandom для случайного заполнения объекта, если параметр console = true, то выводит содержимое на консоль.
        //3)Возвращает возврат метода GetGridPassing.
        //Для прохождения тест должен выполниться без ошибок и вернуть правильный результат.
        public static Boolean Test_GetGridRandomPassing(Int32 height, Int32 width, Boolean console)
        {
            Grid grid = new Grid(height, width);
            Boolean result = false;

            grid.SetGridRandom();

            result = grid.GetGridPassing();

            if (console == true)
            {
                grid.GetGridConsole();
                Console.WriteLine("grid return {0}", result);
            }

            return result;
        }

        //Статический метод для тестирования:
        //1)создаёт экземпляр класса Grid заданной размерности,
        //2)заполняет поле cell одинаковыми заданными значениями true или false и
        //3)возвращает результат равенства возврата метода GetGridPassing и значения cell.
        //Для прохождения тест должен выполниться без ошибок и вернуть true.
        public static Boolean Test_GetTrueGridPassing(Int32 height, Int32 width, Boolean cell)
        {
            Grid grid = new Grid(height, width);

            //Для каждой строки.
            for (Int32 i = 0; i < grid.cell.GetLength(0); i++)
            {
                //Для каждой столбца.
                for (Int32 j = 0; j < grid.cell.GetLength(1); j++)
                {
                    grid.cell[i, j] = cell;
                }
            }

            return (grid.GetGridPassing() == cell);
        }

        //Статический метод для тестирования:
        //1)создаёт экземпляр класса Grid заданной размерности,
        //2)заполняет поле cell, кроме  предпоследней строчки, одинаковыми заданными значениями true или false, последнюю строчку - дргугим значение и
        //3)возвращает результат равенства возврата метода GetGridPassing и значения cell.
        //Для прохождения тест должен выполниться без ошибок и вернуть false.
        public static Boolean Test_GetFalseGridPassing(Int32 height, Int32 width, Boolean cell)
        {
            Grid grid = new Grid(height, width);
            Int32 i = 0;

            //Для каждой строки.
            for (; i < grid.cell.GetLength(0) - 2; i++)
            {
                //Для каждой столбца.
                for (Int32 j = 0; j < grid.cell.GetLength(1); j++)
                {
                    grid.cell[i, j] = cell;
                }
            }

            if (cell == true)
            {
                for (Int32 j = 0; j < grid.cell.GetLength(1); j++)
                {
                    grid.cell[i, j] = false;
                }
            }
            else
            {
                for (Int32 j = 0; j < grid.cell.GetLength(1); j++)
                {
                    grid.cell[i, j] = true;
                }
            }

            i++;

            for (Int32 j = 0; j < grid.cell.GetLength(1); j++)
            {
                grid.cell[i, j] = cell;
            }

            return grid.GetGridPassing();
        }

        //Экземплярный метод, выводящий на консоль содержимое сетки.
        public void GetGridConsole()
        {
            //Для каждой строки.
            for (Int32 i = 0; i < cell.GetLength(0); i++)
            {
                //Проверка проходимости ячейки сверху вниз. j - столбец.
                for (Int32 j = 0; j < cell.GetLength(1); j++)
                {
                    Console.Write((Convert.ToInt32(cell[i, j])).ToString() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
