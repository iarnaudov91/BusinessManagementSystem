﻿namespace BmsWpf.Services.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InquiryPostDto
    {
        private int creatorId;
        private int clientId;
        private string description;

        public int Id { get; set; }

        public int CreatorId {
            get
            {
                return this.creatorId;
            }
            set
            {
                if (value == 0)
                {
                    throw new ApplicationException("You haven't chosen a Creator!");
                }
                this.creatorId = value;
            }
        }
        public int ClientId {
            get
            {
                return this.clientId;
            }
            set
            {
                if (value == 0)
                {
                    throw new ApplicationException("You haven't chosen a Client!");
                }
                this.clientId = value;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (value.Length < 3 || string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
                {
                    throw new ApplicationException("Description have to be more then 3 symbols");
                }

                this.description = value;
            }
        }
        public DateTime Date { get; set; }
    }
}
