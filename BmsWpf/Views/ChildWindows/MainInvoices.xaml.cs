﻿namespace BmsWpf.Views.ChildWindows
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using BmsWpf.Views.Forms;

    using BMS.DataBaseData;

    /// <summary>
    /// Interaction logic for MainInvoices.xaml
    /// </summary>
    public partial class MainInvoices : Window
    {
        public MainInvoices()
        {
            InitializeComponent();
        }

        private void EditSupplierInvoiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddClientInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var clientInvoice = new InvoiceClientForm();
            clientInvoice.Show();
            this.ClientInvoicesGrid.ItemsSource = clientInvoice.Content.ToString();
        }

        private void EditClientInvoicesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddSupplierInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var supplierInvoice = new InvoinceSupplierForm();
            supplierInvoice.Show();
            this.ClientInvoicesGrid.ItemsSource = supplierInvoice.Content.ToString();

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var context = new BmsContex();
            var find = this.SearchTextBox.Text;
            var dateSearch = DateTime.ParseExact(find, "dd/MM/yy", CultureInfo.InvariantCulture);
            var numberSearch = int.Parse(find);

            var found = context.Invoices.Select(
                s => s.Date == dateSearch || s.ClientContragent.Name == find || s.Id == numberSearch).ToList();
            foreach (var f in found)
            {
                this.ClientInvoicesGrid.ItemsSource = f.ToString();
            }


        }

        private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            var dash = new MainWindow();
            dash.Show();
            this.Close();
        }
    }
}