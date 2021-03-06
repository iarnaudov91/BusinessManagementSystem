﻿namespace BmsWpf.Views.ChildWindows
{
    using BmsWpf.Services.Contracts;
    using BmsWpf.ViewModels;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainOffers.xaml
    /// </summary>
    public partial class MainOffers : Window
    {
        public Action CloseAction { get; set; }
        public IViewManager ViewManager { get; set; }

        public MainOffers()
        {
            InitializeComponent();
        }

        public MainOffers(IViewManager viewManager, IOfferService offerService)
        {
            InitializeComponent();

            MainOffersViewModel vm = (MainOffersViewModel)this.DataContext; // this creates an instance of the ViewModel

            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());

            vm.ViewManager = viewManager;
            vm.OfferService = offerService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void Add_New_Click(object sender, RoutedEventArgs e)
        //{
        //    var dash = new OfferForm();
        //    dash.Show();
        //    this.Close();
        //}

        //private void Back_click(object sender, RoutedEventArgs e)
        //{
        //    var dash = new MainWindow();
        //    dash.Show();
        //    this.Close();
        //}

        //public void fillingDataGrid()
        //{
        //    var db = new BmsContex();
        //    DataTable dt = new DataTable();
        //    DataColumn offerId = new DataColumn("Id", typeof(int));
        //    DataColumn creator = new DataColumn("Creator", typeof(string));
        //    DataColumn client = new DataColumn("Client", typeof(string));
        //    DataColumn inquiry = new DataColumn("InquiryID", typeof(string));
        //    DataColumn description = new DataColumn("Description", typeof(string));
        //    DataColumn date = new DataColumn("Date", typeof(DateTime));

        //    dt.Columns.Add(offerId);
        //    dt.Columns.Add(creator);
        //    dt.Columns.Add(client);
        //    dt.Columns.Add(inquiry);
        //    dt.Columns.Add(description);
        //    dt.Columns.Add(date);


        //    var offers = db.Offers.Include(e => e.Client).Include(e => e.Creator).Include(e => e.Inquiry).ToArray();

        //    foreach (var offer in offers)
        //    {
        //        DataRow row = dt.NewRow();
        //        row[0] = offer.Id;
        //        row[1] = offer.Creator.Username;
        //        row[2] = offer.Client.Name;
        //        row[3] = offer.Inquiry.Description;
        //        row[4] = offer.Description;
        //        row[5] = offer.Date;
        //        dt.Rows.Add(row);
        //    }

        //    OffersDataGrid.ItemsSource = dt.DefaultView;
        //}

        //private void OffersDataGrid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    this.fillingDataGrid();
        //}


        //private void Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    EditForm();
        //}

        //private void OffersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    EditForm();
        //}

        //private void EditForm()
        //{
        //    var dash = new OfferForm();
        //    DataRowView dataRow = (DataRowView)OffersDataGrid.SelectedItem;
        //    dash.offer_id.Text = dataRow.Row.ItemArray[0].ToString();
        //    dash.CreatorCombo.SelectedItem = dataRow.Row.ItemArray[1].ToString();
        //    dash.ClientCombo.SelectedItem = dataRow.Row.ItemArray[2].ToString();
        //    dash.InquiryCombo.SelectedItem = dataRow.Row.ItemArray[3].ToString();
        //    dash.offer_description.Text = dataRow.Row.ItemArray[4].ToString();
        //    dash.Show();
        //    this.Close();
        //}


        //private void Delete_Click(object sender, RoutedEventArgs e)
        //{
        //    var db = new BmsContex();
        //    DataRowView dataRow = (DataRowView)OffersDataGrid.SelectedItem;
        //    var itemId = dataRow.Row.ItemArray[0].ToString();
        //    var currentOffer = db.Offers.Where(o => o.Id.ToString() == itemId).SingleOrDefault();
        //    db.Offers.Remove(currentOffer);
        //    db.SaveChanges();
        //    MessageBox.Show("Offer Removed Successfully!");
        //    var dash = new MainOffers();
        //    dash.Show();
        //    this.Close();
        //}
    }
}
