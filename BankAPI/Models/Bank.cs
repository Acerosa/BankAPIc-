namespace BankAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;

    public class Bank
    {
        private static string DatabaseFile = "accounts.json";
        private List<BankAccount> accounts;

        public Bank()
        {
            accounts = LoadAccounts();
        }

        public List<BankAccount> GetAllAccounts()
        {
            return accounts.Where(a => !a.IsDeleted).ToList();
        }

        public BankAccount? GetAccountById(int id)
        {
            return accounts.FirstOrDefault(a => a.AccountID == id && !a.IsDeleted);
        }

        public BankAccount CreateAccount(string owner)
        {
            var account = new BankAccount(owner);
            accounts.Add(account);
            SaveAccounts();
            return account;
        }

        public bool Deposit(int id, decimal amount)
        {
            var account = GetAccountById(id);
            if (account == null) return false;
            account.Deposit(amount);
            SaveAccounts();
            return true;
        }

        public bool Withdraw(int id, decimal amount)
        {
            var account = GetAccountById(id);
            if (account == null) return false;
            account.Withdraw(amount);
            SaveAccounts();
            return true;
        }

        public bool DeleteAccount(int id)
        {
            var account = accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null || account.IsDeleted) return false;
            account.MarkAsDeleted();
            SaveAccounts();
            return true;
        }

        private void SaveAccounts()
        {
            string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DatabaseFile, json);
        }

        private List<BankAccount> LoadAccounts()
        {
            if (File.Exists(DatabaseFile))
            {
                string json = File.ReadAllText(DatabaseFile);
                var loadedAccounts = JsonSerializer.Deserialize<List<BankAccount>>(json) ?? new List<BankAccount>();
                if (loadedAccounts.Count > 0)
                {
                    int maxId = loadedAccounts.Max(a => a.AccountID);
                    BankAccount.SetLastAccountId(maxId);
                }
                return loadedAccounts;
            }
            return new List<BankAccount>();
        }
    }
}