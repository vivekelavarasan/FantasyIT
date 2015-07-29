﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateTeam.xaml
    /// </summary>
    public partial class CreateTeam : Page
    {
        private Team myTeam;
        private List<Employee> members;


        //for now reused data structure from leaderboard, eventually will be tied to database using Data Binding so won't need to perform this
        private struct rowData
        {
            public String name { set; get; }
            public int points { set; get; }
            //public String role { set; get; }
        }

        public CreateTeam()
        {
            InitializeComponent();
            TeamDataParser td = new TeamDataParser();

            //Replace team[0] with list of potential employees playing
            myTeam = td.GetTeams()[0];
            members = myTeam.Members;
 

            //dataGrid population
            DataGridTextColumn memberName = new DataGridTextColumn();
            DataGridTextColumn memberScore = new DataGridTextColumn();
            DataGridTextColumn memberName2 = new DataGridTextColumn();
            DataGridTextColumn memberScore2 = new DataGridTextColumn();
            //DataGridTextColumn memberRole = new DataGridTextColumn();
            //add whatever other team member data we decide to put
            //look back over and see if there is more efficient way to do this
            teamData.Columns.Add(memberName);
            teamData.Columns.Add(memberScore);
            team.Columns.Add(memberName2);
            team.Columns.Add(memberScore2);
            memberName2.Binding = new Binding("name");
            memberScore2.Binding = new Binding("points");
            memberName2.Header = "Member Name";
            memberScore2.Header = "Previous Score";

            //teamData.Columns.Add(memberRole);
            memberName.Binding = new Binding("name");
            memberScore.Binding = new Binding("points");
            //memberRole.Binding = new Binding("role");
            memberName.Header = "Member Name";
            memberScore.Header = "Previous Score";
            //memberRole.header = "Member Role";

            foreach (Employee empl in members)
            {
                teamData.Items.Add(new rowData { name = empl.GetName(), points = empl.GetPoints() });
            }


        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var grid = teamData;
            var si = grid.SelectedItems;
            if (grid.SelectedIndex >= 0)
            {
                for (int i = 0; i <= si.Count; i++)
                    grid.Items.Remove(si[i]);
            }
        }

        private void FormTeam_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var grid = teamData;
            var si = grid.SelectedItems;
            int index = grid.SelectedIndex;
            if (index >= 0)
            {
                for (int i = 0; i <= si.Count; i++)
                    grid.Items.Remove(si[i]);
            }
            int k = 0;
            foreach (Employee empl in members)
            {
                if (k == index)
                {
                    team.Items.Add(new rowData { name = empl.GetName(), points = empl.GetPoints() });
                    k++;
                }
                else k++;
            }
        }


    }
}
