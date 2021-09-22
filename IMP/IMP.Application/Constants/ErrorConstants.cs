using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Constants
{
    public static class ErrorConstants
    {
        public static class Identity
        {
            public const int EmailNotFound = 1;
            public const int EmailNotConfirm = 2;
            public const int PasswordIncorrect = 3;
            
        }

        public static class RefreshToken
        {
            public const int RefreshTokenNotExist = 1;
            public const int RefreshTokenWasExpired = 2;

        }
    }
}
