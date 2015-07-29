using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FITPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Leaderboard : Page
    {
        League game;
        List<TeamsData> displayedTeams;
        List<PlayersData> displayedPlayers;
        int weeks = 0;
        LeagueDataParser ldp;
        List<List<KeyValuePair<string, int>>> dSL = new List<List<KeyValuePair<string, int>>>();
        List<KeyValuePair<string, int>> vL1 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL2 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL3 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL4 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL5 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL6 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL7 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL8 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL9 = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, int>> vL10 = new List<KeyValuePair<string, int>>();

        public class TeamsData : IComparable
        {
            public int teamRank { set; get; }
            public string teamName { set; get; }
            public int teamScore { set; get; }
			public string teamProgress { set; get; }
            public Team hostTeam { set; get; }

            public int weeks = 0;

            public int CompareTo(Object o)
            {
                if (o == null)
                    return 1;
                return this.CompareTo(o as TeamsData);
            }

            public int CompareTo(TeamsData t)
            {
                if (t == null)
                    return 1;
                return -this.hostTeam.GetTotalForWeek(weeks).CompareTo(t.hostTeam.GetTotalForWeek(weeks));
            }

        }
		
		public class PlayersData : IComparable
		{
			public int playerRank {set;get;}
			public string playerName {set;get;}
			public int playerScore {set;get;}
			public string playerProgress{ set; get; }
            public Employee hostEmp { set; get; }

            public int weeks = 0;

            public int CompareTo(Object o)
            {
                if (o == null)
                    return 1;
                return this.CompareTo(o as PlayersData);
            }

            public int CompareTo(PlayersData t)
            {
                if (t == null)
                    return 1;
                return -this.hostEmp.GetTotalForWeek(weeks).CompareTo(t.hostEmp.GetTotalForWeek(weeks));
            }
		}

        //testing for line chart, giving me trouble
        private void updateLineData()
        {
            lineChart.DataContext = null;
            dSL.Add(vL1);
            dSL.Add(vL2);
            dSL.Add(vL3);
            dSL.Add(vL4);
            dSL.Add(vL5);
            dSL.Add(vL6);
            dSL.Add(vL7);
            dSL.Add(vL8);
            dSL.Add(vL9);
            dSL.Add(vL10);
            lineChart.DataContext = dSL;
        }

        private void showLineChart()
        {
            updateLineData();
        }

        private void updateBarGraph()
        {
            int max = 0;
            foreach (TeamsData t in displayedTeams)
            {
                if (t.teamScore > max)
                    max = t.teamScore;
            }
            b1.Maximum = max;
            b2.Maximum = max;
            b3.Maximum = max;
            b4.Maximum = max;
            b5.Maximum = max;
            b6.Maximum = max;
            b7.Maximum = max;
            b8.Maximum = max;
            b9.Maximum = max;
            b10.Maximum = max;

            b1.Value = displayedTeams[0].teamScore;
            s1.Text = "" + displayedTeams[0].teamScore;
            b2.Value = displayedTeams[1].teamScore;
            s2.Text = "" + displayedTeams[1].teamScore;
            b3.Value = displayedTeams[2].teamScore;
            s3.Text = "" + displayedTeams[2].teamScore;
            b4.Value = displayedTeams[3].teamScore;
            s4.Text = "" + displayedTeams[3].teamScore;
            b5.Value = displayedTeams[4].teamScore;
            s5.Text = "" + displayedTeams[4].teamScore;
            b6.Value = displayedTeams[5].teamScore;
            s6.Text = "" + displayedTeams[5].teamScore;
            b7.Value = displayedTeams[6].teamScore;
            s7.Text = "" + displayedTeams[6].teamScore;
            b8.Value = displayedTeams[7].teamScore;
            s8.Text = "" + displayedTeams[7].teamScore;
            b9.Value = displayedTeams[8].teamScore;
            s9.Text = "" + displayedTeams[8].teamScore;
            b10.Value = displayedTeams[9].teamScore;
            s10.Text = "" + displayedTeams[9].teamScore;
        }

        public Leaderboard()
        {
            
			TeamDataParser teamParser = new TeamDataParser();
			List<Team> teamList = teamParser.GetTeams();
			List<Employee> employeeList = new List<Employee>();
            ldp = new LeagueDataParser("leaguerules.xml");
            displayedTeams = new List<TeamsData>();
            displayedPlayers = new List<PlayersData>();
            game = ldp.StoredRules;
            Trace.WriteLine("Playoff weeks: " + game.PlayoffWeeks + " Season weeks: " + game.SeasonWeeks);
			foreach(Team t in teamList)
			{
				employeeList.AddRange(t.Members);
			}
			
			employeeList.Sort();

            

            InitializeComponent();
            DataGridTextColumn tCol1 = new DataGridTextColumn();
            DataGridTextColumn tCol2 = new DataGridTextColumn();
            DataGridTextColumn tCol3 = new DataGridTextColumn();
			DataGridTextColumn tCol4 = new DataGridTextColumn();
            teamsDataGrid.Columns.Add(tCol1);
            teamsDataGrid.Columns.Add(tCol2);
            teamsDataGrid.Columns.Add(tCol3);
			teamsDataGrid.Columns.Add(tCol4);
            tCol1.Binding = new Binding("teamRank");
            tCol2.Binding = new Binding("teamName");
            tCol3.Binding = new Binding("teamScore");
			tCol4.Binding = new Binding("teamProgress");
            tCol1.Header = "Rank";
            tCol2.Header = "Team Name";
            tCol3.Header = "Score";
			tCol4.Header = "Progress";
			
			DataGridTextColumn pCol1 = new DataGridTextColumn();
			DataGridTextColumn pCol2 = new DataGridTextColumn();
			DataGridTextColumn pCol3 = new DataGridTextColumn();
			DataGridTextColumn pCol4 = new DataGridTextColumn();
			playersDataGrid.Columns.Add(pCol1);
			playersDataGrid.Columns.Add(pCol2);
			playersDataGrid.Columns.Add(pCol3);
			playersDataGrid.Columns.Add(pCol4);
			pCol1.Binding = new Binding("playerRank");
			pCol2.Binding = new Binding("playerName");
			pCol3.Binding = new Binding("playerScore");
			pCol4.Binding = new Binding("playerProgress");
			pCol1.Header = "Rank";
			pCol2.Header = "Player Name";
			pCol3.Header = "Score";
			pCol4.Header = "Progress";
			
			for(int j = 0; j<teamList.Count; j++)
			{
                if (teamList[j].TeamName != "Free Agents")
                {
                    teamList[j].SubCheck(weeks);
                    teamsDataGrid.Items.Add(
                        new TeamsData
                        {
                            teamRank = j,
                            teamName = teamList[j].TeamName,
                            teamScore = teamList[j].GetTotalForWeek(weeks),
                            teamProgress = "+" + teamList[j].GetPointsGainedForWeek(weeks),
                            hostTeam = teamList[j]
                        });
                    displayedTeams.Add(
                        new TeamsData
                        {
                            teamRank = j + 1,
                            teamName = teamList[j].TeamName,
                            teamScore = teamList[j].GetTotalForWeek(weeks),
                            teamProgress = "+" + teamList[j].GetPointsGainedForWeek(weeks),
                            hostTeam = teamList[j]
                        });
                }
			}
            teamsDataGrid.Items.Remove(teamsDataGrid.Items[0]);

			for(int k = 0; k<employeeList.Count; k++)
			{
				playersDataGrid.Items.Add(
                    new PlayersData{
                        playerRank = (k+1), 
                        playerName = employeeList[k].GetName(), 
                        playerScore = employeeList[k].GetPoints(), 
                        playerProgress = "+"+employeeList[k].GetScoreOfWeek(weeks),
                        hostEmp = employeeList[k]});
                displayedPlayers.Add(
                        new PlayersData
                        {
                            playerRank = (k + 1),
                            playerName = employeeList[k].GetName(),
                            playerScore = employeeList[k].GetPoints(),
                            playerProgress = "+" + employeeList[k].GetScoreOfWeek(weeks),
                            hostEmp = employeeList[k]
                        });
			}
            playersDataGrid.Items.Remove(playersDataGrid.Items[0]);

            vL1.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL2.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL3.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL4.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL5.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL6.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL7.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL8.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL9.Add(new KeyValuePair<string, int>("Week 1", 0));
            vL10.Add(new KeyValuePair<string, int>("Week 1", 0));
        
        }

        private void NextWeekClick(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Weeks: " + weeks + " Total: " + game.TotalWeeks);
            if (weeks + 1 < game.TotalWeeks)
            {
                weeks += 1;
                Trace.WriteLine("Displayed Teams size: " + displayedTeams.Count + " Displayed Players size: " + displayedPlayers.Count);
                Trace.WriteLine("Teams items size: " + teamsDataGrid.Items.Count + " Displayed Players size: " + playersDataGrid.Items.Count);
                for (int i = 0; i < teamsDataGrid.Items.Count; i++) // Clear substitutes from last week
                {
                    TeamsData t = displayedTeams[i];
                    t.hostTeam.ClearSubs();
                    displayedTeams[i] = t;
                }

                for (int i = 0; i < teamsDataGrid.Items.Count; i++ )
                {
                    TeamsData t = displayedTeams[i];
                    t.hostTeam.SubCheck(weeks); // check if we need to sub in free agents
                    t.teamScore = t.hostTeam.GetTotalForWeek(weeks);
                    t.teamProgress = "+"+t.hostTeam.GetPointsGainedForWeek(weeks);
                    t.weeks = weeks; // Tracks weeks for compareto
                    teamsDataGrid.Items[i] = t;
                    displayedTeams[i] = t;
                }
                
                for (int i = 0; i < playersDataGrid.Items.Count; i++)
                {
                    PlayersData t = displayedPlayers[i];
                    t.playerScore = t.hostEmp.GetTotalForWeek(weeks);
                    t.playerProgress = "+"+t.hostEmp.GetScoreOfWeek(weeks);
                    t.weeks = weeks; // Tracks weeks for compareto
                    playersDataGrid.Items[i] = t;
                    displayedPlayers[i] = t;
                }

                // Sort teams and players based on score rankings
                displayedTeams.Sort();
                teamsDataGrid.Items.Clear();
                for (int i = 0; i < displayedTeams.Count; i++)
                {
                    displayedTeams[i].teamRank = i + 1;
                    teamsDataGrid.Items.Add(displayedTeams[i]);
                }

                displayedPlayers.Sort();
                playersDataGrid.Items.Clear();
                for (int i = 0; i < displayedPlayers.Count; i++)
                {
                    displayedPlayers[i].playerRank = i + 1;
                    playersDataGrid.Items.Add(displayedPlayers[i]);
                }

                playersDataGrid.Items.Refresh();
                teamsDataGrid.Items.Refresh();

                vL1.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[0].teamScore));
                vL2.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[1].teamScore));
                vL3.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[2].teamScore));
                vL4.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[3].teamScore));
                vL5.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[4].teamScore));
                vL6.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[5].teamScore));
                vL7.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[6].teamScore));
                vL8.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[7].teamScore));
                vL9.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[8].teamScore));
                vL10.Add(new KeyValuePair<string, int>("Week" + (weeks + 1), displayedTeams[9].teamScore));
                updateLineData();
                updateBarGraph();
                Trace.WriteLine("Next week...");
            }
        }
        private void PrevWeekClick(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Weeks: " + weeks);
            if (weeks - 1 >= 0)
            {
                weeks -= 1;
                for (int i = 0; i < teamsDataGrid.Items.Count; i++) // Clear substitutes from last week
                {
                    TeamsData t = displayedTeams[i];
                    t.hostTeam.ClearSubs();
                    displayedTeams[i] = t;
                }

                for (int i = 0; i < teamsDataGrid.Items.Count; i++)
                {
                    TeamsData t = displayedTeams[i];
                    t.hostTeam.SubCheck(weeks); // check if we need to sub in free agents
                    t.teamScore = t.hostTeam.GetTotalForWeek(weeks);
                    t.teamProgress = "+" + t.hostTeam.GetPointsGainedForWeek(weeks);
                    t.weeks = weeks; // Tracks weeks for compareto
                    teamsDataGrid.Items[i] = t;
                    displayedTeams[i] = t;
                }

                for (int i = 0; i < playersDataGrid.Items.Count; i++)
                {
                    PlayersData t = displayedPlayers[i];
                    t.playerScore = t.hostEmp.GetTotalForWeek(weeks);
                    t.playerProgress = "+" + t.hostEmp.GetScoreOfWeek(weeks);
                    t.weeks = weeks; // Tracks weeks for compareto
                    playersDataGrid.Items[i] = t;
                    displayedPlayers[i] = t;
                }

                // Sort teams and players based on score rankings
                displayedTeams.Sort();
                teamsDataGrid.Items.Clear();
                for (int i = 0; i < displayedTeams.Count; i++)
                {
                    displayedTeams[i].teamRank = i + 1;
                    teamsDataGrid.Items.Add(displayedTeams[i]);
                }

                displayedPlayers.Sort();
                playersDataGrid.Items.Clear();
                for (int i = 0; i < displayedPlayers.Count; i++)
                {
                    displayedPlayers[i].playerRank = i + 1;
                    playersDataGrid.Items.Add(displayedPlayers[i]);
                    
                }
                
                playersDataGrid.Items.Refresh();
                teamsDataGrid.Items.Refresh();

                vL1.RemoveAt(weeks+1);
                vL2.RemoveAt(weeks+1);
                vL3.RemoveAt(weeks+1);
                vL4.RemoveAt(weeks+1);
                vL5.RemoveAt(weeks+1);
                vL6.RemoveAt(weeks+1);
                vL7.RemoveAt(weeks+1);
                vL8.RemoveAt(weeks+1);
                vL9.RemoveAt(weeks+1);
                vL10.RemoveAt(weeks+1);
                updateLineData();
                updateBarGraph();
                Trace.WriteLine("Previous week...");
            }
        }

        private void TournamentBracketClick(object sender,RoutedEventArgs e)
        {
            if (DateTime.Today >= ldp.StoredRules.PlayoffStart)
            {
                TournamentBracket tb = new TournamentBracket();
                this.NavigationService.Navigate(tb);
            }
        }

        private void LineGraph_Click(object sender, RoutedEventArgs e)
        {
            lineChart.Visibility = Visibility.Visible;
            lineZoom.Visibility = Visibility.Visible;
            showLineChart();
            lineZoom.Minimum = 0.0;
            lineZoom.Maximum = 0.8;
            barCol1.Visibility = Visibility.Collapsed;
            barCol2.Visibility = Visibility.Collapsed;
            barCol3.Visibility = Visibility.Collapsed;
        }

        private void BarGraph_Click(object sender, RoutedEventArgs e)
        {
            lineChart.Visibility = Visibility.Collapsed;
            lineZoom.Visibility = Visibility.Collapsed;
            barCol1.Visibility = Visibility.Visible;
            barCol2.Visibility = Visibility.Visible;
            barCol3.Visibility = Visibility.Visible;
        }

        private void lineZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double z = lineZoom.Value;
            double maxTempY = (double)YAxis.ActualMaximum;
            YAxis.Minimum = maxTempY * z;
        }

        private void playersDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e) 
        {
            
        }
    }
}
