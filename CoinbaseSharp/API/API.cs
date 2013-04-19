using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Runtime.Serialization.Json;
using CoinbaseSharp.Authentication;
using CoinbaseSharp.DataTypes;
using CoinbaseSharp.DataTypes.Requests;
using CoinbaseSharp.DataTypes.Responses;
using CoinbaseSharp.Resources;

namespace CoinbaseSharp.API
{
    public static class HttpWebRequestExtensions
    {
        public static void UploadData(
            this HttpWebRequest request, 
            object data,
            Type dataType
            )
        {
            if (dataType != null && data != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = 
                        new DataContractJsonSerializer(dataType);
                    serializer.WriteObject(ms, data);
                    request.ContentLength = ms.Length;
                        
                    ms.Position = 0;

                    using (Stream stream = request.GetRequestStream())
                    {
                        ms.CopyTo(stream);
                    }
                }
            }
        }

        public static string DownloadResponse(
            this HttpWebRequest request
            )
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public static object DownloadResponse(
            this HttpWebRequest request,
            Type responseType
            )
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer serializer = 
                        new DataContractJsonSerializer(responseType);
                    return serializer.ReadObject(stream);
                }
            }
        }
    }

    public class API
    {
        #region Request Destination Endpoints

        public static string CoinbaseURL
        {
            get
            {
                return "https://coinbase.com/api/v1/";
            }
        }

        public static int Version
        {
            get
            {
                return 1;
            }
        }

        public static HttpWebRequest CreateRequest(
            string controller,
            string action,
            IDictionary<string, string> parameters,
            APIKey apiKey
            )
        {
            return CreateRequest(controller, action, null, parameters, apiKey);
        }

        public static HttpWebRequest CreateRequest(
            string controller,
            string subcontroller,
            string action,
            IDictionary<string, string> parameters,
            APIKey apiKey
            )
        {
            var builder = new UriBuilder(CoinbaseURL);

            if (!string.IsNullOrWhiteSpace(controller))
            {
                if (!string.IsNullOrWhiteSpace(subcontroller))
                {
                    builder.Path += (string.IsNullOrWhiteSpace(action))
                        ? string.Format("{0}/{1}", controller, subcontroller)
                        : string.Format("{0}/{1}/{2}", controller, subcontroller, action);
                }
                else
                {
                    builder.Path += controller;
                }
            }

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (apiKey != null)
            {
                queryParams["api_key"] = apiKey.KeyValue;
            }

            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    queryParams[key] = parameters[key];
                }
            }

            builder.Query = queryParams.ToString();

            return WebRequest.CreateHttp(builder.Uri);
        }

        #endregion

        #region HttpGet

        public static object GetResource(
            string controller,
            Type type
            )
        {
            return GetResource(controller, type, null);
        }

        public static object GetResource(
            string controller,
            Type type,
            APIKey apiKey
            )
        {
            return GetResource(controller, null, type, apiKey);
        }

        public static object GetResource(
            string controller,
            string action,
            Type type
            )
        {
            return GetResource(controller, action, type, null);
        }

        public static object GetResource(
            string controller,
            string action,
            Type type,
            APIKey apiKey
            )
        {
            return GetResource(controller, action, null, type, apiKey);
        }

        public static object GetResource(
            string controller,
            string action,
            IDictionary<string, string> parameters,
            Type type
            )
        {
            return GetResource(controller, action, parameters, type, null);
        }

        public static object GetResource(
            string controller,
            string action,
            IDictionary<string, string> parameters,
            Type type,
            APIKey apiKey
            )
        {
            HttpWebRequest request = CreateRequest(
                controller, 
                action, 
                parameters, 
                apiKey
                );
            try
            {
                return request.DownloadResponse(type);
            }
            catch (WebException ex)
            {
                var constructor = type.GetConstructor(
                    new Type[] { typeof(WebException) }
                    );
                return constructor.Invoke(new object[] { ex });
            }
        }

        #endregion

        #region HttpPost

        public static object PostResource(
            string controller,
            string action,
            Type responseType
            )
        {
            return PostResource(controller, action, responseType, null);
        }

        public static object PostResource(
            string controller,
            string action,
            Type responseType,
            APIKey apiKey
            )
        {
            return PostResource(controller, action, null, null, responseType, apiKey);
        }

        public static object PostResource(
            string controller,
            object postObj,
            Type typeOfPostObj,
            Type responseType
            )
        {
            return PostResource(controller, postObj, typeOfPostObj, responseType, null);
        }

        public static object PostResource(
            string controller,
            object postObj,
            Type typeOfPostObj,
            Type responseType,
            APIKey apiKey
            )
        {
            return PostResource(controller, null, postObj, typeOfPostObj, responseType, apiKey);
        }

        public static object PostResource(
            string controller,
            string action,
            object postObj,
            Type typeOfPostObj,
            Type responseType
            )
        {
            return PostResource(controller, action, postObj, typeOfPostObj, responseType, null);
        }

        public static object PostResource(
            string controller,
            string action,
            object postObj,
            Type typeOfPostObj,
            Type responseType,
            APIKey apiKey
            )
        {
            return PostResource(controller, action, null, postObj, typeOfPostObj, responseType, apiKey);
        }

        public static object PostResource(
           string controller,
           string action,
           IDictionary<string, string> parameters,
           object postObj,
           Type typeOfPostObj,
           Type responseType
           )
        {
            return PostResource(controller, action, parameters, postObj, typeOfPostObj, responseType, null);
        }

        public static object PostResource(
            string controller,
            string action,
            IDictionary<string, string> parameters,
            object postObj,
            Type typeOfPostObj,
            Type responseType,
            APIKey apiKey
            )
        {
            HttpWebRequest request = CreateRequest(
                controller, 
                action, 
                parameters, 
                apiKey
                );
            request.Method = "POST";
            request.ContentType = "application/json";

            try
            {
                request.UploadData(postObj, typeOfPostObj);
                return request.DownloadResponse(responseType);
            }
            catch (WebException ex)
            {
                var constructor = responseType.GetConstructor(
                    new Type[] { typeof(WebException) }
                    );
                return constructor.Invoke(new object[] { ex });
            }
        }

        #endregion

        #region HttpPut

        public static object PutResource(
            string controller,
            string subcontroller,
            string action,
            Type responseType,
            APIKey apiKey
            )
        {
            return PutResource(controller, subcontroller, action, null, null, null, responseType, apiKey);
        }

        public static object PutResource(
            string controller,
            string subcontroller,
            object postObj,
            Type typeOfPostObj,
            Type responseType,
            APIKey apiKey
            )
        {
            return PutResource(
                controller,
                subcontroller,
                null,
                null,
                postObj,
                typeOfPostObj,
                responseType,
                apiKey
                );
        }

        public static object PutResource(
            string controller,
            string subcontroller,
            string action,
            IDictionary<string, string> parameters,
            object postObj,
            Type typeOfPostObj,
            Type responseType,
            APIKey apiKey
            )
        {
            HttpWebRequest request = CreateRequest(
                controller, 
                subcontroller, 
                action, 
                parameters, 
                apiKey
                );
            request.Method = "PUT";
            request.ContentType = "application/json";

            try
            {
                request.UploadData(postObj, typeOfPostObj);
                return request.DownloadResponse(responseType);
            }
            catch (WebException ex)
            {
                var constructor = responseType.GetConstructor(
                    new Type[] { typeof(WebException) }
                    );
                return constructor.Invoke(new object[] { ex });
            }
        }

        #endregion

        #region HttpDelete

        public static object DeleteResource(
            string controller,
            string subcontroller,
            string action,
            Type responseType,
            APIKey apiKey
            )
        {
            return DeleteResource(
                controller,
                subcontroller,
                action,
                null,
                responseType,
                apiKey
                );
        }

        public static object DeleteResource(
            string controller,
            string subcontroller,
            string action,
            IDictionary<string, string> parameters,
            Type responseType,
            APIKey apiKey
            )
        {
            HttpWebRequest request = CreateRequest(controller, subcontroller, action, parameters, apiKey);
            request.Method = "DELETE";
            request.ContentType = "application/json";
            try
            {
                return request.DownloadResponse(responseType);
            }
            catch (WebException ex)
            {
                var constructor = responseType.GetConstructor(new Type[] { typeof(WebException) });
                return constructor.Invoke(new object[] { ex });
            }
        }

        #endregion

        #region ACCOUNT

        public static Amount GetBalance(APIKey apiKey)
        {
            return (Amount) GetResource(
                "account", 
                "balance", 
                typeof(Amount), 
                apiKey
                );
        }
        
        public static ReceiveAddress GetReceiveAddress(APIKey apiKey)
        {
            return (ReceiveAddress) GetResource(
                "account", 
                "receive_address", 
                typeof(ReceiveAddress), 
                apiKey
                );
        }

        public static ReceiveAddress GenerateReceiveAddress(APIKey apiKey)
        {
            return (ReceiveAddress)PostResource(
                "account", 
                "generate_receive_address", 
                typeof(ReceiveAddress), 
                apiKey
                );
        }

        #endregion
        
        #region BUTTONS

        public static Button CreatePaymentButton(
            string name,
            Amount cost,
            string description,
            APIKey apiKey
            )
        {
            Button button = new Button()
            {
                Name = name,
                Cost = cost.AmountValue,
                PriceCurrencyISO = cost.Currency,
                Description = description
            };

            return CreatePaymentButton(button, apiKey);
        }

        public static Button CreatePaymentButton(
            Button button,
            APIKey apiKey
            )
        {
            ButtonResponse buttonResponse = (ButtonResponse) PostResource(
                "buttons", 
                button, 
                typeof(Button), 
                typeof(ButtonResponse), 
                apiKey
                );

            return buttonResponse.Button;
        }

        #endregion
        
        #region BUYS

        public static Transfer BuyBitcoins(decimal qty, APIKey apiKey)
        {
            Quantity quantity = new Quantity() { Value = qty };
            return BuyBitcoins(quantity, apiKey);
        }

        public static Transfer BuyBitcoins(Quantity quantity, APIKey apiKey)
        {
            TransferResponse transferResponse = (TransferResponse)PostResource(
                "buys",
                quantity,
                typeof(Quantity),
                typeof(TransferResponse),
                apiKey
                );

            return transferResponse.Transfer;
        }

        #endregion
      
        #region CONTACTS

        public static Contacts GetContacts(APIKey apiKey)
        {
            return (Contacts) GetResource("contacts", typeof(Contacts), apiKey);
        }

        public static Contacts GetContacts(int page, APIKey apiKey)
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();
            return (Contacts) GetResource(
                "contacts", 
                null, 
                dict, 
                typeof(Contacts), 
                apiKey
                );
        }
        
        public static Contacts GetContacts(int page, int limit, APIKey apiKey)
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();
            dict["limit"] = Math.Min(limit, 1000).ToString();
            return (Contacts) GetResource(
                "contacts", 
                null, 
                dict, 
                typeof(Contacts), 
                apiKey
                );
        }

        public static Contacts GetContacts(
            int page, 
            int limit, 
            string query, 
            APIKey apiKey
            )
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();
            dict["limit"] = Math.Min(limit, 1000).ToString();
            dict["query"] = query;
            return (Contacts) GetResource(
                "contacts", 
                null, 
                dict, 
                typeof(Contacts), 
                apiKey
                );
        }

        public static Contacts GetContacts(string query, APIKey apiKey)
        {
            var dict = new Dictionary<string, string>();
            dict["query"] = query;
            return (Contacts) GetResource(
                "contacts", 
                null, 
                dict, 
                typeof(Contacts), 
                apiKey
                );
        }

        public static Contacts GetContacts(
            int page, 
            string query, 
            APIKey apiKey
            )
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();
            dict["query"] = query;
            return (Contacts)GetResource(
                "contacts", 
                null, 
                dict, 
                typeof(Contacts), 
                apiKey
                );
        }

        #endregion
        
        #region CURRENCIES

        public static List<Currency> GetCurrencies()
        {
            HttpWebRequest request = CreateRequest("currencies", null, null, null);
            try
            {
                string text = request.DownloadResponse();
                return text.Split(new string[] { "[[\"", "\"],[\"", "\"]]" }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(x => x.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries))
                           .Select(y => new Currency() { Name = y[0], ISO = y[1] })
                           .ToList();
            }
            catch (WebException)
            {
                return null;
            }
        }

        public static Dictionary<string, Dictionary<string, decimal>> GetExchangeRates()
        {
            HttpWebRequest request = CreateRequest("currencies", "exchange_rates", null, null);
            try
            {
                string text = request.DownloadResponse();
                string[] tokens = text.Split(new string[] { "{\"", "\",\"", "\"}" }, StringSplitOptions.RemoveEmptyEntries);
                var dict = new Dictionary<string, Dictionary<string, decimal>>();
                foreach (string token in tokens)
                {
                    string[] fields = token.Split(new string[] { "\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] isos = fields[0].Split(new string[] { "_to_" }, StringSplitOptions.RemoveEmptyEntries);
                    decimal amount = Decimal.Parse(fields[1], NumberStyles.Any);
                    if (!dict.ContainsKey(isos[0]))
                    {
                        dict[isos[0]] = new Dictionary<string, decimal>();
                    }
                    dict[isos[0]][isos[1]] = amount;
                }
                return dict;
            }
            catch (WebException)
            {
                return null;
            }
        }

        #endregion
        
        #region ORDERS

        public static Orders GetOrders(APIKey apiKey)
        {
            return (Orders) GetResource("orders", typeof(Orders), apiKey);
        }

        public static Orders GetOrders(int page, APIKey apiKey)
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();
            return (Orders)GetResource(
                "orders", 
                null, 
                dict, 
                typeof(Orders), 
                apiKey
                );
        }

        public static Order GetOrder(string order, APIKey apiKey)
        {
            OrderResponse orderResponse = (OrderResponse) GetResource(
                "orders", 
                order, 
                typeof(OrderResponse), 
                apiKey
                );

            return orderResponse.Order;
        }

        #endregion ORDERS
        
        #region PRICES

        public static Amount GetBuyPrice(decimal quantity)
        {
            var dict = new Dictionary<string, string>();
            dict["qty"] = quantity.ToString();
            return (Amount)GetResource("prices", "buy", dict, typeof(Amount));
        }

        public static Amount GetSellPrice(decimal quantity)
        {
            var dict = new Dictionary<string, string>();
            dict["qty"] = quantity.ToString();
            return (Amount)GetResource("prices", "sell", dict, typeof(Amount));
        }

        #endregion
        
        #region SELLS

        public static Transfer SellBitcoins(decimal qty, APIKey apiKey)
        {
            Quantity quantity = new Quantity() { Value = qty };
            return SellBitcoins(quantity, apiKey);
        }

        public static Transfer SellBitcoins(Quantity quantity, APIKey apiKey)
        {
            TransferResponse transferResponse = (TransferResponse) PostResource(
                "sells", 
                quantity, 
                typeof(Quantity), 
                typeof(TransferResponse), 
                apiKey
                );

            return transferResponse.Transfer;
        }

        #endregion
        
        #region TRANSACTIONS

        public static Transactions GetTransactions(APIKey apiKey)
        {
            return (Transactions) GetResource(
                "transactions", 
                typeof(Transactions), 
                apiKey
                );
        }

        public static Transactions GetTransactions(int page, APIKey apiKey)
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();

            return (Transactions)GetResource(
                "transactions",
                null,
                dict,
                typeof(Transactions),
                apiKey
                );
        }

        public static Transaction GetTransaction(string transaction, APIKey apiKey)
        {
            TransactionResponse transactionResponse = (TransactionResponse) GetResource(
                "transactions", 
                transaction, 
                typeof(TransactionResponse), 
                apiKey
                );

            return transactionResponse.Transaction;
        }

        public static Transaction SendMoney(SendMoneyTransaction transaction, APIKey apiKey)
        {
            SendMoneyTransactionRequest request = new SendMoneyTransactionRequest()
            { 
                Transaction = transaction 
            };
            TransactionResponse transactionResponse = (TransactionResponse) PostResource(
                "transactions", 
                "send_money", 
                request, 
                typeof(SendMoneyTransactionRequest), 
                typeof(TransactionResponse), 
                apiKey
                );

            return transactionResponse.Transaction;
        }

        public static Transaction RequestMoney(RequestMoneyTransaction transaction, APIKey apiKey)
        {
            RequestMoneyTransactionRequest request = new RequestMoneyTransactionRequest()
            { 
                Transaction = transaction
            };

            TransactionResponse transactionResponse = (TransactionResponse)
                PostResource(
                "transactions", 
                "request_money",
                request, 
                typeof(RequestMoneyTransactionRequest), 
                typeof(TransactionResponse), 
                apiKey
                );

            return transactionResponse.Transaction;
        }

        public static CoinbaseResponse ResendRequest(string request, APIKey apiKey)
        {
            return (CoinbaseResponse) PutResource(
                "transactions", 
                request, 
                "resend_request", 
                typeof(CoinbaseResponse), 
                apiKey
                );
        }

        public static CoinbaseResponse CancelRequest(string request, APIKey apiKey)
        {
            return (CoinbaseResponse)DeleteResource(
                "transactions",
                request,
                "cancel_request",
                typeof(CoinbaseResponse),
                apiKey
                );
        }

        public static CoinbaseResponse CompleteRequest(string request, APIKey apiKey)
        {
            return (CoinbaseResponse)PutResource(
                "transactions",
                request,
                "complete_request",
                typeof(CoinbaseResponse),
                apiKey
                );
        }

        #endregion

        #region TRANSFERS

        public static Transfers GetTransfers(APIKey apiKey)
        {
            return (Transfers)GetResource(
                "transfers",
                typeof(Transfers),
                apiKey
                );
        }

        public static Transfers GetTransfers(int page, APIKey apiKey)
        {
            var dict = new Dictionary<string, string>();
            dict["page"] = page.ToString();

            return (Transfers)GetResource(
                "transfers",
                null,
                dict,
                typeof(Transfers),
                apiKey
                );
        }

        #endregion

        #region USERS

        public static User CreateUser(string email, string password)
        {
            User user = new User() { Email = email, Password = password };
            return CreateUser(user);
        }

        public static User CreateUser(User user)
        {
            UserResponse userResponse = (UserResponse)PostResource(
                "users",
                user,
                typeof(User),
                typeof(UserResponse)
                );

            return userResponse.User;
        }

        public static User GetUser(APIKey apiKey)
        {
            Users users = (Users)GetResource(
                "users",
                typeof(Users),
                apiKey
                );
            return users.CurrentUser;
        }

        public static User UpdateUser(
            User user,
            APIKey apiKey
            )
        {
            User updatedUser = new User()
            {
                Email = user.Email,
                Name = user.Name,
                NativeCurrency = user.NativeCurrency,
                TimeZone = user.TimeZone
            };

            UserResponse userResponse = (UserResponse)PutResource(
                "users",
                user.ID,
                updatedUser,
                typeof(User),
                typeof(UserResponse),
                apiKey
                );

            return userResponse.User;
        }

        #endregion
    }
}
