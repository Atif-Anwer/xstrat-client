﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using static WPFSpark.MonitorHelper;

namespace xstrat.Ui
{
    /// <summary>
    /// Interaction logic for HatchControl.xaml
    /// </summary>
    public partial class HatchControl : UserControl
    {
        public Hatchstates[] states = new Hatchstates[] { Hatchstates.solid, Hatchstates.solid, Hatchstates.solid, Hatchstates.solid };

        public bool isLocked = false;

        public HatchControl()
        {
            InitializeComponent();
        }

        private void Rec_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var val = states[0];
            if (states.All(x => x == val))
            {
                for (int i = 0; i < 4; i++)
                {
                    states[i] = increaseHS(val);
                }
            }
        }

        private void Rec1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            states[0] = increaseHS(states[0]);
            UpdateUI();
        }

        private void Rec2_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            states[1] = increaseHS(states[1]);
            UpdateUI();
        }

        private void Rec3_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            states[2] = increaseHS(states[2]);
            UpdateUI();
        }

        private void Rec4_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            states[3] = increaseHS(states[3]);
            UpdateUI();
        }

        private void UpdateUI()
        {
            for (int i = 0; i < 4; i++)
            {
                SetState(i, states[i]);
            }
        }

        private Hatchstates increaseHS(Hatchstates hs )
        {
            int i = (int)hs;
            i++;
            if (i > 3) i = 0;

            return (Hatchstates)i;
        }

        private void SetState(int SPIndex, Hatchstates hs)
        {
            switch (SPIndex)
            {
                case 0:
                    Rec1.Fill = stateToColor(hs);
                    break;
                case 1:
                    Rec2.Fill = stateToColor(hs);
                    break;
                case 2:
                    Rec3.Fill = stateToColor(hs);
                    break;
                case 3:
                    Rec4.Fill = stateToColor(hs);
                    break;
                default:
                    break;
            }
        }
        private SolidColorBrush stateToColor(Hatchstates hs)
        {
            switch (hs)
            {
                case Hatchstates.solid: //blue -> default
                    return new SolidColorBrush(Colors.CornflowerBlue);
                case Hatchstates.removed: //transparent
                    return "#303030".ToSolidColorBrush();
                case Hatchstates.reinforced: //
                    return new SolidColorBrush(Colors.DeepPink);
                default:
                    return new SolidColorBrush(Colors.AliceBlue);
            }
        }
    }
}
