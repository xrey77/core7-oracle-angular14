using System;
using System.Globalization;

namespace core7_oracle_angular14.Helpers
{
    
    public class AppException : Exception
    {

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }

}