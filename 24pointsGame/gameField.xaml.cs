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
using Windows.UI.Xaml.Media.Imaging; // Adding library to work with images

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _24pointsGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class gameField : Page
    {
        public gameField()
        {
            this.InitializeComponent();
        }

        // Assigning new list for store occupied cards after each draw
        public List<int> excludedNumbers = new List<int> { 0 };

        // Declaring variables to store name and score of current player and score of the opponent 
        public string plrName;
        public string difficulty;
        public int playerScore;
        public int opponentScore;

        // Declaring Final Result for each round
        private int finalResult;

        // Declaring booleans, which will identify occupied cards
        private int occupiedCard1 = 0;
        private int occupiedCard2 = 0;
        private int occupiedCard3 = 0;
        private int occupiedCard4 = 0;
        private int occupiedResult1 = 0;
        private int occupiedResult2 = 0;

        // Declaring cards unique numbers
        public int numberCard1;
        public int numberCard2;
        public int numberCard3;
        public int numberCard4;

        // Declaring cards values to use in equation
        public int valueCard1;
        public int valueCard2;
        public int valueCard3;
        public int valueCard4;

        // Preparing items for equation
        public int firstValue = 0;
        public int secondValue = 0;
        public int eqType = 0;

        // Function to get random values
        public void randomCard()
        {
            // Assign new method to generate randoms
            Random rnd = new Random();
            do // Generate random number for first card
            {
                numberCard1 = rnd.Next(3, 55);
            } while (excludedNumbers.Contains(numberCard1));

            do // Generate random number for second card
            {
                numberCard2 = rnd.Next(3, 55);
            } while (excludedNumbers.Contains(numberCard2) || numberCard2 == numberCard1);

            do // Generate random number for third card
            {
                numberCard3 = rnd.Next(3, 55);
            } while (excludedNumbers.Contains(numberCard3) || numberCard3 == numberCard2 || numberCard3 == numberCard1);

            do // Generate random number for fourth card
            {
                numberCard4 = rnd.Next(3, 55);
            } while (excludedNumbers.Contains(numberCard4) || numberCard4 == numberCard3 || numberCard4 == numberCard2 || numberCard4 == numberCard1);

        }

        // Showing operator's images (+, -, *, /) for equation
        private void showOperators()
        {
            imageAdd.Visibility = Visibility.Visible;
            imageDelete.Visibility = Visibility.Visible;
            imageMultiply.Visibility = Visibility.Visible;
            imageSubstract.Visibility = Visibility.Visible;
        }

        // Show button to store value
        private void showEquals()
        {
            // Show EqBtn, if all needed values are filled and ready for equation
            if ( firstValue != 0 && secondValue != 0 && eqType != 0)
            {
                equalsBtn.Visibility = Visibility.Visible;
            }

            // Put first calculated value into block
            if (blockResult1.Text != "") {
                int checkResult1 = Convert.ToInt32(blockResult1.Text.ToString());

                if (checkResult1 == 0)
                {
                    equalsBtn.Visibility = Visibility.Visible;
                }
            }

            // Put second calculated value into second block
            if (blockResult2.Text != "")
            {
                int checkResult2 = Convert.ToInt32(blockResult2.Text.ToString());

                // Show eqBtn, when two blocks are filled with some values
                if (checkResult2 == 0)
                {
                    equalsBtn.Visibility = Visibility.Visible;
                }
            }
        }

        // Declaring variables to store them into blocks to identify current conditions
        private string operatorSymbol;
        private string fval;
        private string sval;

        // Put values into blocks and variables (function performs almost for each action)
        private void putValues()
        {
            // Show operator type, if one of operator's buttons was tapped
            switch(eqType)
            {
                case 0:
                    operatorSymbol = "";
                    break;
                case 1:
                    operatorSymbol = "+";
                    break;
                case 2:
                    operatorSymbol = "-";
                    break;
                case 3:
                    operatorSymbol = "*";
                    break;
                case 4:
                    operatorSymbol = "/";
                    break;
            }

            // Show first value, if it's not empty
            if (firstValue == 0)
            {
                // Show ZEROs in equation (first value), if one of the blocks contains ZERO
                if ((blockResult1.Text != "" && Convert.ToInt32(blockResult1.Text.ToString()) == 0) || (blockResult2.Text != "" && Convert.ToInt32(blockResult2.Text.ToString()) == 0) )
                {
                    fval = "0";
                } else
                {
                    fval = "";
                }
            } else
            {
                fval = Convert.ToString(firstValue);
            }

            // Show second value, if it's not empty
            if (secondValue == 0)
            {
                // Show ZEROs in equation (second value), if one of the blocks contains ZERO
                if ((blockResult1.Text != "" && Convert.ToInt32(blockResult1.Text.ToString()) == 0) || (blockResult2.Text != "" && Convert.ToInt32(blockResult2.Text.ToString()) == 0))
                {
                    sval = "0";
                }
                else
                {
                    sval = "";
                }
            } else
            {
                sval = Convert.ToString(secondValue);
            }

            // Show equation above the cards to help understand current process
            eqBlock.Text = fval + "  " + operatorSymbol + "  " + sval;

            // Hide and show big number at the bottom-right
            if (excludedNumbers.Count <= 1)
            {
                finalResultBlock.Text = "";
            } else
            {
                finalResultBlock.Text = Convert.ToString(finalResult);
            }

            // Show background rectangle behind the block with first result
            if (blockResult1.Text != "")
            {
                blockResult1Bkg.Visibility = Visibility.Visible;
            } else
            {
                blockResult1Bkg.Visibility = Visibility.Collapsed;
            }

            // Show background rectangle behind the block with second result
            if (blockResult2.Text != "")
            {
                blockResult2Bkg.Visibility = Visibility.Visible;
            }
            else
            {
                blockResult2Bkg.Visibility = Visibility.Collapsed;
            }

            // Show green 24 score, if player got it and used all cards
            if (finalResult == 24 && occupiedCard1 == 2 && occupiedCard2 == 2 && occupiedCard3 == 2 && occupiedCard4 == 2 && occupiedResult1 == 2 && occupiedResult2 == 2)
            {
                finalResultBlock.Visibility = Visibility.Collapsed;
                _24ResultBlock.Visibility = Visibility.Visible;
            } else
            {
                finalResultBlock.Visibility = Visibility.Visible;
                _24ResultBlock.Visibility = Visibility.Collapsed;
            }
        }

        // Set 80% opacity to operators
        private void setOpacity()
        {
            imageAdd.Opacity = 0.8;
            imageDelete.Opacity = 0.8;
            imageMultiply.Opacity = 0.8;
            imageSubstract.Opacity = 0.8;
        }

        // Set 100% opacity to cards
        private void unsetTransparency()
        {
            imageCard1.Opacity = 1;
            imageCard2.Opacity = 1;
            imageCard3.Opacity = 1;
            imageCard4.Opacity = 1;
        }

        //Enable all cards and result1, result2
        private void enableCards()
        {
            imageCard1.IsTapEnabled = true;
            imageCard2.IsTapEnabled = true;
            imageCard3.IsTapEnabled = true;
            imageCard4.IsTapEnabled = true;

            occupiedCard1 = 0;
            occupiedCard2 = 0;
            occupiedCard3 = 0;
            occupiedCard4 = 0;

            occupiedResult1 = 0;
            occupiedResult2 = 0;
        }

        // Clear all data for the next equation
        private void clearGameData()
        {
            // Hiding operators
            imageAdd.Visibility = Visibility.Collapsed;
            imageDelete.Visibility = Visibility.Collapsed;
            imageMultiply.Visibility = Visibility.Collapsed;
            imageSubstract.Visibility = Visibility.Collapsed;

            // Unsetting firstValue, secondValue, eqType
            firstValue = 0;
            secondValue = 0;
            eqType = 0;

            // Clear eqBlock above cards
            eqBlock.Text = "";

            setOpacity();

            // Clear all occupied cards
            if (occupiedCard1 == 1)
            {
                occupiedCard1 = 0;
            }

            if (occupiedCard2 == 1)
            {
                occupiedCard2 = 0;
            }

            if (occupiedCard3 == 1)
            {
                occupiedCard3 = 0;
            }

            if (occupiedCard4 == 1)
            {
                occupiedCard4 = 0;
            }

            if (occupiedResult1 == 1)
            {
                occupiedResult1 = 0;
            }

            if (occupiedResult2 == 1)
            {
                occupiedResult2 = 0;
            }
        }

        // Declaring result variable to store it into textBlocks
        private decimal resultValue;

        // Calculate result
        private async void calculateResult()
        {
            // Perform calculation for particular chosen operator
            switch(eqType)
            {
                case 0:
                    MessageDialog error = new MessageDialog("You forgot to choose operator", "Error");
                    await error.ShowAsync();
                    break;
                case 1:
                    resultValue = firstValue + secondValue;
                    break;
                case 2:
                    resultValue = firstValue - secondValue;
                    break;
                case 3:
                    resultValue = firstValue * secondValue;
                    break;
                case 4:
                    int checking = firstValue / secondValue;
                    if (checking * secondValue == firstValue)
                    {
                        resultValue = firstValue / secondValue;
                    }
                    else
                    {
                        // Show message, if the result is not integer
                        MessageDialog mistake = new MessageDialog("The result should be an integer digit... The round will be restarted.", "Error");
                        await mistake.ShowAsync();

                        restartRound();
                    }
                    break;
            }

            // Store value into big textBlock at the bottom-right
            finalResult = Convert.ToInt32(resultValue);
        }

        // Block clicked cards or resulting blocks, if they are clicked
        private void performCardBlocking()
        {
            if (occupiedCard1 == 1 || occupiedCard1 == 2)
            {
                imageCard1.IsTapEnabled = false;
                imageCard1.Opacity = 0.6;
            }
            else
            {
                imageCard1.IsTapEnabled = true;
                imageCard1.Opacity = 1;
            }

            if (occupiedCard2 == 1 || occupiedCard2 == 2)
            {
                imageCard2.IsTapEnabled = false;
                imageCard2.Opacity = 0.6;
            }
            else
            {
                imageCard2.IsTapEnabled = true;
                imageCard2.Opacity = 1;
            }

            if (occupiedCard3 == 1 || occupiedCard3 == 2)
            {
                imageCard3.IsTapEnabled = false;
                imageCard3.Opacity = 0.6;
            }
            else
            {
                imageCard3.IsTapEnabled = true;
                imageCard3.Opacity = 1;
            }

            if (occupiedCard4 == 1 || occupiedCard4 == 2)
            {
                imageCard4.IsTapEnabled = false;
                imageCard4.Opacity = 0.6;
            }
            else
            {
                imageCard4.IsTapEnabled = true;
                imageCard4.Opacity = 1;
            }

            if (occupiedResult1 == 1 || occupiedResult1 == 2)
            {
                blockResult1.IsTapEnabled = false;
                blockResult1.Opacity = 0.4;
            }
            else
            {
                blockResult1.IsTapEnabled = true;
                blockResult1.Opacity = 1;
            }

            if (occupiedResult2 == 1 || occupiedResult2 == 2)
            {
                blockResult2.IsTapEnabled = false;
                blockResult2.Opacity = 0.4;
            }
            else
            {
                blockResult2.IsTapEnabled = true;
                blockResult2.Opacity = 1;
            }
        }

        // Putting values for each particular card
        public int  get_value(int number)
        {
            int cardValue = 0;

            // Put card value, accordingly to generated random integer
            switch(number)
            {
                case 0:
                    cardValue = 0;
                    break;
                case 1:
                    cardValue = 0;
                    break;
                case 2:
                    cardValue = 0;
                    break;
                case 3:
                    cardValue = 10;
                    break;
                case 4:
                    cardValue = 2;
                    break;
                case 5:
                    cardValue = 3;
                    break;
                case 6:
                    cardValue = 4;
                    break;
                case 7:
                    cardValue = 5;
                    break;
                case 8:
                    cardValue = 6;
                    break;
                case 9:
                    cardValue = 7;
                    break;
                case 10:
                    cardValue = 8;
                    break;
                case 11:
                    cardValue = 9;
                    break;
                case 12:
                    cardValue = 1;
                    break;
                case 13:
                    cardValue = 11;
                    break;
                case 14:
                    cardValue = 13;
                    break;
                case 15:
                    cardValue = 12;
                    break;
                case 16:
                    cardValue = 10;
                    break;
                case 17:
                    cardValue = 2;
                    break;
                case 18:
                    cardValue = 3;
                    break;
                case 19:
                    cardValue = 4;
                    break;
                case 20:
                    cardValue = 5;
                    break;
                case 21:
                    cardValue = 6;
                    break;
                case 22:
                    cardValue = 7;
                    break;
                case 23:
                    cardValue = 8;
                    break;
                case 24:
                    cardValue = 9;
                    break;
                case 25:
                    cardValue = 1;
                    break;
                case 26:
                    cardValue = 11;
                    break;
                case 27:
                    cardValue = 13;
                    break;
                case 28:
                    cardValue = 12;
                    break;
                case 29:
                    cardValue = 10;
                    break;
                case 30:
                    cardValue = 2;
                    break;
                case 31:
                    cardValue = 3;
                    break;
                case 32:
                    cardValue = 4;
                    break;
                case 33:
                    cardValue = 5;
                    break;
                case 34:
                    cardValue = 6;
                    break;
                case 35:
                    cardValue = 7;
                    break;
                case 36:
                    cardValue = 8;
                    break;
                case 37:
                    cardValue = 9;
                    break;
                case 38:
                    cardValue = 1;
                    break;
                case 39:
                    cardValue = 11;
                    break;
                case 40:
                    cardValue = 13;
                    break;
                case 41:
                    cardValue = 12;
                    break;
                case 42:
                    cardValue = 10;
                    break;
                case 43:
                    cardValue = 2;
                    break;
                case 44:
                    cardValue = 3;
                    break;
                case 45:
                    cardValue = 4;
                    break;
                case 46:
                    cardValue = 5;
                    break;
                case 47:
                    cardValue = 6;
                    break;
                case 48:
                    cardValue = 7;
                    break;
                case 49:
                    cardValue = 8;
                    break;
                case 50:
                    cardValue = 9;
                    break;
                case 51:
                    cardValue = 1;
                    break;
                case 52:
                    cardValue = 11;
                    break;
                case 53:
                    cardValue = 13;
                    break;
                case 54:
                    cardValue = 12;
                    break;
            }
            return cardValue;
        }

        // Disabling used in equation cards till the end of the round
        private void disableUsedCards()
        {
            if (occupiedCard1 == 1)
            {
                occupiedCard1 = 2;
            }

            if (occupiedCard2 == 1)
            {
                occupiedCard2 = 2;
            }

            if (occupiedCard3 == 1)
            {
                occupiedCard3 = 2;
            }

            if (occupiedCard4 == 1)
            {
                occupiedCard4 = 2;
            }

            if (occupiedResult1 == 1)
            {
                occupiedResult1 = 2;
            }

            if (occupiedResult2 == 1)
            {
                occupiedResult2 = 2;
            }
        }

        // Flush values to store them again in future
        private void deleteEquation()
        {
            if (firstValue > 0 && secondValue > 0)
            {
                clearGameData();
                performCardBlocking();

                equalsBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void storeScore()
        {
            if (playerScore == 0) // Hide block, if score is empty
            {
                ScoreBlock.Text = "";
                scoreBkg.Visibility = Visibility.Collapsed;
            }
            else // Show block, if score more, than 0
            {
                ScoreBlock.Text = "Game score: " + Convert.ToString(playerScore);
                scoreBkg.Visibility = Visibility.Visible;
            }

            if (opponentScore == 0) // Hide block, if opponents score is empty
            {
                opScoreBlock.Text = "";
                opScoreBkg.Visibility = Visibility.Collapsed;
            }
            else // Show block, if opponents score more, than 0
            {
                opScoreBlock.Text = "Opponent score: " + Convert.ToString(opponentScore);
                opScoreBkg.Visibility = Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Turning all card backside up
            numberCard1 = numberCard2 = numberCard3 = numberCard4 = 2; // Number of backside
            // Links to the images respectively to number of the cards
            imageCard1.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard1 + ".png"));
            imageCard2.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard2 + ".png"));
            imageCard3.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard3 + ".png"));
            imageCard4.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard4 + ".png"));

            // Disable cards in NULL round
            imageCard1.IsTapEnabled = false;
            imageCard2.IsTapEnabled = false;
            imageCard3.IsTapEnabled = false;
            imageCard4.IsTapEnabled = false;
        }

        // Restart round function
        private void restartRound()
        {
            // Clear result blocks and enable them
            blockResult1.Text = "";
            blockResult2.Text = "";
            blockResult1.IsTapEnabled = true;
            blockResult2.IsTapEnabled = true;

            // Clear big number at the bottom-right
            finalResult = 0;

            // Hide restartBtn and equalsBtn
            restartRoundBtn.Visibility = Visibility.Collapsed;
            equalsBtn.Visibility = Visibility.Collapsed;

            clearGameData();
            performCardBlocking();
            unsetTransparency();
            enableCards();
            putValues();
        }

        // Setup timer variables
        DispatcherTimer gameTimer;
        int timesTicked;
        int timesToTick = 0;

        // Creating timer for each draw
        private void inGameTimer()
        {
            // Stop previous timer, if the game is already running
            if (excludedNumbers.Count > 1)
            {
                gameTimer.Stop();
            }

            // Set proper time for Medium and hard modes
            if (difficulty == "Medium")
            {
                timerBlock.Text = "Next draw in: 300";
                timesTicked = 299;
            }
            else if (difficulty == "Hard")
            {
                timerBlock.Text = "Next draw in: 90";
                timesTicked = 89;
            }

            // Assign new timer
            gameTimer = new DispatcherTimer();
            if (!gameTimer.IsEnabled)
            {
                gameTimer.Tick += gameTimer_Tick;
            }
            //Set intervals and start the count
            gameTimer.Interval = new TimeSpan(0, 0, 1);
            gameTimer.Start();
        }

        // Set conditions, when the timer is ticking
        private void gameTimer_Tick(object sender, object e)
        {
            timerBlock.Text = "Next draw in: " + timesTicked.ToString();
            // Stop timer, if the count is 0
            if (timesTicked == timesToTick)
            {
                gameTimer.Stop();

                // Launch next round
                nextDraw();
            }
            timesTicked--;
        }

        private async void nextDraw()
        {
            // Launch timer for Meduim and hard modes only
            if (difficulty != "Easy")
            {
                inGameTimer();
            }

            // Clear blocks
            restartRoundBtn.Visibility = Visibility.Collapsed;
            blockResult1.Text = "";
            blockResult2.Text = "";

            // Calculate players and opponents score
            if (finalResult == 24 && occupiedCard1 == 2 && occupiedCard2 == 2 && occupiedCard3 == 2 && occupiedCard4 == 2 && occupiedResult1 == 2 && occupiedResult2 == 2)
            {
                playerScore += valueCard1 + valueCard2 + valueCard3 + valueCard4;
            }
            else
            {
                opponentScore += valueCard1 + valueCard2 + valueCard3 + valueCard4;
            }

            storeScore();

            // Clear big number at the bottom-right
            finalResult = 0;

            enableCards();

            unsetTransparency();

            clearGameData();

            // Getting four random numbers for cards
            randomCard();

            // Putting used card numbers into Excluded list
            excludedNumbers.Insert(1, numberCard1);
            excludedNumbers.Insert(1, numberCard2);
            excludedNumbers.Insert(1, numberCard3);
            excludedNumbers.Insert(1, numberCard4);

            // Changing card number respectively to randomised number
            imageCard1.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard1 + ".png"));
            imageCard2.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard2 + ".png"));
            imageCard3.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard3 + ".png"));
            imageCard4.Source = new BitmapImage(new Uri(base.BaseUri, "Assets/images/Cards/" + numberCard4 + ".png"));

            // Getting values for random drawn cards
            valueCard1 = get_value(numberCard1);
            valueCard2 = get_value(numberCard2);
            valueCard3 = get_value(numberCard3);
            valueCard4 = get_value(numberCard4);

            // Putting values to text blocks beneath the cards
            blockCard1.Text = valueCard1.ToString();
            blockCard2.Text = valueCard2.ToString();
            blockCard3.Text = valueCard3.ToString();
            blockCard4.Text = valueCard4.ToString();

            // Change round number
            roundBlock.Text = "Round: " + Convert.ToString((excludedNumbers.Count - 1) / 4) + "/13";

            // Check the end of the full list
            if (excludedNumbers.Count >= 53)
            {
                // Disabling the timer
                if (difficulty == "Medium" || difficulty == "Hard")
                {
                    gameTimer.Stop();
                    timerBlock.Text = "";
                }

                // Show message at the last round
                MessageDialog end = new MessageDialog("This is the last round... Do not hurry. You have got plenty of time!", "Last round!");
                await end.ShowAsync();

                // Disable drawBtn and enable button, which finishes the game
                drawBtn.IsEnabled = false;
                drawBtn.Visibility = Visibility.Collapsed;
                gameBtn.Visibility = Visibility.Visible;
            }
            else
            {
                // Let the drawBtn be enabled
                drawBtn.IsEnabled = true;
                gameBtn.Visibility = Visibility.Collapsed;
            }

            putValues();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // getting values from object, which was passed during navigation to this page
            gameData gData = (gameData)e.Parameter;
            plrName = gData.playerName;
            difficulty = gData.gameMode;

            // Notice about player and game Difficulty at the bottom-right area of the screen
            nameBlock.Text = plrName + " is now playing";
            modeBlock.Text = gData.gameMode + " level";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to main page and passing name og current player back
            Frame.Navigate(typeof(MainPage), plrName );
        }

        private void drawBtn_Click(object sender, RoutedEventArgs e)
        {
            // Perform the next round
            nextDraw();
        }

        private async void rulesBtn_Click(object sender, RoutedEventArgs e)
        {
            // Showing game rules after clicking Rules button
            MessageDialog rules = new MessageDialog("• This game is kind of similar to 21 points and tests your math skills." + Environment.NewLine + "• All that you need is to calculate ALL four drawn cards in any sequence (+, -, *, / and brackets are allowed) to get 24 points." + Environment.NewLine + "• Just follow the sequence, as you are doing it on the peace of paper." + Environment.NewLine + "• The person, who has calculated the proper equation take drawn cards, and the next four cards go to the table." + Environment.NewLine + "• The winner is the person, who holds the biggest number of points, calculated by adding values from cards." + Environment.NewLine + "• Card values: 2-10 as they are, Jack - 11, Queen - 12, King - 13, Ace - 1" + Environment.NewLine + "• Game has 13 rounds. Press DRAW to start the first one." + Environment.NewLine + "• When you used all cards and got 24 points, press DRAW again to save the score and to go to the next round." + Environment.NewLine + "• NOTE: If you haven't got 24 points in one equation and press DRAW, the score will go to your opponent." + Environment.NewLine + "• NOTE: If you got 24 points, but you haven't used all the cards in one equation, after pushing the DRAW button the score will go to your opponent." + Environment.NewLine + "• EXAMPLE: If you see cards with values 12, 6, 4, 4, you may calculate them, as (12/6+4)*4=24. To achieve it just use the sequence: divide card 12 into card 6 (result:2), then add value 2 to card 4 (result:6) and then multiply value 6 by second card 4.", "Rules");
            await rules.ShowAsync();
        }

        private void imageCard1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deleteEquation();

            // Block current card and change appearance of all cards
            occupiedCard1 = 1;
            performCardBlocking();

            showOperators();

            // Pass value to first or second value
            if (firstValue == 0)
            {
                firstValue = valueCard1;
            } else
            {
                secondValue = valueCard1;
            }

            // Show equals button
            showEquals();
            putValues();
        }

        private void imageCard2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deleteEquation();

            // Block current card and change appearance of all cards
            occupiedCard2 = 1;
            performCardBlocking();

            showOperators();

            // Pass value to first or second value
            if (firstValue == 0)
            {
                firstValue = valueCard2;
            }
            else
            {
                secondValue = valueCard2;
            }

            // Show equals button
            showEquals();
            putValues();
        }

        private void imageCard3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deleteEquation();

            // Block current card and change appearance of all cards
            occupiedCard3 = 1;
            performCardBlocking();

            showOperators();

            // Pass value to first or second value
            if (firstValue == 0)
            {
                firstValue = valueCard3;
            }
            else
            {
                secondValue = valueCard3;
            }

            // Show equals button
            showEquals();
            putValues();
        }

        private void imageCard4_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deleteEquation();

            // Block current card and change appearance of all cards
            occupiedCard4 = 1;
            performCardBlocking();

            showOperators();

            // Pass value to first or second value
            if (firstValue == 0)
            {
                firstValue = valueCard4;
            }
            else
            {
                secondValue = valueCard4;
            }

            // Show equals button
            showEquals();
            putValues();
        }

        private void playBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Clear the current active equation
            equalsBtn.Visibility = Visibility.Collapsed;
            clearGameData();
            performCardBlocking();
            putValues();
        }

        // Addd actions, when -> button is clicked
        private void equalsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            disableUsedCards();

            // Put the result into first or second result blocks
            calculateResult();
            if (blockResult1.Text == "")
            {
                blockResult1.Text = Convert.ToString(resultValue);
            }
            else
            {
                blockResult2.Text = Convert.ToString(resultValue);
            }

            // Show restartBtn and hide eqBtn
            restartRoundBtn.Visibility = Visibility.Visible;
            equalsBtn.Visibility = Visibility.Collapsed;
            clearGameData();
            putValues();
        }

        private void restartRoundBtn_Click(object sender, RoutedEventArgs e)
        {
            restartRound();
        }

        private void againBtn_Click(object sender, RoutedEventArgs e)
        {
            // Go back to main page
            Frame.Navigate(typeof(MainPage), plrName);
        }

        // Perform substract action for two values
        private void imageSubstract_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            setOpacity();
            imageSubstract.Opacity = 1;
            eqType = 2;

            showEquals();
            putValues();
        }

        // Perform multiply action for two values
        private void imageMultiply_Tapped(object sender, TappedRoutedEventArgs e)
        {
            setOpacity();
            imageMultiply.Opacity = 1;
            eqType = 3;

            showEquals();
            putValues();
        }

        // Perform delete action for two values
        private void imageDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            setOpacity();
            imageDelete.Opacity = 1;
            eqType = 4;

            showEquals();
            putValues();
        }

        // Perform add action for two values
        private void imageAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            setOpacity();
            imageAdd.Opacity = 1;
            eqType = 1;

            showEquals();
            putValues();
        }

        private void blockResult1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deleteEquation();
            occupiedResult1 = 1;
            performCardBlocking();

            showOperators();

            // Pass value to first or second value
            if (firstValue == 0)
            {
                firstValue = Convert.ToInt32(blockResult1.Text);
            }
            else
            {
                secondValue = Convert.ToInt32(blockResult1.Text);
            }

            // Show equals button
            showEquals();
            putValues();
        }

        private void blockResult2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deleteEquation();
            occupiedResult2 = 1;
            performCardBlocking();

            showOperators();

            // Pass value to first or second value
            if (firstValue == 0)
            {
                firstValue = Convert.ToInt32(blockResult2.Text);
            }
            else
            {
                secondValue = Convert.ToInt32(blockResult2.Text);
            }

            // Show equals button
            showEquals();
            putValues();
        }

        private async void gameBtn_Click(object sender, RoutedEventArgs e)
        {
            // Disable this button after clicking
            gameBtn.IsEnabled = false;

            // Calculate players and opponents score
            if (finalResult == 24 && occupiedCard1 == 2 && occupiedCard2 == 2 && occupiedCard3 == 2 && occupiedCard4 == 2 && occupiedResult1 == 2 && occupiedResult2 == 2)
            {
                playerScore += valueCard1 + valueCard2 + valueCard3 + valueCard4;
            }
            else
            {
                opponentScore += valueCard1 + valueCard2 + valueCard3 + valueCard4;
            }

            storeScore();

            // Checking, who's won
            if (playerScore > opponentScore)
            {
                MessageDialog end = new MessageDialog("You won!" + Environment.NewLine + "• Your score: " + playerScore + Environment.NewLine + "• Opponents score: " + opponentScore, "Congratulations!");
                await end.ShowAsync();
            } else
            {
                MessageDialog end = new MessageDialog("You lost..." + Environment.NewLine + "• Your score: " + playerScore + Environment.NewLine + "• Opponents score: " + opponentScore, "Good luck next time...");
                await end.ShowAsync();
            }
        }
    }
}
