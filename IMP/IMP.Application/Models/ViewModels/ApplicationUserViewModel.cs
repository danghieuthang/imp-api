﻿using IMP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class ApplicationUserViewModel : BaseViewModel<int>
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public int? LocationId { get; set; }

        public string Job { get; set; }
        public string Interests { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public string Pet { get; set; }

        public string Description { get; set; }

        public int WalletId { get; set; }

        public PaymentInforViewModel PaymentInfor { get; set; }
    }
    public class PaymentInforViewModel : BaseViewModel<int>
    {
        public int? BankId { get; set; }
        public BankViewModel Bank { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
    }

    public class BankViewModel : BaseViewModel<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
