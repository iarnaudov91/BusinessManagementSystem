﻿namespace BmsWpf.ViewModels
{
    using BmsWpf.Behaviour;
    using BmsWpf.Services.Contracts;
    using BmsWpf.Sessions;
    using BmsWpf.Views.ChildWindows;
    using BmsWpf.Views.Forms;
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Input;

    public class MainInquiriesViewModel : ViewModelBase, IPageViewModel
    {
        private DataTable inquiries;
        private DataRowView selectedInquiry;
        private string inquiryCreator;

        public ICommand WindowLoadedCommand;
        public ICommand DoubleClickCommand;
        public ICommand AddNewCommand;
        public ICommand EditCommand;
        public ICommand DeleteCommand;
        public ICommand BackCommand;

        public IInquiryService InquiryService { get; set; }
        public IViewManager ViewManager { get; set; }

        public Action CloseAction { get; set; }

        public string ViewName
        {
            get
            {
                return "Inquiries main page";
            }
        }

        public DataRowView SelectedInquiry
        {
            get
            {
                return this.selectedInquiry;
            }
            set
            {
                this.selectedInquiry = value;
                this.OnPropertyChanged(nameof(SelectedInquiry));
            }
        }

        public DataTable Inquiries
        {
            get
            {
                return inquiries;
            }
            private set
            {
                this.inquiries = value;
                this.OnPropertyChanged(nameof(Inquiries));
            }
        }

        public string InquiryCreator
        {
            get
            {
                return this.inquiryCreator;
            }
            set
            {
                this.inquiryCreator = value;
                this.OnPropertyChanged(nameof(InquiryCreator));
            }
        }

        public ICommand WindowLoaded
        {
            get
            {
                if (this.WindowLoadedCommand == null)
                {
                    this.WindowLoadedCommand = new RelayCommand(this.HandleLoadedCommand);
                }
                return this.WindowLoadedCommand;
            }
        }

        public ICommand DoubleClick
        {
            get
            {
                if (this.DoubleClickCommand == null)
                {
                    this.DoubleClickCommand = new RelayCommand(this.HandleEditCommand);
                }
                return this.DoubleClickCommand;
            }
        }

        public ICommand AddNew
        {
            get
            {
                if (this.AddNewCommand == null)
                {
                    this.AddNewCommand = new RelayCommand(this.HandleAddNewCommand);
                }
                return this.AddNewCommand;
            }
        }

        public ICommand Edit
        {
            get
            {
                if (this.EditCommand == null)
                {
                    this.EditCommand = new RelayCommand(this.HandleEditCommand);
                }
                return this.EditCommand;
            }
        }

        public ICommand Delete
        {
            get
            {
                if (this.DeleteCommand == null)
                {
                    this.DeleteCommand = new RelayCommand(this.HandleDeleteCommand);
                }
                return this.DeleteCommand;
            }
        }

        public ICommand Back
        {
            get
            {
                if (this.BackCommand == null)
                {
                    this.BackCommand = new RelayCommand(this.HandleBackCommand);
                }
                return this.BackCommand;
            }
        }

        private void HandleLoadedCommand(object parameter)
        {
            this.Inquiries = InquiryService.GetMainInquiriesInfo();
        }

        private void HandleAddNewCommand(object parameter)
        {
            var addNewInquiryWindow = this.ViewManager.ComposeObjects<InquireForm>();
            var vm = (OffersFormViewModel)addNewInquiryWindow.DataContext;
            vm.InquiryCreator = Session.Instance.Username;
            addNewInquiryWindow.Show();
            this.CloseAction();
        }

        private void HandleEditCommand(object parameter)
        {
            if (this.SelectedInquiry == null)
            {
                MessageBox.Show("Please select an inquiry to continue");
                return;
            }
            var addNewInquiryWindow = this.ViewManager.ComposeObjects<InquireForm>();
            var vm = (OffersFormViewModel)addNewInquiryWindow.DataContext;
            vm.SelectedInquiry = this.SelectedInquiry;
            vm.InquiryCreator = Session.Instance.Username;
            addNewInquiryWindow.Show();
            this.CloseAction();
        }

        private void HandleDeleteCommand(object parameter)
        {
            var inquiryId = (int)selectedInquiry.Row.ItemArray[0];

            var result = string.Empty;

            try
            {
                result = this.InquiryService.Delete(inquiryId);
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            MessageBox.Show(result);
            var inquiriesWindow = ViewManager.ComposeObjects<MainInquiries>();
            inquiriesWindow.Show();
            this.CloseAction();
        }

        private void HandleBackCommand(object parameter)
        {
            var mainWindow = this.ViewManager.ComposeObjects<MainWindow>();
            mainWindow.Show();
            this.CloseAction();
        }
    }
}
