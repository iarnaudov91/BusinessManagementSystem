﻿namespace BmsWpf.Services.Services
{
    using BMS.DataBaseModels;
    using BmsWpf.Services.Contracts;
    using BmsWpf.Services.DTOs;
    using Microsoft.EntityFrameworkCore;
    using MoreLinq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public class InquiryService : IInquiryService
    {
        private IBmsData bmsData;

        public InquiryService(IBmsData bmsData)
        {
            this.bmsData = bmsData;
        }

        public DataTable GetMainInquiriesInfo()
        {
            var inquiries = this.bmsData
                        .Inquiries
                        .All();
            var inquiriesDtos = inquiries.Select(x => new InquiriesMainWindowDto
                                        {
                                            Id = x.Id,
                                            Creator = new UserListDto()
                                            {
                                                Id = x.Creator.Id,
                                                Username = x.Creator.Username
                                            },
                                            Client = new ContragentListDto()
                                            {
                                                Id = x.Contragent.Id,
                                                NameAndIdentity = x.Contragent.Name + "|" + (x.Contragent.PersonalIndentityNumber == null ? x.Contragent.PersonalVatNumber : x.Contragent.PersonalIndentityNumber)
                                            },
                                            Description = x.Description,
                                            Date = x.Date.ToString("dd/MM/yyyy")
                                        }).ToDataTable();
            return inquiriesDtos;
        }

        public IEnumerable<InquiryListDto> GetInquiriesList()
        {
            var inquiries = this.bmsData
                        .Inquiries
                        .All();
            var inquiriesDtos = inquiries.Select(x => new InquiryListDto
                                            {
                                                Id = x.Id,
                                                Description = x.Description
                                            });
            return inquiriesDtos;
        }

        public string Delete(int id)
        {
            var inquiry = this.bmsData.Inquiries.Find(id);
            var result = string.Empty;
            try
            {
                this.bmsData.Inquiries.Remove(inquiry);
                this.bmsData.SaveChanges();
                result = $"You deleted inquiry {inquiry.Id} from {inquiry.Date.ToShortDateString()} successfully";
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException;
                if (innerException is SqlException)
                {
                    var sqlEx = (SqlException)innerException;
                    if (sqlEx.Errors.Count > 0 && sqlEx.Errors[0].Number == 547) // Foreign Key violation
                    {
                        result = "You cannot delete inquiry that is used in an offer!";
                    }
                }

            }

            return result;
        }

        public string CreateInquiry(InquiryPostDto newInquiry)
        {
            var userSrv = new UserService(bmsData);
            var inquiry = new Inquiry()
            {
                CreatorId = newInquiry.CreatorId,
                ContragentId = newInquiry.ClientId,
                Description = newInquiry.Description,
                Date = newInquiry.Date
            };
            bmsData.Inquiries.Add(inquiry);
            bmsData.SaveChanges();

            return $"Inquiry {newInquiry.Description} from date {newInquiry.Date.ToShortDateString()} successfully created!";
        }

        public string EditInquiry(InquiryPostDto newInquiry)
        {
            var inquiryToUpdate = bmsData.Inquiries.Find(newInquiry.Id);

            inquiryToUpdate.CreatorId = newInquiry.CreatorId;
            inquiryToUpdate.ContragentId = newInquiry.ClientId;
            inquiryToUpdate.Description = newInquiry.Description;
            inquiryToUpdate.Date = newInquiry.Date;

            bmsData.Inquiries.Update(inquiryToUpdate);
            bmsData.SaveChanges();

            return $"Inquiry with ID {newInquiry.Id} successfully updated!";
        }
    }
}
