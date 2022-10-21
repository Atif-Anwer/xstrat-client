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
    /// Interaction logic for WallControl.xaml
    /// </summary>
    public partial class WallControl : UserControl
    {
        public Wallstates[] states = new Wallstates[] {Wallstates.solid, Wallstates.solid, Wallstates.solid, Wallstates.solid, Wallstates.solid, Wallstates.solid, };

        public bool isLocked = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="walls"></param>
        public WallControl(Wallstates[] walls)
        {
            InitializeComponent();
            states = new Wallstates[] { walls[0], walls[1], walls[2], walls[3], walls[4], walls[5] };
            updateWidth();
        }
        public WallControl()
        {
            InitializeComponent();
            updateWidth();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isLocked) return;
            var val = states[0];
            if (states.All(x => x == val))
            {
                if(val >= Wallstates.mira)
                {
                    for (int i = 0; i < states.Length; i++)
                    {
                        states[i] = Wallstates.solid;
                    }
                }
                else
                {
                    for (int i = 0; i < states.Length; i++)
                    {
                        states[i]++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < states.Length; i++)
                {
                    states[i] = Wallstates.solid;
                }
            }
            UpdateGUI();
        }

        private void Rec_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isLocked) return;
        }


        private void StackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isLocked) return;
            int clickedIndex = SP.Children.IndexOf((UIElement)e.OriginalSource);
            if(clickedIndex < states.Length && clickedIndex >= 0)
            {
                if(states[clickedIndex] > Wallstates.mira && states[clickedIndex] < Wallstates.punch)
                {
                    states[clickedIndex]++;
                }
                else
                {
                    states[clickedIndex] = Wallstates.head;
                }
            }
            UpdateGUI();
        }
        private void UpdateGUI()
        {
            for (int i = 0; i < states.Length; i++)
            {
                SetState(i, states[i], stateToIcon(states[i]));
            }
        }
        private void SetState(int SPIndex, Wallstates ws, MaterialDesignThemes.Wpf.PackIconKind icon)
        {
            switch (SPIndex)
            {
                case 0:
                    Rec1.Fill = stateToColor(ws);
                    Sym1.Kind = icon;
                    break;
                case 1:
                    Rec2.Fill = stateToColor(ws);
                    Sym2.Kind = icon;
                    break;
                case 2:
                    Rec3.Fill = stateToColor(ws);
                    Sym3.Kind = icon;
                    break;
                case 3:
                    Rec4.Fill = stateToColor(ws);
                    Sym4.Kind = icon;
                    break;
                case 4:
                    Rec5.Fill = stateToColor(ws);
                    Sym5.Kind = icon;
                    break;
                case 5:
                    Rec6.Fill = stateToColor(ws);
                    Sym6.Kind = icon;
                    break;
                default:
                    break;
            }
        }
        private SolidColorBrush stateToColor(Wallstates ws)
        {
            switch (ws)
            {
                case Wallstates.solid: //blue -> default
                    return new SolidColorBrush(Colors.CornflowerBlue);
                case Wallstates.removed: //transparent
                    return "#303030".ToSolidColorBrush();
                case Wallstates.reinforced: //
                    return "#101010".ToSolidColorBrush();
                case Wallstates.mira: // mira purple
                    return new SolidColorBrush(Colors.Purple);
                case Wallstates.head: // Red
                    return new SolidColorBrush(Colors.Red);
                case Wallstates.body: // Orange-Red
                    return new SolidColorBrush(Colors.OrangeRed);
                case Wallstates.foot: // Orange
                    return new SolidColorBrush(Colors.Orange);
                case Wallstates.punch: // default with icon
                    return new SolidColorBrush(Colors.CornflowerBlue);
                default:
                    return new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private MaterialDesignThemes.Wpf.PackIconKind stateToIcon(Wallstates ws)
        {
            switch (ws)
            {
                case Wallstates.solid:
                    SymAll.Kind = MaterialDesignThemes.Wpf.PackIconKind.None;
                    return MaterialDesignThemes.Wpf.PackIconKind.None;
                case Wallstates.removed:
                    SymAll.Kind = MaterialDesignThemes.Wpf.PackIconKind.Reload;
                    return MaterialDesignThemes.Wpf.PackIconKind.None;
                case Wallstates.reinforced:
                    SymAll.Kind = MaterialDesignThemes.Wpf.PackIconKind.LockOutline;
                    return MaterialDesignThemes.Wpf.PackIconKind.None;
                case Wallstates.mira:
                    SymAll.Kind = MaterialDesignThemes.Wpf.PackIconKind.RectangleOutline;
                    return MaterialDesignThemes.Wpf.PackIconKind.None;
                case Wallstates.head:
                    return MaterialDesignThemes.Wpf.PackIconKind.HeadPlusOutline;
                case Wallstates.body:
                    return MaterialDesignThemes.Wpf.PackIconKind.Person;
                case Wallstates.foot:
                    return MaterialDesignThemes.Wpf.PackIconKind.ShoePrint;
                case Wallstates.punch:
                    return MaterialDesignThemes.Wpf.PackIconKind.EyeArrowLeftOutline;
                default:
                    return MaterialDesignThemes.Wpf.PackIconKind.None;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateWidth();
        }

        public void updateWidth()
        {
            double nwidth = ActualWidth / 6;
            Rec1.Width = nwidth;
            Rec2.Width = nwidth;
            Rec3.Width = nwidth;
            Rec4.Width = nwidth;
            Rec5.Width = nwidth;
            Rec6.Width = nwidth;

            Sym1.Width = nwidth;
            Sym2.Width = nwidth;
            Sym3.Width = nwidth;
            Sym4.Width = nwidth;
            Sym5.Width = nwidth;
            Sym6.Width = nwidth;
        }
    }
}
