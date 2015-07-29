using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// The first page the user sees.
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
            TeamDataParser td = new TeamDataParser();
            LeagueDataParser ld;
            DateTime CurrentDate = DateTime.Today;
            DateString.Text = "Today's Date: " + CurrentDate.Month + "-" + CurrentDate.Day + "-" + CurrentDate.Year;
            int WeekNum = 0;

            if (File.Exists("leaguerules.xml"))
            {
                leagueButton.Content = "Manage League";

                ld = new LeagueDataParser("leaguerules.xml");
                WeekNum = ld.GetWeekNumber(CurrentDate);
                DateString.Text += "\nWeek " + WeekNum + " of current League";
            }
            else
            {
                leagueButton.Content = "Setup League";
            }

            teamString.Text = td.GetTeams()[0].ToString();
        }
        private void ManageTeamClick(object sender, RoutedEventArgs e)
        {
            ManageTeam manage = new ManageTeam();
            this.NavigationService.Navigate(manage);
        }
        private void CreateTeamClick(object sender, RoutedEventArgs e)
        {
            CreateTeam create = new CreateTeam();
            this.NavigationService.Navigate(create);
        }
        private void LeaderboardClick(object sender, RoutedEventArgs e)
        {
            Leaderboard lb = new Leaderboard();
            this.NavigationService.Navigate(lb);
        }

        private void SetupLeagueClick(object sender, RoutedEventArgs e)
        {
            SetupScreen ss = new SetupScreen();
            this.NavigationService.Navigate(ss);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LeagueRules lr = new LeagueRules();
            this.NavigationService.Navigate(lr);
        }
    }
}
