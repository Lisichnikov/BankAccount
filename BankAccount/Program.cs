using System;
using System.Threading;

namespace BankAccount
{
    class Account
    {
        private decimal balance;

        public Account(decimal initialBalance)
        {
            balance = initialBalance;
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public void Deposit(decimal amount)
        {
            // Имитация задержки в операции пополнения
            Thread.Sleep(1000);
            balance += amount;
            Console.WriteLine($"Пополнение на {amount} руб. Баланс: {balance} руб.");
        }

        public void Withdraw(decimal amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                Console.WriteLine($"Снятие {amount} руб. Баланс: {balance} руб.");
            }
            else
            {
                Console.WriteLine("Недостаточно средств на счете.");
            }
        }

        public void WaitForBalance(decimal targetAmount)
        {
            while (balance < targetAmount)
            {
                // Пауза в ожидании пополнения счета
                Thread.Sleep(1000);
            }

            Console.WriteLine($"Баланс достиг {targetAmount} руб., можно снять деньги.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Account account = new Account(1000m);

            // Запуск отдельного потока для пополнения счета
            Thread depositThread = new Thread(() => DepositRandomAmount(account));
            depositThread.Start();

            // Ожидание пополнения счета до 5000 руб.
            account.WaitForBalance(5000m);

            // Снятие 3000 руб. со счета
            account.Withdraw(3000m);

            // Вывод остатка на балансе
            Console.WriteLine($"Остаток на балансе: {account.Balance} руб.");

            // Ожидание завершения работы потока пополнения
            depositThread.Join();
        }

        static void DepositRandomAmount(Account account)
        {
            Random random = new Random();
            while (true)
            {
                decimal amount = random.Next(100, 1000);
                account.Deposit(amount);
            }
        }
    }
}