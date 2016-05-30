using GodnoscCup.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GodnoscCup.ViewModels
{
    public class ReportPlayer : INotifyPropertyChanged
    {
        public string playerName;
        public int scoredGoals;

        public ReportPlayer(string pname, int scgoals = 0)
        {
            this.playerName = pname;
            this.scoredGoals = scgoals;
        }

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
            set
            {
                this.playerName = value;
            }
        }

        public int ScoredGoals
        {
            get
            {
                return this.scoredGoals;
            }
            set
            {
                this.scoredGoals = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ReportViewModel
    {
        private ObservableCollection<ReportPlayer> manutd = new ObservableCollection<ReportPlayer>();
        private ObservableCollection<ReportPlayer> barca = new ObservableCollection<ReportPlayer>();

        public ObservableCollection<ReportPlayer> ManUtd { get { return this.manutd; } }
        public ObservableCollection<ReportPlayer> Barcelona { get { return this.barca; } }

        public void ConvertFromDbModelCollection(List<Player> playersModel)
        {
            CustomContext db = new CustomContext();
            var manutdId = db.Teams.Where(x => x.TeamName.Equals("Manchester United")).Select(x => x.TeamId).FirstOrDefault();
            var barcaId = db.Teams.Where(x => x.TeamName.Equals("FC Barcelona")).Select(x => x.TeamId).FirstOrDefault();
            foreach (var player in playersModel.Where(x => x.TeamId == manutdId).OrderBy(x => x.SequenceNumber))
            {
                this.manutd.Add(new ReportPlayer(player.PlayerName));
            }

            foreach (var player in playersModel.Where(x => x.TeamId == barcaId).OrderBy(x => x.SequenceNumber))
            {
                this.barca.Add(new ReportPlayer(player.PlayerName));
            }
        }
    }
}
