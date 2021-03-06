﻿namespace BmsWpf.Services.DTOs
{
    using System;

    public class ProjectsMainWindowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OfferListDto Offer { get; set; }
        public InquiryListDto Inquiry { get; set; }
        public UserListDto Creator { get; set; }
        public ContragentListDto Client { get; set; }
        public string StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Deadline { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public decimal Incomes { get; set; }
        public decimal Expenses { get; set; }
        public decimal FinancialResult
        {
            get
            {
                return Incomes - Expenses;
            }
        }
    }
}
