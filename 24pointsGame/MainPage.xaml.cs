using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups; // Library for displaying errors and notices

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _24pointsGame
{
    // Initialising new class, which allows to pass multiple values throug navigation
    public class gameData
    {
        public string playerName { get; set; }
        public string gameMode { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Getting name of current player from gameField, if User went back from game to main page
            string currentName = e.Parameter.ToString();
            nameBox.Text = currentName; // Storing this value to textbox to let user use previous name
        }

        private async void playBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text == "") // validation. If name is empty, user will get error popup
            {
                // Popup notice
                MessageDialog validControl = new MessageDialog("Please type your name", "Error");
                await validControl.ShowAsync();
            }
            else // If username is OK, user will be redirected to new game
            {
                gameData gData = new gameData() { playerName = nameBox.Text, gameMode = modeCombo.SelectionBoxItem.ToString()  };
                Frame.Navigate(typeof(gameField), gData); // passing name of player and game difficulty mode
            }
        }

        // Showing links to recourses
        private async void creditsBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Popup credits
            MessageDialog validControl = new MessageDialog("• The game has been created by Ilia Pikulev." + Environment.NewLine + "• E-mail: tiz00001eu@myntec.ac.nz" + Environment.NewLine + "• Game card images: http://www.snap2objects.com/2012/01/free-vector-playing-cards-deck/" + Environment.NewLine + "• Game background image: http://giozaga.deviantart.com/art/Casino-Card-Background-Wallpaper-HD-1920x1080-454608180" + Environment.NewLine + "• Thank you for playing!", "24 Points | Credits");
            await validControl.ShowAsync();
        }
    }
}
