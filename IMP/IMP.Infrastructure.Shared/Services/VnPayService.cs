using IMP.Application.Interfaces;
using IMP.Application.Utils;
using IMP.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Shared.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPaySettings _vnPaySettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _returnUrl;
        private string _ipAddress;

        public VnPayService(IOptions<VnPaySettings> options, IHttpContextAccessor httpContextAccessor)
        {
            _vnPaySettings = options.Value;
            _httpContextAccessor = httpContextAccessor;
            InitVariables();
        }
        private void InitVariables()
        {
            _returnUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
            if (string.IsNullOrEmpty(_returnUrl))
            {
                _returnUrl = "http://localhost";
            }
            else
            {
                _returnUrl = "https://" + _returnUrl;
            }
            _returnUrl = _returnUrl + "/api/v1/wallet-transactions/confirm-transaction-vnpay";

            _ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        }
        public string GetServerIp()
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            if (string.IsNullOrEmpty(host) || host.Contains("localhost"))
            {
                return "127.0.0.1";
            }
            return Dns.GetHostAddresses(_httpContextAccessor.HttpContext.Request.Host.Value).ToString();
        }


        public string CreatePaymentUrl(int amount, int walletId, string paymentInfo, string locale)
        {
            VnPayLibrary vnPay = new();

            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", _vnPaySettings.Vnp_TmnCode);
            vnPay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            vnPay.AddRequestData("vnp_BankCode", "");
            vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", _ipAddress);
            if (!string.IsNullOrEmpty(locale))
            {
                vnPay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnPay.AddRequestData("vnp_Locale", "vn");
            }
            vnPay.AddRequestData("vnp_OrderInfo", paymentInfo);
            vnPay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnPay.AddRequestData("vnp_ReturnUrl", _returnUrl);
            vnPay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString() + "_" + walletId); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            vnPay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            return vnPay.CreateRequestUrl(_vnPaySettings.Vnp_Url, _vnPaySettings.Vnp_HashSecret);
        }

        public async Task<bool> VerifyPaymentTransaction(Dictionary<string, string> requestTransactionData, string sercureHash)
        {
            VnPayLibrary vnPay = new();
            foreach (var item in requestTransactionData)
            {
                vnPay.AddResponseData(item.Key, item.Value);
            }
            var isValidateSignature = vnPay.ValidateSignature(sercureHash, secretKey: _vnPaySettings.Vnp_HashSecret);
            if (isValidateSignature)
            {
                var responseTranactionData = await QueryTransacion(requestTransactionData);

                if (responseTranactionData != null
                    && responseTranactionData["vnp_Amount"] == requestTransactionData["vnp_Amount"]
                    && responseTranactionData["vnp_TxnRef"] == requestTransactionData["vnp_TxnRef"]
                    && responseTranactionData["vnp_PayDate"] == requestTransactionData["vnp_PayDate"]
                    // vnp_TransactionStatus: Success - 00
                    && responseTranactionData["vnp_TransactionStatus"] == "00")
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<Dictionary<string, string>> QueryTransacion(Dictionary<string, string> data)
        {
            VnPayLibrary vnPay = new();

            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "querydr");
            vnPay.AddRequestData("vnp_TmnCode", _vnPaySettings.Vnp_TmnCode);
            vnPay.AddRequestData("vnp_TxnRef", data["vnp_TxnRef"]);
            vnPay.AddRequestData("vnp_OrderInfo", "Query transaction No: " + data["vnp_TransactionNo"]);
            vnPay.AddRequestData("vnp_TransDate", data["vnp_PayDate"]);
            vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_IpAddr", GetServerIp());

            string url = vnPay.CreateRequestUrl(baseUrl: _vnPaySettings.Querydr, vnp_HashSecret: _vnPaySettings.Vnp_HashSecret);
            var vnPayQueryTransactionResponse = await GetResponseFromUrl(url);
            if (vnPayQueryTransactionResponse != null)
            {
                //foreach (var item in vnPayQueryTransactionResponse)
                //{
                //    vnPay.AddResponseData(item.Key, item.Value);
                //}
                //// If signature are correct
                //if (vnPay.ValidateSignature(vnPayQueryTransactionResponse["vnp_SecureHash"], secretKey: _vnPaySettings.Vnp_HashSecret))
                //{
                //    return vnPayQueryTransactionResponse;
                //}
                return vnPayQueryTransactionResponse;
            }
            return null;
        }

        private async Task<Dictionary<string, string>> GetResponseFromUrl(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Dictionary<string, string> data = new();
                foreach (string item in content.Split("&"))
                {
                    string[] keyAndValue = item.Split("=");
                    data.Add(keyAndValue[0], keyAndValue[1]);
                }
                return data;
            }
            return null;
        }

    }

    public class VnPayLibrary
    {
        public const string VERSION = "2.1.0";
        private SortedList<String, String> _requestData = new SortedList<String, String>(new VnPayCompare());
        private SortedList<String, String> _responseData = new SortedList<String, String>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }

        #region Request

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();
            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }


        #endregion

        #region Response process
        /// <summary>
        /// Check ensure that data of the transaction is not changed during the transaction the process of from VNPAY To server
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            bool result = myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }
        private string GetResponseData()
        {

            StringBuilder data = new StringBuilder();
            //if (_responseData.ContainsKey("vnp_SecureHashType"))
            //{
            //    _responseData.Remove("vnp_SecureHashType");
            //}
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }
        #endregion
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
