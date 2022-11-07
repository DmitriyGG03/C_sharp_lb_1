using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace Manager;
class AccountsManager
{
    public List<BankAccount> Accounts { get; } = new List<BankAccount>(0); // Записуємо всі активні аккаунти користувачів
    public List<int> DelAccounts { get; set; } = new List<int>(0); // Записуємо всі номери видалених АККАУНТІВ користувачів
    public List<int> DelDeposits { get; set; } = new List<int>(0); // Записуємо всі номери видалених ДЕПОЗИТІВ користувачів

    public void AddNewAccount(BankAccount bankAccount) //Додаємо акаунт
    {
        Accounts.Add(bankAccount);
    }
    public void DelAccount(BankAccount bankAccount) //Видаляємо акаунт
    {
        Accounts.Remove(bankAccount);
    }
    public BankAccount? FindAccount(int num) //Знаходження аккаунта
    {
        foreach (BankAccount i in Accounts)
        {
            if (i.AccountNumber == num)
            {
                return i;
            }
        }
        return null;
    }

    public int GetAccountID()
    {

        if (DelAccounts.Count != 0)
        {
            int temp = DelAccounts[0];
            DelAccounts.RemoveAt(0);
            return temp;
        }
        else if (Accounts.Count == 0)
        {
            return 1;
        }
        else
        {
            int max = 0, value = 0;
            for (int i = 0; i < Accounts.Count; i++)
            {
                value = Accounts[i].AccountNumber;
                if (value > max) max = value;
            }
            return max + 1;
        }
    }

    public void InterestAccrual() // Нарахування відсотків по депозиту за рік
    {
        DateTime date1 = DateTime.Today;
        DateTime date2 = new DateTime();
        TimeSpan interval = new TimeSpan();
        double interestMoney = 0.0;
        double interestRate = 0.0;

        foreach (BankAccount i in Accounts)
        {
            if (i.Deposits.Count != 0)
            {
                foreach (Deposits d in i.Deposits)
                {
                    date2 = d.LastPayedDate;
                    interval = date1 - date2;
                    while (interval.TotalDays >= 365)
                    {
                        interestRate = (int)d.InterestRate;
                        interestMoney = d.DepAmount * (interestRate / 100);
                        d.DepAmount += interestMoney;
                        //d.LastPayedDate.AddYears(1);
                        Console.Clear();
                        Console.WriteLine($"По депозиту № {d.ID} було нараховано відсотки в розмірі {interestMoney} грн.");
                        date2 = d.LastPayedDate;
                        interval = date2 - date1;
                    }
                }
            }

        }
    }

    public AccountsManager()
    {
        AddNewAccount(new BankAccount(GetAccountID(), "Іванов Іван Іванович"));
        AddNewAccount(new BankAccount(GetAccountID(), "Петро Петров Петрович"));
        AddNewAccount(new BankAccount(GetAccountID(), "Віталій Сидоров Опанасович"));
    }

}

