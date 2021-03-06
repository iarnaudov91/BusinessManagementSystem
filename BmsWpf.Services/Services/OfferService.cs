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

    public class OfferService : IOfferService
    {
        private IBmsData bmsData;

        public OfferService(IBmsData bmsData)
        {
            this.bmsData = bmsData;
        }

        public DataTable GetOffersAsDataTable()
        {
            var offers = this.bmsData
                            .Offers
                            .All();
            var dataTable = offers.Select(x => new
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
                InquiryID = new InquiryListDto()
                {
                    Id = x.Inquiry.Id,
                    Description = x.Inquiry.Description
                },
                x.Description,
                Date = x.Date.ToString("dd/MM/yyyy")
            })
                                    .ToDataTable();
            return dataTable;
        }

        public string Delete(int id)
        {
            var offer = this.bmsData.Offers.Find(id);
            var result = string.Empty;
            try
            {
                this.bmsData.Offers.Remove(offer);
                this.bmsData.SaveChanges();
                result = $"You deleted offer {offer.Id} from {offer.Date.ToShortDateString()} successfully";
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException;
                if (innerException is SqlException)
                {
                    var sqlEx = (SqlException)innerException;
                    if (sqlEx.Errors.Count > 0 && sqlEx.Errors[0].Number == 547) // Foreign Key violation
                    {
                       result = "You cannot delete offer that is a part of a project!";
                    }
                }
            }

            return result ;
        }

        public string CreateOffer(OfferPostDto newOffer)
        {
            var offer = new Offer()
            {
                ContragentId = newOffer.ClientId,
                CreatorId = newOffer.CreatorId,
                InquiryId = newOffer.InquiryId,
                Description = newOffer.Description,
                Date = newOffer.Date
            };

            this.bmsData.Offers.Add(offer);
            this.bmsData.SaveChanges();

            return $"Offer \"{newOffer.Description}\" from date {newOffer.Date.ToShortDateString()} successfully created!";
        }

        public string EditOffer(OfferPostDto newOffer)
        {
            var offer = bmsData.Offers.Find(newOffer.Id);

            offer.InquiryId = newOffer.InquiryId;
            offer.CreatorId = newOffer.CreatorId;
            offer.ContragentId = newOffer.ClientId;
            offer.Description = newOffer.Description;
            offer.Date = newOffer.Date;

            this.bmsData.Offers.Update(offer);
            this.bmsData.SaveChanges();

            return $"Offer with Id: {newOffer.Id} successfully updated!";
        }

        public IEnumerable<OfferListDto> GetOffersList()
        {
            var offers = this.bmsData.Offers.All();
            var offersList = offers.Select(x => new OfferListDto
            {
                Id = x.Id,
                Description = x.Description
            });

            return offersList;
        }
    }
}
