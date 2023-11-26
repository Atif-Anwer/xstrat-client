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
using xstrat.MVVM.View;

namespace xstrat.Ui
{
    /// <summary>
    /// Interaction logic for WallControl.xaml
    /// </summary>
    public partial class WallControl : UserControl
    {
        public WallStates[] states = new WallStates[] {WallStates.solid, WallStates.solid, WallStates.solid, WallStates.solid, WallStates.solid, WallStates.solid, };
        public string User_ID { get; set; }

        public bool isLocked = false;

        public ImageSource None { get; set; } = null;
        public ImageSource Breach { get; set; }
        public ImageSource Reinforcement { get; set; }
        public ImageSource HeadLevel { get; set; }
        public ImageSource BodyLevel { get; set; }
        public ImageSource FootLevel { get; set; }
        public ImageSource MurderHole { get; set; }
        public ImageSource Mira { get; set; }
        public ImageSource MiraReversed { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="walls"></param>
        public WallControl(WallStates[] walls)
        {
            InitializeComponent();
            Init();
            states = new WallStates[] { walls[0], walls[1], walls[2], walls[3], walls[4], walls[5] };
            UpdateWidth();
            UpdateColor();
        }
        public WallControl()
        {
            InitializeComponent();
            Init();
            UpdateWidth();
            UpdateColor();
        }

        public void Init()
        {
            Breach = GetImageSource("/Images/Icons/wall_breach.png");
            Reinforcement = GetImageSource("/Images/Icons/wall_reinforcement.png");
            HeadLevel = GetImageSource("/Images/Icons/wall_head.png");
            BodyLevel = GetImageSource("/Images/Icons/wall_body.png");
            FootLevel = GetImageSource("/Images/Icons/wall_foot.png");
            MurderHole = GetImageSource("/Images/Icons/wall_murderhole.png");
            Mira = GetImageSource("/Images/Icons/wall_mira.png");
            MiraReversed = GetImageSource("/Images/Icons/wall_mirareversed.png");
        }

        public void UpdateColor()
        {
            if (User_ID.IsNullOrEmpty())
            {

                this.BorderBrush = Brushes.Transparent;
                this.BorderThickness = new Thickness(0);
                return;
            }
            
            var user = DataCache.CurrentTeamMates.Where(x => x.Id == User_ID).FirstOrDefault();
            if (user == null) return;

            var color = user.Color;
            if (color.IsNullOrEmpty()) return;

            this.BorderBrush = color.ToSolidColorBrush();
            this.BorderThickness = new Thickness(2);
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isLocked) return;

            if(Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if(User_ID.IsNotNullOrEmpty() && User_ID == StratMakerToolTipHelper.CurrentBrushUser)
                {
                    User_ID = null;
                    UpdateColor();
                    return;
                }

                User_ID = StratMakerToolTipHelper.CurrentBrushUser;
                UpdateColor();
                return;
            }

            var val = states[0];
            if (states.All(x => x == val))
            {
                if(val >= WallStates.miraReversed)
                {
                    for (int i = 0; i < states.Length; i++)
                    {
                        states[i] = WallStates.solid;
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
                    states[i] = WallStates.solid;
                }
            }
            UpdateUI();
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
                if(states[clickedIndex] > WallStates.mira && states[clickedIndex] < WallStates.punch)
                {
                    states[clickedIndex]++;
                }
                else
                {
                    states[clickedIndex] = WallStates.head;
                }
            }
            UpdateUI();
        }
        public void UpdateUI()
        {
            for (int i = 0; i < states.Length; i++)
            {
                SetState(i, states[i], StateToImage(states[i]));
            }
        }
        private void SetState(int SPIndex, WallStates ws, ImageSource source)
        {
            switch (SPIndex)
            {
                case 0:
                    Rec1.Fill = stateToColor(ws);
                    Icon1.Source = source;
                    break;
                case 1:
                    Rec2.Fill = stateToColor(ws);
                    Icon2.Source = source;
                    break;
                case 2:
                    Rec3.Fill = stateToColor(ws);
                    Icon3.Source = source;
                    break;
                case 3:
                    Rec4.Fill = stateToColor(ws);
                    Icon4.Source = source;
                    break;
                case 4:
                    Rec5.Fill = stateToColor(ws);
                    Icon5.Source = source;
                    break;
                case 5:
                    Rec6.Fill = stateToColor(ws);
                    Icon6.Source = source;
                    break;
                default:
                    break;
            }
        }
        private SolidColorBrush stateToColor(WallStates ws)
        {
            switch (ws)
            {
                case WallStates.solid: //blue -> default
                    return new SolidColorBrush(Colors.CornflowerBlue);
                case WallStates.removed: //transparent
                    return "#303030".ToSolidColorBrush();
                case WallStates.reinforced: //
                    return "#101010".ToSolidColorBrush();
                case WallStates.mira: // mira purple
                    return new SolidColorBrush(Colors.Purple);
                case WallStates.miraReversed: // mira purple
                    return new SolidColorBrush(Colors.Purple);
                case WallStates.head: // Red
                    return new SolidColorBrush(Colors.Red);
                case WallStates.body: // Orange-Red
                    return new SolidColorBrush(Colors.OrangeRed);
                case WallStates.foot: // Orange
                    return new SolidColorBrush(Colors.Orange);
                case WallStates.punch: // default with icon
                    return new SolidColorBrush(Colors.CornflowerBlue);
                default:
                    return new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private ImageSource StateToImage(WallStates ws)
        {
            switch (ws)
            {
                case WallStates.solid:
                    CenterImage.Source = None;
                    CenterImage.Height = 19;
                    CenterImage.Width = 19;
                    CenterIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.None;
                    return None;
                case WallStates.removed:
                    //CenterImage.Source = Breach;
                    CenterImage.Source = None;
                    CenterImage.Height = 22;
                    CenterImage.Width = double.NaN;
                    CenterIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.None;
                    return None;
                case WallStates.reinforced:
                    //CenterImage.Source = Reinforcement;
                    CenterImage.Source = None;
                    CenterImage.Height = 19;
                    CenterImage.Width = this.Width * 0.9;
                    CenterIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Lock;
                    return None;
                case WallStates.mira:
                    CenterImage.Source = Mira;
                    CenterImage.Height = 50;
                    CenterImage.Width = 50;
                    return None;
                case WallStates.miraReversed:
                    CenterImage.Source = MiraReversed;
                    CenterImage.Height = 50;
                    CenterImage.Width = 50;
                    return None;
                case WallStates.head:
                    CenterImage.Source = None;
                    return HeadLevel;
                case WallStates.body:
                    CenterImage.Source = None;
                    return BodyLevel;
                case WallStates.foot:
                    CenterImage.Source = None;
                    return FootLevel;
                case WallStates.punch:
                    CenterImage.Source = None;
                    return MurderHole;
                default:
                    CenterImage.Source = None;
                    return None;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWidth();
        }

        public void UpdateWidth()
        {
            double nwidth = ActualWidth / 6;
            double nheight = ActualHeight;
            Rec1.Width = nwidth;
            Rec2.Width = nwidth;
            Rec3.Width = nwidth;
            Rec4.Width = nwidth;
            Rec5.Width = nwidth;
            Rec6.Width = nwidth;

            Icon1.Width = nwidth;
            Icon2.Width = nwidth;
            Icon3.Width = nwidth;
            Icon4.Width = nwidth;
            Icon5.Width = nwidth;
            Icon6.Width = nwidth;

            Icon1.Height = nheight;
            Icon2.Height = nheight;
            Icon3.Height = nheight;
            Icon4.Height = nheight;
            Icon5.Height = nheight;
            Icon6.Height = nheight;
        }

        public ImageSource GetImageSource(string relativePath)
        {
            Uri uri = new Uri(relativePath, UriKind.Relative);
            BitmapImage bitmapImage = new BitmapImage(uri);

            return bitmapImage;
        }
    }
}
