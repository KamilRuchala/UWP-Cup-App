using GodnoscCup.Models;
using GodnoscCup.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GodnoscCup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MatchReport : Page
    {
        CustomContext context;
        private ReportViewModel repViewModel;

        public ReportViewModel RepViewModel
        {
            get
            {
                return this.repViewModel;
            }
            set
            {
                if (value != repViewModel)
                {
                    repViewModel = value;
                }
            }
        }

        public MatchReport()
        {
            this.InitializeComponent();
            warningTxt.Visibility = Visibility.Collapsed;

            context = new CustomContext();
            RepViewModel = new ReportViewModel();
            
            if (!context.Games.Any())
            {
                this.game_number_text.Text = "Mecz nr. 1";
            }
            else
            {
                this.game_number_text.Text = "Mecz nr. " + Convert.ToString(context.Games.Count() + 1);
            }

            FillPlayersSection();

            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void FillPlayersSection()
        {
            RepViewModel.ConvertFromDbModelCollection(context.Players.ToList());
            FirstTeamPlayers.ItemsSource = RepViewModel.ManUtd;
            SecondTeamPlayers.ItemsSource = RepViewModel.Barcelona;
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            string scoreOne = teamOneScore.Text;
            string scoreTwo = teamTwoScore.Text;

            bool dataValid = validateScore(scoreOne, scoreTwo);

            if(!dataValid)
            {
                return;
            }

            storeGameReport(scoreOne, scoreTwo);

            this.Frame.Navigate(typeof(MainMenu));
        }

        private bool validateScore(string scoreOne, string scoreTwo)
        {
            
            string pattern = "\\d";

            if(!Regex.IsMatch(scoreOne, pattern) || !Regex.IsMatch(scoreTwo, pattern))
            {
                this.warningTxt.Visibility = Visibility.Visible;
                this.warningTxt.Text = "Podano wynik w nieprawidłowej formie";
                return false;
            }

            int manutdScores = RepViewModel.ManUtd.Sum(x => x.ScoredGoals);
            int barcaScores = RepViewModel.Barcelona.Sum(x => x.ScoredGoals);

            if(Convert.ToInt32(scoreOne) != manutdScores || Convert.ToInt32(scoreTwo) != barcaScores)
            {
                this.warningTxt.Visibility = Visibility.Visible;
                this.warningTxt.Text = "Przypisane bramki zawodnikom nie zgadzają się z wynikiem!";
                return false;
            }

            return true;
        }

        private void storeGameReport(string scoreOne, string scoreTwo)
        {
            int firstTeamId = context.Teams.Where(x => x.TeamName.Equals("Manchester United")).Select(x => x.TeamId).FirstOrDefault();
            int secondTeamId = context.Teams.Where(x => x.TeamName.Equals("FC Barcelona")).Select(x => x.TeamId).FirstOrDefault();

            Game game = new Game();
            game.GameDate = gameDate.Date.DateTime;
            game.TeamOneId = firstTeamId;
            game.TeamOneScore = Convert.ToInt32(scoreOne);
            game.TeamTwoId = secondTeamId;
            game.TeamTwoScore = Convert.ToInt32(scoreTwo);

            if (game.TeamOneScore == game.TeamTwoScore)
            {
                game.TeamOnePoints = 1;
                game.TeamTwoPoints = 1;
            }
            else if (game.TeamOneScore > game.TeamTwoScore)
            {
                game.TeamOnePoints = 3;
                game.TeamTwoPoints = 0;
            }
            else
            {
                game.TeamOnePoints = 0;
                game.TeamTwoPoints = 3;
            }

            context.Games.Add(game);

            context.SaveChanges();

            addScorers(game);
        }

        private void addScorers(Game game)
        {
            foreach(var player in RepViewModel.ManUtd.Where(x => x.ScoredGoals > 0))
            {
                for(int i = 0; i < player.ScoredGoals; i++)
                { 
                    Scorer scorer = new Scorer();
                    scorer.Game = game;
                    scorer.GameId = game.GameId;
                    scorer.PlayerId = context.Players.Where(x => x.PlayerName.Equals(player.PlayerName)).Select(x => x.PlayerId).FirstOrDefault();
                    context.Scorers.Add(scorer);
                    var plaja = context.Players.Where(x => x.PlayerId == scorer.PlayerId).FirstOrDefault();
                    plaja.ScoredGoals++;
                }
            }
            foreach (var player in RepViewModel.Barcelona.Where(x => x.ScoredGoals > 0))
            {
                for (int i = 0; i < player.ScoredGoals; i++)
                {
                    Scorer scorer = new Scorer();
                    scorer.Game = game;
                    scorer.GameId = game.GameId;
                    scorer.PlayerId = context.Players.Where(x => x.PlayerName.Equals(player.PlayerName)).Select(x => x.PlayerId).FirstOrDefault();
                    context.Scorers.Add(scorer);
                    var plaja = context.Players.Where(x => x.PlayerId == scorer.PlayerId).FirstOrDefault();
                    plaja.ScoredGoals++;
                }
            }

            context.SaveChanges();
        }

        private void teamOneScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.warningTxt.Visibility = Visibility.Collapsed;
        }

        private void teamTwoScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.warningTxt.Visibility = Visibility.Collapsed;
        }

        private void buttonplus_Click(object sender, RoutedEventArgs e)
        {
            int score = Convert.ToInt32(this.scoreTxt.Text);
            score++;
            this.scoreTxt.Text = Convert.ToString(score);
        }

        private void buttonminus_Click(object sender, RoutedEventArgs e)
        {
            int score = Convert.ToInt32(this.scoreTxt.Text);
            score--;
            this.scoreTxt.Text = Convert.ToString(score);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbn = sender as ComboBox;

            string teamName = string.Empty;

            if (cbn.SelectedValue != null) teamName = cbn.SelectedValue.ToString();
            else return;

            int teamid = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).FirstOrDefault();

            scorerLabel.Visibility = Visibility.Visible;
            comboBoxPlayes.Visibility = Visibility.Visible;

            comboBoxPlayes.ItemsSource = context.Players.Where(x => x.TeamId == teamid).OrderBy(x => x.ScoredGoals)
                .Select(x => x.PlayerName);
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            warningTxt.Visibility = Visibility.Collapsed;     
            this.comboBox.ItemsSource = context.Teams.Select(x => x.TeamName);
            comboBoxPlayes.Visibility = Visibility.Collapsed;
            scorerLabel.Visibility = Visibility.Collapsed;
            scoreTxt.Visibility = Visibility.Collapsed;
            buttonminus.Visibility = Visibility.Collapsed; 
            buttonplus.Visibility = Visibility.Collapsed;
            this.MyContentDialog.Visibility = Visibility.Visible;
            var result = await MyContentDialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {
                string teamName = comboBox.SelectedValue.ToString();
                string playerName = comboBoxPlayes.SelectedValue.ToString();
                int scoredGoals = Convert.ToInt32(scoreTxt.Text);

                if(teamName.Equals("Manchester United"))
                {
                    var player = RepViewModel.ManUtd.Where(x => x.PlayerName.Equals(playerName)).First();
                    player.ScoredGoals = scoredGoals;
                }
                else
                {
                    var player = RepViewModel.Barcelona.Where(x => x.PlayerName.Equals(playerName)).First();
                    player.ScoredGoals = scoredGoals;
                }
            }
        }

        private void comboBoxPlayes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;                       

            if (cmb.SelectedValue != null)
            {
                scoreTxt.Visibility = Visibility.Visible;
                buttonminus.Visibility = Visibility.Visible;
                buttonplus.Visibility = Visibility.Visible;

                string teamName = comboBox.SelectedValue.ToString();

                ReportPlayer player;

                if (teamName.Equals("Manchester United")) player = RepViewModel
                        .ManUtd.Where(x => x.PlayerName.Equals(cmb.SelectedValue.ToString())).FirstOrDefault();

                else player = RepViewModel.Barcelona.Where(x => x.PlayerName
                                                            .Equals(cmb.SelectedValue.ToString())).FirstOrDefault(); ;

                scoreTxt.Text = Convert.ToString(player.ScoredGoals);
            }
        }
    }
}
