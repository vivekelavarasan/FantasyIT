using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FITPrototype
{
    /// <summary>
    /// Interaction logic for TournamentBracket.xaml
    /// </summary>
    
    public partial class TournamentBracket : Page
    {
        private int NumTeams,WeekNum,DaysSincePlayoffStart;
        private TeamDataParser teamParser;
        private LeagueDataParser leagueParser;
        private PlayoffTeamParser playoffParser;
        private List<Team> teamList,playoffTeamList;
        private List<Employee> playoffPlayers;
        private DateTime CurrentDate;
        private TimeSpan TimeSincePlayoffStart;

        public TournamentBracket()
        {
            teamParser = new TeamDataParser();
            teamList = teamParser.GetTeams();

            leagueParser = new LeagueDataParser("leaguerules.xml");
            NumTeams = leagueParser.StoredRules.PlayoffTeams;

            CurrentDate = DateTime.Today;
            WeekNum = leagueParser.GetWeekNumber(CurrentDate);

            DetermineTeams();   //Determines which teams made playoffs, takes top 4/8/16 teams at the start of the playoffs and puts them in playoffTeamList
            
            InitializeComponent();

//Fill in initial playoff standings (week 1)
            DataGridTextColumn round1 = new DataGridTextColumn();
            round1.Header = "Round 1";
            bracketGrid1.Columns.Add(round1);

            TimeSincePlayoffStart = CurrentDate - leagueParser.StoredRules.PlayoffStart;
            DaysSincePlayoffStart = TimeSincePlayoffStart.Days;

            //int row = 0;
            for (int t = 0; t < playoffTeamList.Count; t++ )
            {
                bracketGrid1.Items.Add(playoffTeamList[t].TeamName);
                
                if (t % 2 == 0) //every even indexed team should be versus someone and every odd indexed team should have a blank spot between it and the next matchup
                    bracketGrid1.Items.Add("vs");
                else bracketGrid1.Items.Add("");
            }

//Fill in playoff standings after 1 week (week2)
            if(DaysSincePlayoffStart >= 7 && DaysSincePlayoffStart < 14)
            {
                DataGridTextColumn round2 = new DataGridTextColumn();
                round2.Header = "Round 2";
                bracketGrid2.Columns.Add(round2);

                //List<Team> tempList = playoffTeamList;
                for(int t = 0;t<playoffTeamList.Count;t+=2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        //tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid2.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");

                    }
                    else if(playoffTeamList[t].GetTotalForWeek(WeekNum) < playoffTeamList[t+1].GetTotalForWeek(WeekNum))
                    {
                        //tempList.Remove(playoffTeamList[t]);

                        bracketGrid2.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");
                    }
                }
            }

//Fill in playoff standings after 2 weeks (up to week 3)
            else if(DaysSincePlayoffStart >= 14 && DaysSincePlayoffStart < 21)
            {
                DataGridTextColumn round2 = new DataGridTextColumn();
                round2.Header = "Round 2";
                bracketGrid2.Columns.Add(round2);
                
                List<Team> tempList = playoffTeamList;

                for(int t = 0;t<playoffTeamList.Count;t+=2)
                {                    
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum-1) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum-1)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid2.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");

                    }
                    else if(playoffTeamList[t].GetTotalForWeek(WeekNum-1) < playoffTeamList[t+1].GetTotalForWeek(WeekNum-1))
                    {
                        tempList.Remove(playoffTeamList[t]);

                        bracketGrid2.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");
                    }
                    
                }
                playoffTeamList = tempList;

                DataGridTextColumn round3 = new DataGridTextColumn();
                round3.Header = "Round 3";
                bracketGrid3.Columns.Add(round3);

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        //tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid3.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid3.Items.Add("vs");
                        else bracketGrid3.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum))
                    {
                        //tempList.Remove(playoffTeamList[t]);

                        bracketGrid3.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid3.Items.Add("vs");
                        else bracketGrid3.Items.Add("");
                    }

                }
            }
//Fill in playoff standings after 3 weeks (up to week 4) (should only display if there are 8 or more teams)
            else if (DaysSincePlayoffStart >= 21 && DaysSincePlayoffStart < 28 && NumTeams>=8)
            {
                DataGridTextColumn round2 = new DataGridTextColumn();
                round2.Header = "Round 2";
                bracketGrid2.Columns.Add(round2);

                List<Team> tempList = playoffTeamList;

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum - 2) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 2)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid2.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum - 2) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 2))
                    {
                        tempList.Remove(playoffTeamList[t]);

                        bracketGrid2.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");
                    }

                }
                playoffTeamList = tempList;

                DataGridTextColumn round3 = new DataGridTextColumn();
                round3.Header = "Round 3";
                bracketGrid3.Columns.Add(round3);

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum - 1) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 1)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid3.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid3.Items.Add("vs");
                        else bracketGrid3.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum - 1) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 1))
                    {
                        tempList.Remove(playoffTeamList[t]);

                        bracketGrid3.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid3.Items.Add("vs");
                        else bracketGrid3.Items.Add("");
                    }

                }
                playoffTeamList = tempList;

                DataGridTextColumn round4 = new DataGridTextColumn();
                round4.Header = "Round 4";
                bracketGrid4.Columns.Add(round4);

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        //tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid4.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid4.Items.Add("vs");
                        else bracketGrid4.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum))
                    {
                        //tempList.Remove(playoffTeamList[t]);

                        bracketGrid4.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid4.Items.Add("vs");
                        else bracketGrid4.Items.Add("");
                    }

                }
            }
//Fill in playoff standings after 4 weeks (week 5) (should only display if there are 16 or more teams)
//(also last possible display since the max number of playoff weeks is 16 currently)
            else if (DaysSincePlayoffStart >= 28  && NumTeams>=16)
            {
                DataGridTextColumn round2 = new DataGridTextColumn();
                round2.Header = "Round 2";
                bracketGrid2.Columns.Add(round2);

                List<Team> tempList = playoffTeamList;

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum - 3) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 3)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid2.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum - 3) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 3))
                    {
                        tempList.Remove(playoffTeamList[t]);

                        bracketGrid2.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid2.Items.Add("vs");
                        else bracketGrid2.Items.Add("");
                    }

                }
                playoffTeamList = tempList;

                DataGridTextColumn round3 = new DataGridTextColumn();
                round3.Header = "Round 3";
                bracketGrid3.Columns.Add(round3);

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum - 2) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 2)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid3.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid3.Items.Add("vs");
                        else bracketGrid3.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum - 2) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 2))
                    {
                        tempList.Remove(playoffTeamList[t]);

                        bracketGrid3.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid3.Items.Add("vs");
                        else bracketGrid3.Items.Add("");
                    }

                }
                playoffTeamList = tempList;

                DataGridTextColumn round4 = new DataGridTextColumn();
                round4.Header = "Round 4";
                bracketGrid4.Columns.Add(round4);

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum - 1) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 1)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid4.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid4.Items.Add("vs");
                        else bracketGrid4.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum - 1) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum - 1))
                    {
                        tempList.Remove(playoffTeamList[t]);

                        bracketGrid4.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid4.Items.Add("vs");
                        else bracketGrid4.Items.Add("");
                    }

                }
                playoffTeamList = tempList;

                DataGridTextColumn round5 = new DataGridTextColumn();
                round5.Header = "Round 5";
                bracketGrid5.Columns.Add(round5);

                for (int t = 0; t < playoffTeamList.Count; t += 2)
                {
                    if (playoffTeamList[t].GetTotalForWeek(WeekNum) >= playoffTeamList[t + 1].GetTotalForWeek(WeekNum)) //Right now the team that was already ahead goes on in the event of a tie
                    {
                        //tempList.Remove(playoffTeamList[t + 1]);

                        bracketGrid4.Items.Add(playoffTeamList[t].TeamName);
                        if (t % 4 == 0)
                            bracketGrid4.Items.Add("vs");
                        else bracketGrid4.Items.Add("");

                    }
                    else if (playoffTeamList[t].GetTotalForWeek(WeekNum) < playoffTeamList[t + 1].GetTotalForWeek(WeekNum))
                    {
                        //tempList.Remove(playoffTeamList[t]);

                        bracketGrid4.Items.Add(playoffTeamList[t + 1].TeamName);
                        if (t % 4 == 0)
                            bracketGrid4.Items.Add("vs");
                        else bracketGrid4.Items.Add("");
                    }

                }
            }
            
        }

        public void DetermineTeams()
        {
            if(!(CurrentDate < leagueParser.StoredRules.PlayoffStart))//make sure we're actually in playoffs
            {
                if (CurrentDate == leagueParser.StoredRules.PlayoffStart)//If we're starting playoffs then we need to determine who's playing
                {
                    playoffTeamList = teamList.GetRange(0, NumTeams);   //gets the top 4/8/16 teams based on playoff bracket size

                    if (!(File.Exists("playoffplayers.xml")))
                    {
                        List<string> teamNames = new List<string>();
                        foreach (Team t in playoffTeamList)
                        {
                            teamNames.Add(t.TeamName);
                        }

                        playoffParser = new PlayoffTeamParser(teamNames);
                    }
                }
                else //we're already in the playoffs and should always load the teams based on who is in the playoffplaers.xml file
                {
                    playoffParser = new PlayoffTeamParser();
                    List<string> teamNames = playoffParser.GetPlayoffTeamNames();

                    playoffTeamList = new List<Team>();
                    foreach (string n in teamNames)
                    {
                        foreach(Team t in teamList)
                        {
                            if (n == t.TeamName)
                                playoffTeamList.Add(t);
                        }
                    }
                 }
            }
        }
    }
}
