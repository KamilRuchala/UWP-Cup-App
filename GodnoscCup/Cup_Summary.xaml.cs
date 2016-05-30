using GodnoscCup.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Cup_Summary : Page
    {
        CustomContext context;

        public Cup_Summary()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested +=
                App_BackRequested;
            context = new CustomContext();
            fillSummaryLayout();
        }

        private void App_BackRequested(object sender,
             Windows.UI.Core.BackRequestedEventArgs e)
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


        private void fillSummaryLayout()
        {
            string teamOne = "Manchester United";
            string teamTwo = "FC Barcelona";

            firstTeamPoints.Text = getTeamPoints(teamOne) + " pkt.";
            secondTeamPoints.Text = getTeamPoints(teamTwo) + " pkt.";

            bilansJeden.Text = getTeamRecord(teamOne);
            bilansDwa.Text = getTeamRecord(teamTwo);

            bilansBramekJeden.Text = getTeamGoals(teamOne);
            bilansBramekDwa.Text = getTeamGoals(teamTwo);

            bestWinOne.Text = getGreatestWin(teamOne);
            bestWinTwo.Text = getGreatestWin(teamTwo);

            formaTxtOne.Text = getTeamCurrentStreak(teamOne);
            formaTxtTwo.Text = getTeamCurrentStreak(teamTwo);

            bestScorerOne.Text = getBestScorer(teamOne);
            bestScorerTwo.Text = getBestScorer(teamTwo); 
        }

        private string getTeamRecord(string teamName)
        {
            int teamId = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).First();

            int winCounter = 0;
            int drawCounter = 0;
            int lossCounter = 0;

            var gamesCollection = context.Games.Where(x => x.TeamOneId == teamId);

            if (gamesCollection.Count() > 0)
            {
                winCounter += gamesCollection.Count(x => x.TeamOnePoints == 3);
                drawCounter += gamesCollection.Count(x => x.TeamOnePoints == 1);
                lossCounter += gamesCollection.Count(x => x.TeamOnePoints == 0);
            }

            gamesCollection = context.Games.Where(x => x.TeamTwoId == teamId);

            if (gamesCollection.Count() > 0)
            {
                winCounter += gamesCollection.Count(x => x.TeamTwoPoints == 3);
                drawCounter += gamesCollection.Count(x => x.TeamTwoPoints == 1);
                lossCounter += gamesCollection.Count(x => x.TeamTwoPoints == 0);
            }

            return Convert.ToString(winCounter) + "-" + Convert.ToString(drawCounter) + "-" + Convert.ToString(lossCounter);
        }

        private string getTeamPoints(string teamName)
        {
            int teamId = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).First();

            int teamPoints = 0;

            var gamesCollection = context.Games.Where(x => x.TeamOneId == teamId);

            if(gamesCollection.Count() > 0)
            {
                teamPoints += gamesCollection.Sum(x => x.TeamOnePoints);
            }

            gamesCollection = context.Games.Where(x => x.TeamTwoId == teamId);

            if (gamesCollection.Count() > 0)
            {
                teamPoints += gamesCollection.Sum(x => x.TeamTwoPoints);
            }

            return Convert.ToString(teamPoints);
        }

        private string getTeamGoals(string teamName)
        {
            int teamId = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).First();

            int teamGoalsPlus = 0;
            int teamGoalsMinus = 0;

            var gamesCollection = context.Games.Where(x => x.TeamOneId == teamId);

            if (gamesCollection.Count() > 0)
            {
                teamGoalsPlus = gamesCollection.Sum(x => x.TeamOneScore);
                teamGoalsMinus = gamesCollection.Sum(x => x.TeamTwoScore);
            }

            gamesCollection = context.Games.Where(x => x.TeamTwoId == teamId);

            if (gamesCollection.Count() > 0)
            {
                teamGoalsPlus = gamesCollection.Sum(x => x.TeamTwoScore);
                teamGoalsMinus = gamesCollection.Sum(x => x.TeamOneScore);
            }

            return Convert.ToString(teamGoalsPlus) + "-" + Convert.ToString(teamGoalsMinus);
        }

        private string getGreatestWin(string teamName)
        {
            string result = "Brak";

            bool winFlag = false;

            int teamId = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).First();

            int teamGoalsPlus = 0;
            int teamGoalsMinus = 0;

            var gamesCollection = context.Games.Where(x => x.TeamOneId == teamId && x.TeamOnePoints == 3)
                .OrderByDescending(x => x.TeamOneScore);

            Game bestGame = new Game();

            if (gamesCollection.Count() > 0)
            {
                winFlag = true;
                bestGame = gamesCollection.First();
                teamGoalsPlus = bestGame.TeamOneScore;
                teamGoalsMinus = bestGame.TeamTwoScore;
            }

            gamesCollection = context.Games.Where(x => x.TeamTwoId == teamId && x.TeamTwoPoints == 3)
                .OrderByDescending(x => x.TeamTwoScore); ;

            if (gamesCollection.Count() > 0)
            {
                winFlag = true;
                if (teamGoalsPlus < gamesCollection.Max(x => x.TeamTwoScore))
                {
                    bestGame = gamesCollection.First();
                    teamGoalsPlus = bestGame.TeamTwoScore;
                    teamGoalsMinus = bestGame.TeamOneScore;
                }
            }

            if (winFlag) result = Convert.ToString(teamGoalsPlus) + "-" + Convert.ToString(teamGoalsMinus);

            return result;
        }

        private string getTeamCurrentStreak(string teamName)
        {
            int teamId = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).First();

            string result = "-----";
            StringBuilder teamStreak = new StringBuilder();

            var gamesCollection = context.Games.Where(x => x.TeamOneId == teamId || x.TeamTwoId == teamId).OrderByDescending(x => x.GameDate);

            if(gamesCollection.Count() > 0)
            {
                if(gamesCollection.Count() > 5) gamesCollection = gamesCollection.Take(5).OrderByDescending(x => x.GameDate);

                foreach(var game in gamesCollection)
                {
                    if(game.TeamOneId == teamId)
                    {
                        if (game.TeamOnePoints == 3) teamStreak.Append("W");

                        else if (game.TeamOnePoints == 1) teamStreak.Append("R");

                        else teamStreak.Append("P");
                    }
                    else
                    {
                        if (game.TeamTwoPoints == 3) teamStreak.Append("W");

                        else if (game.TeamTwoPoints == 1) teamStreak.Append("R");

                        else teamStreak.Append("P");
                    }
                }
            }

            if (teamStreak.Length > 0) result = teamStreak.ToString();

            return result;
        }

        private string getBestScorer(string teamName)
        {
            int teamId = context.Teams.Where(x => x.TeamName.Equals(teamName)).Select(x => x.TeamId).First();

            var bestPlayer = context.Players.Where(x => x.TeamId == teamId).OrderByDescending(x => x.ScoredGoals).First();

            StringBuilder result = new StringBuilder();

            result.Append(bestPlayer.PlayerName);
            result.Append("(");
            result.Append(Convert.ToString(bestPlayer.ScoredGoals));
            result.Append(")");

            return result.ToString();
        }
    }
}
