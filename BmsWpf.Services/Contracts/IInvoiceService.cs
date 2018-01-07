﻿namespace BmsWpf.Services.Contracts
{
    using BmsWpf.Services.DTOs;
    using System;
    using System.Data;

    public interface IInvoiceService
    {
        string CreateInvoice(InvoicePostDto newInvoice);
        string Delete(int id);
        string EditInvoice(InvoicePostDto newInvoice);
        DataTable GetInvoicesAsDataTable();
        DataTable Search(DateTime datesearch, int idSearch, string searchText);
    }
}