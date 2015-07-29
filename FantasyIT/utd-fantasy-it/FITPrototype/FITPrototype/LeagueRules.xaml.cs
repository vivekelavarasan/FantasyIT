using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for LeagueRules.xaml
    /// </summary>
    ///  

    public partial class LeagueRules : Page
    {
        public LeagueRules()
        {
            LeagueDataParser ldp = new LeagueDataParser("leaguerules.xml");
            League game = ldp.StoredRules;
            InitializeComponent();

            String teamR = "error reading rules";
            String seasonR = "seriously, there's a problem";

            String mm;
            if (game.MutualMatching)
                mm = "Yes";
            else mm = "No";

            //team size, extras, x teams player is on, mutual match
            teamR = "Team Size: " + game.MembersPerTeam +
                    "\nExtras on team: " + game.ExtrasPerTeam +
                    "\nMutual Matching: " + mm +
                    "\nPlayer Recurrence: " + game.SimultaneousTeams;

            String gd;
            if (game.GameNight == 0)
                gd = "Thursday";
            else if (game.GameNight == 1)
                gd = "Sunday";
            else gd = "Monday";

            String tsb;
            if (game.TeamSuperbowl)
                tsb = "Yes";
            else tsb = "No";

            //game night, playoff size, bowl project, season duration, break duration
            //Season End needs work, -does start date mean first game that night or not?
            seasonR = "Game Night: " + gd +
                        "\nDraft Day: " + game.DraftTime.Date.ToString("d") +
                        "\nSeason Start: " + game.SeasonStart.Date.ToString("d") +
                        "\nSeason End: " + game.SeasonStart.AddDays((game.SeasonWeeks-1) * 7).Date.ToString("d") +
                        "\nTeams in Playoffs: " + game.PlayoffTeams +
                        "\nPlayoff Start: " + game.PlayoffStart.Date.ToString("d") +
                        "\nSuperbowl Project: " + tsb;

            teamRules.Text = teamR;
            seasonRules.Text = seasonR;

            //IMPLEMENT CALENDAR STUFF
            //draft day, season play, game days, playoffs, playoff break
     
            calendar.SelectedDates.Add(game.SeasonStart);
            for(int i = 1; i < game.SeasonWeeks; i++)
            {
                calendar.SelectedDates.Add(game.SeasonStart.AddDays(i * 7));
            }
            for(int i = 0; i < game.PlayoffWeeks; i++)
            {
                calendar.SelectedDates.Add(game.PlayoffStart.AddDays(i * 7));
            }
        }
    }
}
