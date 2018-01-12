﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsWpf.ViewModels
{
    public class PFExpensesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public string TabTitle { get; protected set; }

        public PFExpensesViewModel()
        {
            TabTitle = "Expenses";
        }
    }
}