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
using xstrat.Core;

namespace xstrat.Ui
{
    /// <summary>
    /// Interaction logic for TeamMatesControl.xaml
    /// </summary>
    public partial class TeamMatesControl : UserControl
    {
        public TeamMatesControl()
        {
            InitializeComponent();
            RetrieveTeamMatesAsync();
        }
        public async Task RetrieveTeamMatesAsync()
        {
            if (Globals.teammates != null)
            {

                MemberSP.Children.Clear();
                foreach (var user in Globals.teammates)
                {
                    var newItem = new Teammate(user.name, user.id, user.color);
                    newItem.Height = 30;
                    newItem.Width = 250;
                    MemberSP.Children.Add(newItem);
                }
            }
            else
            {
                Notify.sendError("Teammates could not be loaded");
            }
        }
    }
}