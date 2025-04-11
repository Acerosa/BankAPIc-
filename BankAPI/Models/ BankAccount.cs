namespace BankAPI.Models
{
    public class BankAccount
    {
        private static int lastAccountId = 1000;
        public int AccountID { get; set; }
        public string Owner { get; set; }
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }

        public BankAccount(string owner)
        {
            Owner = owner;
            AccountID = ++lastAccountId;
            Balance = 0;
            IsDeleted = false;
        }

        public BankAccount() { }

        public void Deposit(decimal amount)
        {
            if (IsDeleted) return;

            if (amount > 0)
            {
                Balance += amount;
            }
        }

        public void Withdraw(decimal amount)
        {
            if (IsDeleted) return;

            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
            }
        }

        public static void SetLastAccountId(int maxId)
        {
            lastAccountId = maxId;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}