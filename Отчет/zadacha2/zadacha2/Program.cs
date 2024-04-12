using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadacha2
{
    struct BuyerInfo
    {
        public int OrderNumber;
        public string FullName;
        public string Address;
        public DateTime RegistrationDate;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Создание списка покупателей
            Queue<BuyerInfo> buyersQueue = new Queue<BuyerInfo>();

            // Пример ввода данных о покупателях
            AddBuyer(buyersQueue, 1, "Иванов Иван Иванович", "ул. Пушкина, д.10", new DateTime(2024, 4, 6));
            AddBuyer(buyersQueue, 2, "Петров Петр Петрович", "ул. Лермонтова, д.5", new DateTime(2024, 4, 5));
            AddBuyer(buyersQueue, 3, "Сидоров Сидор Сидорович", "ул. Гоголя, д.3", new DateTime(2024, 4, 7));

            // Вывод списка покупателей в порядке очереди по датам постановки на учет
            Console.WriteLine("Список лиц в порядке очереди по датам постановки на учет:");
            while (buyersQueue.Count > 0)
            {
                BuyerInfo buyer = buyersQueue.Dequeue();
                Console.WriteLine($"Порядковый номер: {buyer.OrderNumber}");
                Console.WriteLine($"Ф.И.О.: {buyer.FullName}");
                Console.WriteLine($"Домашний адрес: {buyer.Address}");
                Console.WriteLine($"Дата постановки на учет: {buyer.RegistrationDate}");
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        // Метод для добавления информации о покупателе в очередь
        static void AddBuyer(Queue<BuyerInfo> buyersQueue, int orderNumber, string fullName, string address, DateTime registrationDate)
        {
            BuyerInfo buyer = new BuyerInfo();
            buyer.OrderNumber = orderNumber;
            buyer.FullName = fullName;
            buyer.Address = address;
            buyer.RegistrationDate = registrationDate;
            buyersQueue.Enqueue(buyer);
        }
    }
}