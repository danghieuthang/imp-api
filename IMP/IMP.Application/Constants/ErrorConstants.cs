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

        public static class Application
        {
            public static class WalletTransaction
            {
                public const int AmountNotValid = 1;
                public const int BalanceNotEnough = 2;
                public const int ExistWithdrawRequest = 3;
            }
            public static class Page
            {
                public const int BlockIdNotValid = 1;
                public const int BioLinkDuplicate = 2;
                public const int BioLinkNotValid = 3;
            }
            public static class Campaign
            {
                public const int NotFound = 1;
                public const int NotInTimeApply = 2;
                public const int FullInfluencer = 3;
                public const int AlreadyJoined = 4;
                public const int NotSuitable = 5;
            }

        }
    }
}
