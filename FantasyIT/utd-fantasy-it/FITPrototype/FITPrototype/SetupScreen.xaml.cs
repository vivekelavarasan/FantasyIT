using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
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
    /// Interaction logic for SetupScreen.xaml
    /// </summary>
    public partial class SetupScreen : Page
    { 
        //League lg;
        LeagueDataParser ldp;
        bool startSelected = false;
        bool endSelected = false;
        bool breakSelected = false;
        public SetupScreen()
        {
            ldp = new LeagueDataParser("leaguerules.xml");
            //lg = ldp.StoredRules;
            InitializeComponent();

            if (ldp.initialized)
                done.Content = "Apply Changes";
            else
                done.Content = "Start League";
				
			//done.Content = "Start League";

           
			cb_teamSize.Items.Add(1);
			cb_teamSize.Items.Add(2);
			cb_teamSize.Items.Add(3);
			cb_teamSize.Items.Add(4);
			cb_teamSize.Items.Add(5);
			cb_teamSize.Items.Add(6);
            cb_teamSize.SelectedValue = ldp.StoredRules.MembersPerTeam;
			cb_teamSize.SelectionChanged +=  TeamSize_SelectionChanged;
			
			cb_teamExtra.Items.Add(0);
			cb_teamExtra.Items.Add(1);
			cb_teamExtra.Items.Add(2);
            cb_teamExtra.SelectedValue = ldp.StoredRules.ExtrasPerTeam;
			cb_teamExtra.SelectionChanged += TeamExtras_SelectionChanged;
			
			cb_gameNight.Items.Add("Sunday");
			cb_gameNight.Items.Add("Monday");
			cb_gameNight.Items.Add("Tuesday");
			cb_gameNight.Items.Add("Wednesday");
			cb_gameNight.Items.Add("Thursday");
			cb_gameNight.Items.Add("Friday");
			cb_gameNight.Items.Add("Saturday");
            byte gnight = ldp.StoredRules.GameNight;
            switch(gnight)
            {
                case 0:
                    cb_gameNight.SelectedValue = "Thursday";
                    break;
                case 1:
                    cb_gameNight.SelectedValue = "Sunday";
                    break;
                case 2:
                    cb_gameNight.SelectedValue = "Monday";
                    break;
                case 3:
                    cb_gameNight.SelectedValue = "Tuesday";
                    break;
                case 4:
                    cb_gameNight.SelectedValue = "Wednesday";
                    break;
                case 5:
                    cb_gameNight.SelectedValue = "Friday";
                    break;
                case 6:
                    cb_gameNight.SelectedValue = "Saturday";
                    break;
            }
			cb_gameNight.SelectionChanged += GameNight_SelectionChanged;
			
			cb_playerExclusive.Items.Add(1);
			cb_playerExclusive.Items.Add(2);
			cb_playerExclusive.Items.Add(3);
            cb_playerExclusive.SelectedValue = (int)ldp.StoredRules.SimultaneousTeams;
			cb_playerExclusive.SelectionChanged += PlayerExclusivity_SelectionChanged;
			
			cb_playoffSize.Items.Add(4);
			cb_playoffSize.Items.Add(8);
			cb_playoffSize.Items.Add(16);
            cb_playoffSize.SelectedValue = (int)ldp.StoredRules.PlayoffTeams;
			cb_playoffSize.SelectionChanged += PlayoffSize_SelectionChanged;
			
			for(int n = 1; n<=52; n++)
			{	
				cb_seasonDuration.Items.Add(n);
			}
            TimeSpan t = ldp.StoredRules.SeasonEnd - ldp.StoredRules.SeasonStart;
            cb_seasonDuration.SelectedValue = (int)(t.TotalDays/7);
			cb_seasonDuration.SelectionChanged += SeasonDuration_SelectionChanged;
			
			cb_playoffBreak.Items.Add(0);
			cb_playoffBreak.Items.Add(1);
			cb_playoffBreak.Items.Add(2);
			cb_playoffBreak.Items.Add(3);
			cb_playoffBreak.Items.Add(4);
			cb_playoffBreak.Items.Add(5);

            t = ldp.StoredRules.SeasonEnd - ldp.StoredRules.PlayoffStart;
            cb_playoffBreak.SelectedValue = (int)(t.TotalDays / 7);
			cb_playoffBreak.SelectionChanged += PlayoffBreak_SelectionChanged;
            breakSelected = true;

            dp_seasonStartDate.SelectedDate = ldp.StoredRules.SeasonStart;
			dp_seasonStartDate.SelectedDateChanged += StartDate_SelectionChanged;
            startSelected = true;

            dp_draftDate.SelectedDate = ldp.StoredRules.DraftTime;
			dp_draftDate.SelectedDateChanged += DraftDay_SelectionChanged;

            rb_mutualMatchYes.IsChecked = ldp.StoredRules.MutualMatching;
            rb_mutualMatchNo.IsChecked = !ldp.StoredRules.MutualMatching;
			rb_mutualMatchYes.Checked += MutualMatching_SelectionChanged;

            rb_superbowlYes.IsChecked = ldp.StoredRules.TeamSuperbowl;
            rb_superbowlNo.IsChecked = !ldp.StoredRules.TeamSuperbowl;
			rb_superbowlYes.Checked += Superbowl_SelectionChanged;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
			ldp.SaveRules(ldp.StoredRules,"leaguerules.xml");
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }
		
		private void TeamSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ldp.StoredRules.MembersPerTeam = (int)cb_teamSize.SelectedValue;
		}
		
		private void TeamExtras_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ldp.StoredRules.ExtrasPerTeam = (int)cb_teamExtra.SelectedValue;
		}
		
		private void GameNight_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			String day = (String)cb_gameNight.SelectedValue;
			byte val = 8;
			switch(day)
			{
				case "Thursday": 
					val = 0;
				break;
				case "Sunday":
					val = 1;
				break;
				case "Monday":
					val = 2;
				break;
				case "Tuesday":
					val = 3;
				break;
				case "Wednesday":
					val = 4;
				break;
				case "Friday":
					val = 5;
				break;
				case "Saturday":
					val = 6;
				break;
			}
		
			ldp.StoredRules.GameNight = val;
		}
		
		private void PlayerExclusivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int temp = (int)cb_playerExclusive.SelectedValue;
			ldp.StoredRules.SimultaneousTeams = (byte)temp;
		}
		
		private void PlayoffSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int temp = (int)cb_playoffSize.SelectedValue;
			ldp.StoredRules.PlayoffTeams = (byte)temp;
            switch(temp)
            {
                case 4: ldp.StoredRules.PlayoffWeeks = 2; break;
                case 8: ldp.StoredRules.PlayoffWeeks = 3; break;
                case 16: ldp.StoredRules.PlayoffWeeks = 4; break;
                default: ldp.StoredRules.PlayoffWeeks = 1; break;
            }
		}
		
		private void SeasonDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            ldp.StoredRules.SeasonWeeks = (int)cb_seasonDuration.SelectedValue;
            if (startSelected)
            {
                ldp.StoredRules.SeasonEnd = ldp.StoredRules.SeasonStart;
                ldp.StoredRules.SeasonEnd.AddDays((int)cb_seasonDuration.SelectedValue * 7);
                Trace.WriteLine("New Season End: " + ldp.StoredRules.SeasonEnd);
            }
            endSelected = true;
            if(breakSelected) // Recalculate playoff break with new season end
            {
                ldp.StoredRules.PlayoffStart = ldp.StoredRules.SeasonEnd;
                ldp.StoredRules.PlayoffStart.AddDays((int)cb_playoffBreak.SelectedValue * 7);
            }
		}
		
		private void PlayoffBreak_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (endSelected)
            {
                ldp.StoredRules.PlayoffStart = ldp.StoredRules.SeasonEnd;
                ldp.StoredRules.PlayoffStart.AddDays((int)cb_playoffBreak.SelectedValue * 7);
            }
            breakSelected = true;
		}
		
		private void DraftDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ldp.StoredRules.DraftTime = (DateTime)dp_draftDate.SelectedDate;
		}
		
		private void StartDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            startSelected = true;
			ldp.StoredRules.SeasonStart = (DateTime)dp_seasonStartDate.SelectedDate;
            if(endSelected) // Recalculate SeasonEnd with new value
            {
                ldp.StoredRules.SeasonEnd = ldp.StoredRules.SeasonStart;
                ldp.StoredRules.SeasonEnd.AddDays((int)cb_seasonDuration.SelectedValue * 7);
            }
            if (breakSelected) // Recalculate playoff break with new season end
            {
                ldp.StoredRules.PlayoffStart = ldp.StoredRules.SeasonEnd;
                ldp.StoredRules.PlayoffStart.AddDays((int)cb_playoffBreak.SelectedValue * 7);
            }
		}
		
		private void Superbowl_SelectionChanged(object sender,RoutedEventArgs e)
		{
			ldp.StoredRules.TeamSuperbowl = (bool)rb_superbowlYes.IsChecked;
		}
		
		private void MutualMatching_SelectionChanged(object sender,RoutedEventArgs e)
		{
			ldp.StoredRules.MutualMatching = (bool)rb_mutualMatchYes.IsChecked;
		}
		
    }
}
