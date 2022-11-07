using Manager;
using System.Text;

class BankAccount
{
    public string FullName { get; set; }
    public int AccountNumber { get; set; }
    public double Amount { get; set; } = 0;

    public List<Deposits> Deposits { get; set; } = new List<Deposits>(0); // Записуємо всі активні депозити користувачів




    public BankAccount(int num, string fullName)
    {
        FullName = fullName;
        AccountNumber = num; // Додати функцію генерації номера акаунта
    }
    //---------------------------------------------


    public int GetDepositID(AccountsManager main)
    {

        if (main.DelDeposits.Count != 0)
        {
            int temp = main.DelDeposits[0];
            main.DelDeposits.RemoveAt(0);
            return temp;
        }
        else if (Deposits.Count == 0)
        {
            return 1;
        }
        else
        {
            int max = 0, value = 0;
            for (int i = 0; i < Deposits.Count; i++)
            {
                value = Deposits[i].ID;
                if (value > max) max = value;
            }
            return max + 1;
        }
    }

    public void CreateDeposit(int id, double amount, int interestRate) //Створення нового депозиту для акаунта
    {
        Deposits.Add(new Deposits(id, amount, (InterestRate)interestRate));
    }

    public bool DeleteDeposit(AccountsManager mainAcc, Deposits deposit) //Закриття депозиту для акаунта по номеру депозита
    {
        Amount += deposit.DepAmount; // Додаємо суму на депозиті на рахунок

        mainAcc.DelDeposits.Add(deposit.ID); // Записуємо номер депозиту в список видалених номерів

        return Deposits.Remove(deposit); // Видаляємо депозит з масиву депозитів
    }

    public Deposits? FindDeposits(int num) //Знаходження аккаунта
    {
        foreach (Deposits i in Deposits)
        {
            if (i.ID == num)
            {
                return i;
            }
        }
        return null;
    }

    public double AmountOnDeposits()
    {
        double sum = 0.0;
        foreach (Deposits i in Deposits)
        {
            sum += i.DepAmount;
        }
        return sum;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder($"ПІБ: {FullName}, номер рахунку: {AccountNumber}, сума на рахунку: {Amount}");
        if (Deposits.Count == 0)
        {
            sb.Append(", відкритих депозитів немає.");
        }
        else
        {
            sb.Append($", номера відкритих депозитів: ");
            for (int i = 0; i < Deposits.Count; i++)
            {
                sb.Append(Convert.ToString(Deposits[i].ID));
                if (i != Deposits.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(".");
        }
        return sb.ToString();
    }
}
