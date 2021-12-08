using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Helpers
{
    public class IMPLibrary
    {
        public const string VERSION = "1.0.0";
        private SortedList<string, string> _requestData;
        private SortedList<string, string> _responseData;

        public IMPLibrary()
        {
            _requestData = new SortedList<string, string>();
            _responseData = new SortedList<string, string>();
        }

        public void AddRequestData(string key, string value)
        {
            if (!_requestData.ContainsKey(key))
                _requestData.Add(key, value);
        }
        public void AddResponse(string key, string value)
        {
            if (!_responseData.ContainsKey(key))
                _responseData.Add(key, value);
        }

        public string GetResponseValue(string key)
        {
            if (_responseData.ContainsKey(key))
            {
                return _responseData[key];
            }
            return string.Empty;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string data = GetResponseData();
            string checksum = Utils.Utils.HmacSHA512(secretKey, data);
            return checksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public string GetResponseData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var keyValue in _responseData)
            {
                if (!string.IsNullOrEmpty(keyValue.Value))
                {
                    sb.Append(WebUtility.UrlEncode(keyValue.Key) + "=" + WebUtility.UrlEncode(keyValue.Value) + "&");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

    }
}
