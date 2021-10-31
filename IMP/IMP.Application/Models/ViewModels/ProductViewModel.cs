﻿namespace IMP.Application.Models.Compaign
{
    public class ProductViewModel : BaseViewModel<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }

    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}