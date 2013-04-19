using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CoinbaseSharp.API;
using CoinbaseSharp.Authentication;
using CoinbaseSharp.DataTypes;
using CoinbaseSharp.DataTypes.Responses;
using CoinbaseSharp.Resources;

namespace CoinbaseSharpApp
{
    public class Program
    {
        private static APIKey KEY1 = new APIKey("<INSERT KEY1>");
        private static APIKey KEY2 = new APIKey("<INSERT KEY2>");

        private static APIKey GOOD_KEY = new APIKey("<INSERT GOOD KEY>");
        private static APIKey BAD_KEY = new APIKey("aaaaaaaa");
        private static APIKey NO_BANK_ACCOUNT_KEY = new APIKey("<INSERT KEY WITH NO BANK ACCOUNT>");

        private static void ASSERT(bool condition, CoinbaseResource resouce)
        {
            if (condition)
            {
                Console.WriteLine("Passed");
            }
            else
            {
                Debug.Assert(condition, resouce.ToString());
                Console.WriteLine("Failed");
            }
        }

        #region TEST ACCOUNT

        public static void TestGetBalance()
        {
            Console.Write("Testing GetBalance w/ Valid Key: ");
            Amount balance = API.GetBalance(GOOD_KEY);
            ASSERT(balance.IsValid, balance);
            Console.Write("Testing GetBalance w/ Known Invalid Key: ");
            balance = API.GetBalance(BAD_KEY);
            ASSERT(!balance.IsValid, balance);
        }
        
        public static void TestGetReceiveAddress()
        {
            Console.Write("Testing GetReceiveAddress w/ Valid Key: ");
            ReceiveAddress addr = API.GetReceiveAddress(GOOD_KEY);
            ASSERT(addr.IsValid, addr);
            Console.Write("Testing GetReceiveAddress w/ Invalid Key: ");
            addr = API.GetReceiveAddress(BAD_KEY);
            ASSERT(!addr.IsValid, addr);
        }

        public static void TestGenerateReceiveAddress()
        {
            Console.Write("Testing GetReceiveAddress w/ Valid Key: ");
            ReceiveAddress addr = API.GenerateReceiveAddress(GOOD_KEY);
            ASSERT(addr.IsValid, addr);
            Console.Write("Testing GetReceiveAddress w/ Invalid Key: ");
            addr = API.GetReceiveAddress(BAD_KEY);
            ASSERT(!addr.IsValid, addr);
        }

        #endregion

        #region TEST BUTTON

        public static void TestCreateButton()
        {
            Console.Write("Testing CreatePaymentButton w/out Button w/ Valid Key: ");
            Button button = API.CreatePaymentButton(
                "Test1",
                new Amount() { AmountValue = 1m, Currency = "USD" },
                "This is Test1",
                GOOD_KEY
                );
            ASSERT(button.IsValid, button);
            Console.Write("Testing CreatePaymentButton w/out Button w/ Invalid Key: ");
            button = API.CreatePaymentButton(
                "Test2",
                new Amount() { AmountValue = 1m, Currency = "USD" },
                "This is Test2",
                BAD_KEY
                );
            ASSERT(!button.IsValid, button);

            Console.Write("Testing CreatePaymentButton w/ Button + Valid Key: ");
            button = new Button()
            {
                Name = "Test3",
                Cost = 1.23m,
                PriceCurrencyISO = "BTC",
                Description = "This is Test3",
                Type = ButtonType.Subscription,
                Style = ButtonStyle.None
            };
            button = API.CreatePaymentButton(button, GOOD_KEY);
            ASSERT(button.IsValid, button);
            Console.Write("Testing CreatePaymentButton w/ Button + Invalid Key: ");
            button = new Button()
            {
                Name = "Test4",
                Cost = 1.23m,
                PriceCurrencyISO = "BTC",
                Description = "This is Test4",
                Type = ButtonType.Subscription,
                Style = ButtonStyle.None
            };
            button = API.CreatePaymentButton(button, BAD_KEY);
            ASSERT(!button.IsValid, button);
        }

        #endregion

        #region TEST BUYS

        public static void TestBuyBitcoins()
        {
            Console.Write("Testing BuyBitcoins w/ Valid Key + Valid Amount: ");
            Transfer transfer = API.BuyBitcoins(0.1m, GOOD_KEY);
            ASSERT(transfer.IsValid, transfer);
            
            Console.Write("Testing BuyBitcoins w/ Valid Key + Invalid Small Amount: ");
            transfer = API.BuyBitcoins(0.01m, GOOD_KEY);
            ASSERT(!transfer.IsValid, transfer);

            Console.Write("Testing BuyBitcoins w/ Valid Key + Invalid Large Amount: ");
            transfer = API.BuyBitcoins(1000m, GOOD_KEY);
            ASSERT(!transfer.IsValid, transfer);

            Console.Write("Testing BuyBitcoins w/ Invalid Key: ");
            transfer = API.BuyBitcoins(0.1m, BAD_KEY);
            ASSERT(!transfer.IsValid, transfer);
            
            Console.Write("Testing BuyBitcoins w/ No Bank Account Key: ");
            transfer = API.BuyBitcoins(0.1m, NO_BANK_ACCOUNT_KEY);
            ASSERT(!transfer.IsValid, transfer);
        }

        #endregion

        #region TEST CONTACTS

        public static void TestGetContacts()
        {
            Console.Write("Testing GetContacts w/ Valid Key: ");
            Contacts contacts = API.GetContacts(GOOD_KEY);
            ASSERT(contacts.IsValid, contacts);

            if (contacts.IsValid && contacts.NumberOfPages > 1)
            {
                int n = contacts.NumberOfPages;
                for (int i = 2; i <= n; ++i)
                {
                    Console.Write("Testing GetContacts w/ Valid Key Page {0}: ", i);
                    contacts = API.GetContacts(n, GOOD_KEY);
                    ASSERT(contacts.IsValid, contacts);
                }
            }

            if (contacts.IsValid && contacts.TotalCount > 0)
            {
                string query = contacts.ContactList[0].Email;
                Console.Write("Testing GetContacts w/ Valid Key + Query {0}: ", query);
                contacts = API.GetContacts(query, GOOD_KEY);
                ASSERT(contacts.IsValid, contacts);
            }

            Console.Write("Testing GetContacts w/ Invalid Key: ");
            contacts = API.GetContacts(BAD_KEY);
            ASSERT(!contacts.IsValid, contacts);
        }

        #endregion

        #region TEST CURRENCIES

        public static void TestCurrencyInfo()
        {
            Console.Write("Testing GetCurrencies: ");
            List<Currency> currencies = API.GetCurrencies();
            if (currencies != null && currencies.Count >= 2)
            {
                Console.WriteLine("Passed [{0}]", currencies.Count);
            }
            else
            {
                Debug.Assert(false, "Currencies are not valid");
                Console.WriteLine("Failed");
            }

            Console.Write("Testing GetExchangeRates: ");

            var exchangeRates = API.GetExchangeRates();
            if (exchangeRates != null && exchangeRates.Count > 2)
            {
                Console.WriteLine("Passed [{0}]", exchangeRates.Count);
            }

            foreach (var from in exchangeRates.Keys)
            {
                foreach (var to in exchangeRates[from].Keys)
                {
                    Console.Write("Testing Exchange Rate {0} to {1}: ", from, to);
                    if (exchangeRates[to].ContainsKey(from))
                    {
                        Console.WriteLine("Passed");
                    }
                    else
                    {
                        Debug.Assert(false, "Exchange Rate Missing");
                        Console.WriteLine("Failed");
                    }
                }
            }
        }

        #endregion

        #region TEST ORDERS

        public static void TestGetOrders()
        {
            Console.Write("Testing GetOrders w/ Valid Key: ");
            Orders orders = API.GetOrders(GOOD_KEY);
            ASSERT(orders.IsValid, orders);

            if (orders.IsValid && orders.NumberOfPages > 1)
            {
                int n = orders.NumberOfPages;
                for (int i = 2; i <= n; ++i)
                {
                    Console.Write("Testing GetOrders w/ Valid Key Page {0}: ", i);
                    orders = API.GetOrders(n, GOOD_KEY);
                    ASSERT(orders.IsValid, orders);
                }
            }

            if (orders.IsValid && orders.TotalCount > 0)
            {
                Console.Write("Testing GetOrder {0} w/ Valid Key: ", orders.OrderList[0].ID);
                Order order = API.GetOrder(orders.OrderList[0].ID, GOOD_KEY);
                ASSERT(order.IsValid, orders);
            }

            Console.Write("Testing GetOrders w/ Valid Key + Invalid Page: ");
            orders = API.GetOrders(1000, GOOD_KEY);
            ASSERT(orders.IsValid && orders.TotalCount == 0, orders);

            Console.Write("Testing GetOrders w/ Invalid Key: ");
            orders = API.GetOrders(BAD_KEY);
            ASSERT(!orders.IsValid, orders);
        }

        #endregion

        #region TEST PRICES

        public static void TestGetBuyPrice()
        {
            Console.Write("Testing GetBuyPrice: ");
            Amount buyPrice = API.GetBuyPrice(3.24m);
            ASSERT(buyPrice.IsValid, buyPrice);
        }

        public static void TestGetSellPrice()
        {
            Console.Write("Testing GetSellPrice: ");
            Amount sellPrice = API.GetSellPrice(3.24m);
            ASSERT(sellPrice.IsValid, sellPrice);
        }

        #endregion

        #region TEST SELLS

        public static void TestSellBitcoins()
        {
            Console.Write("Testing SellBitcions w/ Valid Key: ");
            Transfer transfer = API.SellBitcoins(.1m, GOOD_KEY);
            ASSERT(transfer.IsValid, transfer);

            Console.Write("Testing SellBitcions w/ Invalid Key: ");
            transfer = API.SellBitcoins(.1m, BAD_KEY);
            ASSERT(!transfer.IsValid, transfer);
        }

        #endregion

        #region TEST TRANSACTIONS

        public static void TestGetTransactions()
        {
            Console.Write("Testing GetTransactions w/ Valid Key: ");
            Transactions transactions = API.GetTransactions(GOOD_KEY);
            ASSERT(transactions.IsValid, transactions);

            if (transactions.IsValid && transactions.NumberOfPages > 1)
            {
                int n = transactions.NumberOfPages;
                for (int i = 2; i <= n; ++i)
                {
                    Console.Write("Testing GetTransactions w/ Valid Key Page {0}: ", i);
                    transactions = API.GetTransactions(n, GOOD_KEY);
                    ASSERT(transactions.IsValid, transactions);
                }
            }

            if (transactions.IsValid && transactions.TotalCount > 0)
            {
                Console.Write(
                    "Testing GetTransaction {0} w/ Valid Key: ",
                    transactions.TransactionList[0].ID
                    );
                Transaction transaction = API.GetTransaction(
                    transactions.TransactionList[0].ID, 
                    GOOD_KEY
                    );
                ASSERT(transaction.IsValid, transaction);
            }

            Console.Write("Testing GetTransactions w/ Invalid Key: ");
            transactions = API.GetTransactions(BAD_KEY);
            ASSERT(!transactions.IsValid, transactions);
        }

        public static void TestConductTransactions(
            APIKey key1,
            APIKey key2
            )
        {
            Console.Write("Testing SendMoney w/ Valid Key: ");
            ReceiveAddress addr1 = API.GetReceiveAddress(key1);
            ReceiveAddress addr2 = API.GetReceiveAddress(key2);

            SendMoneyTransaction sendMoney = new SendMoneyTransaction()
            { 
                ToAddr = addr1.Address, 
                Amount = new Amount() { AmountValue = 0.01m, Currency = "BTC" }, 
                Notes = "Testing send_money" 
            };
            Transaction sendMoneyTransaction = API.SendMoney(
                sendMoney,
                key2
                );
            ASSERT(sendMoneyTransaction.IsValid, sendMoneyTransaction);

            if (!sendMoneyTransaction.IsValid)
            {
                return;
            }

            User user1 = API.GetUser(key1);
            if (!user1.IsValid)
            {
                return;
            }

            Console.Write("Testing RequestMoney w/ Valid Key: ");
            RequestMoneyTransaction requestMoney = new RequestMoneyTransaction()
            {
                FromAddr = user1.Email, 
                Amount = new Amount() { AmountValue = 0.01m, Currency = "BTC" }, 
                Notes = "Testing request_money" 
            };
            Transaction requestMoneyTransaction = API.RequestMoney(
                requestMoney,
                key2
                );
            ASSERT(requestMoneyTransaction.IsValid, requestMoneyTransaction);

            if (!requestMoneyTransaction.IsValid)
            {
                return;
            }

            Console.Write("Testing CancelRequest w/ Valid Key: ");
            CoinbaseResponse response = API.CancelRequest(requestMoneyTransaction.ID, key1);
            ASSERT(response.IsValid, response);

            requestMoneyTransaction = API.RequestMoney(
                requestMoney,
                key2
                );
            if (!requestMoneyTransaction.IsValid)
            {
                return;
            }

            response = API.ResendRequest(requestMoneyTransaction.ID, key2);
            Console.Write("Testing ResendRequest w/ Valid Key: ");
            ASSERT(response.IsValid, response);

            Console.Write("Testing CompleteRequest w/ Valid Key: ");
            response = API.CompleteRequest(requestMoneyTransaction.ID, key1);
            ASSERT(response.IsValid, response);
        }

        #endregion

        #region TEST TRANSFERS

        public static void TestGetTransfers()
        {
            Console.Write("Testing GetTransfers w/ Valid Key: ");
            Transfers transfers = API.GetTransfers(GOOD_KEY);
            ASSERT(transfers.IsValid, transfers);

            Console.Write("Testing GetTransfers w/ Invalid Key: ");
            transfers = API.GetTransfers(BAD_KEY);
            ASSERT(!transfers.IsValid, transfers);
        }

        #endregion

        #region TEST USERS

        public static void TestCreateUser(string newEmail, string password)
        {
            Console.Write("Testing Create User for {0}: ", newEmail);
            User user = API.CreateUser(newEmail, password);
            ASSERT(user.IsValid, user);

            Console.Write("Testing Create User Again for {0}: ", newEmail);
            user = API.CreateUser(newEmail, password);
            ASSERT(!user.IsValid, user);
        }

        public static void TestGetUser()
        {
            Console.Write("Testing GetUser w/ Valid Key: ");
            User user = API.GetUser(GOOD_KEY);
            ASSERT(user.IsValid, user);

            Console.Write("Testing GetUser w/ Invalid Key: ");
            user = API.GetUser(BAD_KEY);
            ASSERT(!user.IsValid, user);
        }

        public static void TestUpdateUser(string name)
        {
            User user = API.GetUser(GOOD_KEY);
            if (!user.IsValid)
            {
                return;
            }

            Console.Write("Testing UpdateUser w/ Valid Key: ");
            user.Name = name;
            user = API.UpdateUser(user, GOOD_KEY);
            ASSERT(
                user.IsValid && user.Name.Equals(
                    name, 
                    StringComparison.InvariantCultureIgnoreCase
                    ),
                user);

            Console.Write("Testing UpdateUser w/ Valid Key + No Update: ");
            user = API.UpdateUser(user, GOOD_KEY);
            ASSERT(user.IsValid, user);

            User goodUser = user;

            Console.Write("Testing UpdateUserName w/ Invalid Key: ");
            user.Name = "Willy Wonka";
            user = API.UpdateUser(user, BAD_KEY);
            ASSERT(!user.IsValid, user);

            user = goodUser;

            Console.Write("Testing UpdateUserEmail w/ Valid Key + Invalid Email: ");
            user.Name = name;
            user.Email = "Bad email";
            user = API.UpdateUser(user, GOOD_KEY);
            ASSERT(!user.IsValid, user);
        }

        #endregion

        public static void Main(string[] args)
        {
            // TestGetBalance();
            // TestGetReceiveAddress();
            // TestGenerateReceiveAddress();
            // TestCreateButton();
            // TestBuyBitcoins();
            // TestGetContacts();
            // TestCurrencyInfo();
            // TestGetOrders();
            // TestGetBuyPrice();
            // TestGetSellPrice();
            // TestSellBitcoins();
            // TestGetTransactions();
            // TestConductTransactions(KEY1, KEY2);
            // TestGetTransfers();
            // TestCreateUser("<INSERT EMAIL>", "<INSERT PASSWORD>");
            // TestGetUser();
            // TestUpdateUser("<INSERT NAME>");       

            Console.ReadLine();
        }
    }
}
