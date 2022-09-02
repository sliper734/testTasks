/*
    Разработать консольное приложение, которое генерирует список случайных чисел диапозоном от -100 до 100 в случайном количестве, но не меньше 20 штук и не более 100 , выводит получившуюся последовательность на экран,
затем следующей строкой выводит отсортированную по одному из алгоритмов сортировки последовательность (алгоритм выбирается каждый раз случайным образом).

В приложении должны быть реализованы минимум 2 алгоритма сортировки (на выбор исполнителя). Выбор алгоритма сортировки случайный.
Результат сортировки отобразить в консоли и реализовать отправку на rest api сервер, адрес которого берется из файла конфигурации (требуется реализовать только отправку данных, поднимать сервер и реализовывать на его стороне приём и обработку данных не требуется).
Исходный код выложить в репозиторий Github и выслать нам его адрес ( сделать публичным ).
*/

using System.Configuration;
using System.Text;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new();
            int count = rnd.Next(20, 101);
            List<int> list = new ();
            for (int i = 0; i < count; i++)
            {
                list.Add(rnd.Next(-100, 101));
            }
            Write(list);

            Console.WriteLine();
            var sortType = rnd.Next(0, 4);
            switch (sortType)
            {
                case 0:
                    list.Sort();
                    break;

                case 1:
                    list = Sort1(list);
                    break;

                case 2:
                    list = Sort2(list);
                    break;

                case 3:
                    list = Sort3(list);
                    break;
            }
            Write(list);

            Console.WriteLine();
            SendList(list);
        }
        
        static void Write(List<int> list)
        {
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
        }

        static List<int> Sort1(List<int> list)
        {
            var orderedlist = from i in list
                                 orderby i
                                 select i;

            return orderedlist.ToList();
        }

        static List<int> Sort2(List<int> list)
        {
            var tempMas = list.ToArray();
            int temp;
            for (int i = 0; i < tempMas.Length - 1; i++)
            {
                for (int j = i + 1; j < tempMas.Length; j++)
                {
                    if (tempMas[i] > tempMas[j])
                    {
                        temp = tempMas[i];
                        tempMas[i] = tempMas[j];
                        tempMas[j] = temp;
                    }
                }
            }

            var a = tempMas.ToList();
            return a;
        }

        static List<int> Sort3(List<int> list)
        {
            return QuickSort(list.ToArray(), 0, list.Count-1).ToList();
        }

        static int[] QuickSort(int[] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return array;
            }

            int pivotIndex = GetPivotIndex(array, minIndex, maxIndex);

            QuickSort(array, minIndex, pivotIndex - 1);

            QuickSort(array, pivotIndex + 1, maxIndex);

            return array;
        }

        static int GetPivotIndex(int[] array, int minIndex, int maxIndex)
        {
            int pivot = minIndex - 1;

            for (int i = minIndex; i <= maxIndex; i++)
            {
                if (array[i] < array[maxIndex])
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }
            }

            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);

            return pivot;
        }

        static void Swap(ref int leftItem, ref int rightItem)
        {
            int temp = leftItem;

            leftItem = rightItem;

            rightItem = temp;
        }

        static async void SendList(List<int> list)
        {
            HttpClient client = new ();
            await client.PostAsync(ConfigurationManager.AppSettings.Get("uri"), new StringContent(String.Join(", ", list), Encoding.UTF8, "application/json"));
        }
    }
}