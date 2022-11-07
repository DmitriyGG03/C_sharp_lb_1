using Manager;
using System;
using System.Collections;
using System.Text;
using System.Windows.Input;



Console.OutputEncoding = UTF8Encoding.UTF8; //Для української мови


Menu();


void Menu()
{
    AccountsManager MainObject = new AccountsManager();
    string? Comand;
    while (true)
    {
        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine("                            МЕНЮ\n");
        Console.WriteLine("[1] Поповнити рахунок");
        Console.WriteLine("[2] Зняти кошти з рахунку");
        Console.WriteLine("[3] Відкрити депозит");
        Console.WriteLine("[4] Закрити депозит");
        Console.WriteLine("[5] Визначити загальну кількусть грошей на депозитах рахунку");
        Console.WriteLine("[6] Вивести інформацію про рахунок");
        Console.WriteLine("[7] Перерахувати депозити");
        Console.WriteLine("[8] Вихід");
        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine("                        Існуючі акаунти                        ");
        if (MainObject.Accounts is not null)
        {
            for (int i = 0; i < MainObject.Accounts.Count; i++)
            {
                Console.WriteLine(ConvertNumber(MainObject.Accounts[i].AccountNumber));
            }
        }
        Console.WriteLine("---------------------------------------------------------------");


        Comand = Console.ReadLine();
        switch (Comand)
        {
            case "1": // Поповнення коштів на рахунку
                {
                    Console.WriteLine("Для поповнення рахунку за його номером введіть такі дані (для віміни напишіть \"back\"):");
                    int code = 0;
                    string? input = null;
                    double amount = 0.0;
                    BankAccount? bankAccount = null;
                    code = GetAccountNumber(ref bankAccount, MainObject);
                    if (code == 1) break;
                    else if (code == 2) continue;

                    while (true)
                    {
                        Console.WriteLine("Яку суму ви хочете занести на рахунок?");
                        code = CheckNumber(ref input, ref amount);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        bankAccount.Amount += amount;
                        Ending($"{amount} грн були успішно начислені на рахунок з номером: {ConvertNumber(bankAccount.AccountNumber)};\nЗалишок на рахунку: {bankAccount.Amount} грн.");

                        break;
                    }

                    break;
                }

            case "2": // Зняття коштів з рахунку
                {
                    Console.WriteLine("Для зняття грошей з рахунку введіть такі дані (для віміни напишіть \"back\"):");
                    int code;
                    string? input = null;
                    double amount = 0.0;
                    BankAccount? bankAccount = null;
                    code = GetAccountNumber(ref bankAccount, MainObject);
                    if (code == 1) break;
                    else if (code == 2) continue;

                    if (bankAccount.Amount == 0)
                    {
                        Ending("На рахунку немає коштів!");
                        continue;
                    }

                    while (true)
                    {
                        Console.WriteLine("Яку суму ви хочете зняти з рахунку?");
                        code = CheckNumber(ref input, ref amount);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (amount > bankAccount.Amount)
                        {
                            Ending("На рахунку недостатньо коштів!!!");
                            continue;
                        }
                        bankAccount.Amount -= amount;
                        Ending($"{amount} грн були успішно зняті з рахунку з номером: {ConvertNumber(bankAccount.AccountNumber)};\nЗалишок на рахунку: {bankAccount.Amount} грн.");
                        break;
                    }
                    break;
                }

            case "3": // Відкриття депозиту 
                {
                    int rate, code, temp;
                    double val = 0.0;
                    string? input = null;
                    BankAccount? bankAccount = null; //Аккаунт, в якому буде відкрито депозит
                    Console.Clear();
                    while (true)
                    {
                        Console.WriteLine("Для відкриття депозиту за номером рахунку введіть такі дані (для віміни напишіть \"back\"):");
                        //Номер рахунку
                        code = GetAccountNumber(ref bankAccount, MainObject);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (bankAccount.Amount < 100)
                        {
                            Ending("На рахунку недостатньо коштів, мінімально 100!");
                            continue;
                        }

                        while (true)
                        {
                            //Кількість коштів, що вноситься на депозит
                            Console.WriteLine("Кількість грошей (мін 100):");
                            code = CheckNumber(ref input, ref val);
                            if (code == 1) break;
                            else if (code == 2) continue;

                            if (val < 100 || val > bankAccount.Amount)
                            {
                                Ending("Введена сума є некоректною!");
                                continue;
                            }
                            //Відсоткова станка депозиту
                            Console.WriteLine("Відсоткова ставка(6, 8, 10, 14):");
                            rate = Convert.ToInt32(Console.ReadLine());
                            if (rate != 6 && rate != 8 && rate != 10 && rate != 14)
                            {
                                Ending("Введена відсоткова ставка є некоректною!");
                                continue;
                            }

                            bankAccount.Amount -= val;
                            temp = bankAccount.GetDepositID(MainObject);
                            bankAccount.CreateDeposit(temp, val, rate);
                            Ending($"{val} грн були успішно покладені на новостворений депозит № {temp} на рахунку з номером: {ConvertNumber(bankAccount.AccountNumber)};\nЗалишок на рахунку: {bankAccount.Amount} грн.");
                            break;
                        }
                        break;
                    }
                    break;
                }

            case "4": // Закриття депозиту
                {
                    int code;
                    int id = 0;
                    double val = 0.0;
                    string? input = null;
                    BankAccount? bankAccount = null; //Аккаунт, в якому буде відкрито депозит
                    Deposits? deposits = null; //Шукаємий депозит
                    Console.Clear();
                    while (true)
                    {
                        Console.WriteLine("Для відкриття депозиту за номером рахунку введіть такі дані (для віміни напишіть \"back\"):");
                        //Номер рахунку
                        code = GetAccountNumber(ref bankAccount, MainObject);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (bankAccount.Deposits is null)
                        {
                            Ending("На рахунку немає відкритих депозитів!");
                            continue;
                        }

                        while (true)
                        {
                            //Номер депозиту, що необхідно закрити
                            Console.WriteLine("Номер депозиту, що необхідно закрити:");

                            code = CheckNumber(ref input, ref val); //Отримуємо перевірену цифру номера
                            if (code == 1) break;
                            else if (code == 2) continue;

                            id = Convert.ToInt32(val);
                            deposits = bankAccount.FindDeposits(id); // Знаходимо депозит, що нам необхідно закрити
                            val = deposits.DepAmount;

                            bankAccount.DeleteDeposit(MainObject, deposits);

                            Ending($"Депозит з номером {id} був успішно видалений, а сума {val} грн була успішно покладена на основний рахунок № {ConvertNumber(bankAccount.AccountNumber)};\nЗалишок на рахунку: {bankAccount.Amount} грн.");
                            break;
                        }
                        break;
                    }
                    break;
                }

            case "5": // Визначення загальної кількості грошей на депозитах рахунку
                {
                    int code;
                    BankAccount? bankAccount = null; //Аккаунт, в якому буде відкрито депозит
                    Console.Clear();
                    while (true)
                    {
                        Console.WriteLine("Для визначення загальної кількості грошей на депозитах рахунку введіть такі дані (для віміни напишіть \"back\"):");
                        //Номер рахунку
                        code = GetAccountNumber(ref bankAccount, MainObject);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        if (bankAccount.Deposits.Count == 0)
                        {
                            Ending("На рахунку немає відкритих депозитів!");
                            continue;
                        }

                        Ending($"На рахунку номер {ConvertNumber(bankAccount.AccountNumber)} відкрито {bankAccount.Deposits.Count} депозити\\ів із загальною кількістю грошей на них: {bankAccount.AmountOnDeposits()} грн.");
                        break;
                    }
                    break;
                }

            case "6": // Вивести інформацію про рахунок
                {
                    int code;
                    BankAccount? bankAccount = null; //Аккаунт, в якому буде відкрито депозит
                    Console.Clear();
                    while (true)
                    {
                        Console.WriteLine("Для визначення загальної кількості грошей на депозитах рахунку введіть такі дані (для віміни напишіть \"back\"):");
                        //Номер рахунку
                        code = GetAccountNumber(ref bankAccount, MainObject);
                        if (code == 1) break;
                        else if (code == 2) continue;

                        Ending(bankAccount.ToString());
                        break;
                    }
                    break;
                }

            case "7": // Перерахувати депозити
                {
                    DateTime dt = MainObject.Accounts[0].Deposits[0].LastPayedDate;                    
                    TimeSpan ts = new TimeSpan(365, 0, 0, 0, 0);
                    dt -= ts;
                    MainObject.Accounts[0].Deposits[0].LastPayedDate = dt;
                    MainObject.InterestAccrual();
                    break;
                }

            case "8": // Вихід
                {
                    Console.WriteLine("Вийти з програми? Так/Ні");
                    Comand = Console.ReadLine();

                    if (Comand == "Так" || Comand == "так") return;
                    else break;
                }

            default:
                Ending("Помилка введення. Спробуйте знову!");
                break;
        }

    }
}

void Ending(string text)
{
    Console.Clear();
    Console.WriteLine(text);
}

int GetAccountNumber(ref BankAccount? bankAccount, AccountsManager MainObject)
{
    int temp;
    Console.WriteLine("Номер рахунку:");
    string? input = Console.ReadLine();
    if (input == "back") // Проверка на выход
    {
        Console.Clear();
        return 1;
    }
    else if (input is null || !int.TryParse(input, out temp) || input.Length != 8) // Проверка на ввод данных
    {
        Ending("Помилка введення даних!!!");
        return 2;
    }
    bankAccount = MainObject.FindAccount(Convert.ToInt32(input));
    if (bankAccount == null)
    {
        Ending("Помилка знаходження аккаунта, спробуйте знову!");
        return 2;
    }
    return 0;
}

int CheckNumber(ref string? input, ref double amount)
{
    int temp;
    input = Console.ReadLine();
    if (input == "back") // Проверка на выход
    {
        Console.Clear();
        return 1;
    }
    else if (input is null || !int.TryParse(input, out temp)) // Проверка на ввод данных
    {
        Ending("Помилка введення даних!!!");
        return 2;
    }
    amount = Convert.ToDouble(input);
    if (amount == 0) // Проверка на ввод данных
    {
        Ending("Помилка введення даних!!!");
        return 2;
    }
    return 0;
}

string ConvertNumber(in double number)
{
    StringBuilder sb = new StringBuilder();
    string num = number.ToString();
    for (int i = 0; i < 8 - num.Length; i++)
    {
        sb.Append('0');
    }
    sb.Append(num);
    return sb.ToString();
}