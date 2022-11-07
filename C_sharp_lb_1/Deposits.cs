
class Deposits
{
    public int ID { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public DateTime LastPayedDate { get; set; } = DateTime.Today;
    public double DepAmount { get; set; }
    public InterestRate InterestRate { get; init; }

    public Deposits(int id, double depAmount, InterestRate interestRate)
    {
        ID = id;
        DepAmount = depAmount;
        InterestRate = interestRate;
    }  
}

public enum InterestRate
{
    Low = 6,
    Medium = 8,
    High = 10,
    VeryHigh = 14
}