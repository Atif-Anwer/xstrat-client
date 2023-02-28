﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xstrat.MVVM.View;

namespace xstrat.Ui
{
    /// <summary>
    /// Interaction logic for ReplayControl.xaml
    /// </summary>
    public partial class ReplayControl : UserControl
    {
        public MatchReplayFolder Replay { get; set; }

        public ReplayControl()
        {
            InitializeComponent();
        }

    }
}
