using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FITPrototype 
{
    public class League : ISerializable
    {
        /// <summary>
        /// Contains league rules, stores and retrieves rules from a file, maintains leaderboard and game progression
        /// </summary>
        
        #region Class members
        private int teams, membersPerTeam, extrasPerTeam;
        private byte playoffTeams; // 4, 8, or 16
        private byte gameNight; // 0 = Thursday, 1 = Sunday, 2 = Monday
        private byte simultaneousTeams; // Number of teams one player can be on
        private bool mutualMatching; // True: players must be invited and they must accept to join a team, false: GMs draft whichever players they choose
        private bool teamSuperbowl; // True: there will be a team project for the superbowl game
        private DateTime draftTime; // Draft day and time
        private DateTime seasonStart;
        private DateTime seasonEnd; // Can be at least 6 weeks later than seasonStart, but never less
        private DateTime playoffStart; // Can be 0, 1, or 2 weeks later than seasonEnd
        private FileInfo storedRules;
        private List<Employee> freeAgents; // If an employee is not on a team, they are in this list of free agents
        private List<Team> leagueTeams; // List of all teams in the league
        private int totalWeeks; // for reference
        private int seasonWeeks;
        private int playoffWeeks;

        public bool initialized; // Tells whether or not values have been fully assigned and the league is ready to go

        public int Teams
        {
            get
            {
                return teams;
            }
            set
            {
                teams = value;
            }
        }
        public int MembersPerTeam
        {
            get
            {
                return membersPerTeam;
            }
            set
            {
                membersPerTeam = value;
            }
        }
        public int ExtrasPerTeam
        {
            get
            {
                return extrasPerTeam;
            }
            set
            {
                extrasPerTeam = value;
            }
        }
        public byte PlayoffTeams
        {
            get
            {
                return playoffTeams;
            }
            set
            {
                playoffTeams = value;
            }
        }
        public byte GameNight
        {
            get
            {
                return gameNight;
            }
            set
            {
                gameNight = value;
            }
        }
        public byte SimultaneousTeams
        {
            get
            {
                return simultaneousTeams;
            }
            set
            {
                simultaneousTeams = value;
            }
        }
        public bool MutualMatching
        {
            get
            {
                return mutualMatching;
            }
            set
            {
                mutualMatching = value;
            }
        }
        public bool TeamSuperbowl
        {
            get
            {
                return teamSuperbowl;
            }
            set
            {
                teamSuperbowl = value;
            }
        }
        public DateTime SeasonStart
        {
            get
            {
                return seasonStart;
            }
            set
            {
                seasonStart = value;
            }
        }
        public DateTime SeasonEnd
        {
            get
            {
                return seasonEnd;
            }
            set
            {
                seasonEnd = value;
            }
        }
        public DateTime DraftTime
        {
            get
            {
                return draftTime;
            }
            set
            {
                draftTime = value;
            }
        }
        public DateTime PlayoffStart
        {
            get
            {
                return playoffStart;
            }
            set
            {
                playoffStart = value;
            }
        }
        public List<Employee> FreeAgents
        {
            get
            {
                return freeAgents;
            }
            set
            {
                freeAgents = value;
            }
        }
        public List<Team> LeagueTeams
        {
            get
            {
                return leagueTeams;
            }
            set
            {
                leagueTeams = value;
            }
        }
        public int TotalWeeks
        {
            get { return totalWeeks; }
        }
        public int SeasonWeeks { get { return seasonWeeks; } set { seasonWeeks = value;  } }
        public int PlayoffWeeks { get { return playoffWeeks; } set { playoffWeeks = value; } }
        #endregion

        #region Constructors
        public League() // instantiate new league; to be edited in program and saved afterwards using LeagueDataParser
        {
            teams = 0; membersPerTeam = 0; extrasPerTeam = 0;
            playoffTeams = 0; gameNight = 0; simultaneousTeams = 0;
            mutualMatching = false; teamSuperbowl = false;
            draftTime = new DateTime(); seasonStart = new DateTime();
            seasonEnd = new DateTime(); playoffStart = new DateTime();
            freeAgents = new List<Employee>();
            leagueTeams = new List<Team>();
            totalWeeks = 0; playoffWeeks = 0; seasonWeeks = 0;
            this.GetTeams();
            initialized = false;
        }
        
		public League(SerializationInfo info, StreamingContext ctxt)
		{
			teams = (int)info.GetValue("NumTeams",typeof(int));
			membersPerTeam = (int)info.GetValue("NumPerTeam",typeof(int));
			extrasPerTeam = (int)info.GetValue("NumExtrasPerTeam",typeof(int));
			playoffTeams = (byte)info.GetValue("PlayoffTeams",typeof(byte));
			gameNight = (byte)info.GetValue("GameNight",typeof(byte));
			simultaneousTeams = (byte)info.GetValue("SimultaneousTeams",typeof(byte));
			mutualMatching = (bool)info.GetValue("MutualMatching",typeof(bool));
			teamSuperbowl = (bool)info.GetValue("Superbowl",typeof(bool));
			draftTime = (DateTime)info.GetValue("DraftTime",typeof(DateTime));
			seasonStart = (DateTime)info.GetValue("SeasonStart",typeof(DateTime));
			seasonEnd = (DateTime)info.GetValue("SeasonEnd",typeof(DateTime));
			playoffStart = (DateTime)info.GetValue("PlayoffStart",typeof(DateTime));
			storedRules = (FileInfo)info.GetValue("StoredRules",typeof(FileInfo));
            seasonWeeks = (int)info.GetValue("SeasonWeeks", typeof(int));
            playoffWeeks = (int)info.GetValue("PlayoffWeeks", typeof(int));
            totalWeeks = (int)(seasonWeeks + playoffWeeks);
			this.GetTeams();
		}
        #endregion

        public void AddTeam(Team t)
        {
            if (!(leagueTeams.Count >= teams))
                leagueTeams.Add(t);
        }

        public void RemoveTeam(String s)
        {
            foreach(Team t in leagueTeams)
            {
                if (t.TeamName == s)
                {
                    leagueTeams.Remove(t);
                    break;
                }
            }
        }
		
        public void UpdateWeeks()
        {
            /*
            TimeSpan ts = seasonStart.Subtract(seasonEnd);
            totalWeeks = (int)(ts.TotalDays / 7);*/
            totalWeeks = seasonWeeks + playoffWeeks;
        }

		public void GetTeams()
		{
			TeamDataParser tdp = new TeamDataParser();
			leagueTeams = tdp.GetTeams();
		}

        public void Update() // Sorts all the teams in order of points so that the leaderboards are properly updated; team name is the tiebreaker
        {
            leagueTeams.OrderByDescending(x => x.Total)
                        .ThenBy(x => x.TeamName);
            int i = 0;
            foreach(Team t in leagueTeams) // Debug
                Trace.WriteLine(++i + ": " + t.TeamName + ": " + t.Total);
            
        }

        /*      public void TransferToTeam(String s, String t) // Transfer employee with name S from free agency to a team with name T
              {
                  foreach (Employee e in )
                  {
                      if (te.TeamName == s)
                      {
                          leagueTeams.Remove(te);
                          break;
                      }
                  }
              }*/

        #region Output functions
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("NumTeams", teams);
            info.AddValue("NumPerTeam", membersPerTeam);
            info.AddValue("NumExtrasPerTeam", extrasPerTeam);
            info.AddValue("PlayoffTeams", playoffTeams);
            info.AddValue("GameNight", gameNight);
            info.AddValue("SimultaneousTeams", simultaneousTeams);
            info.AddValue("MutualMatching", mutualMatching);
            info.AddValue("Superbowl", teamSuperbowl);
            info.AddValue("DraftTime", draftTime);
            info.AddValue("SeasonStart", seasonStart);
            info.AddValue("SeasonEnd", seasonEnd);
            info.AddValue("PlayoffStart", playoffStart);
            info.AddValue("StoredRules", storedRules);
            info.AddValue("TotalWeeks", totalWeeks);
            info.AddValue("SeasonWeeks", seasonWeeks);
            info.AddValue("PlayoffWeeks", playoffWeeks);
        }

        public override String ToString()
        {
            String s = "";
            s += this.membersPerTeam + "\n";
            s += this.extrasPerTeam + "\n";
            s += this.playoffTeams + "\n";
            s += this.gameNight + "\n";
            s += this.simultaneousTeams + "\n";
            s += this.mutualMatching + "\n";
            s += this.teamSuperbowl + "\n";
            s += this.draftTime.Year + this.draftTime.Month + this.draftTime.Day + this.draftTime.Hour + this.draftTime.Minute + this.draftTime.Second + "\n";
            s += this.seasonStart.Year + this.seasonStart.Month + this.seasonStart.Day + "\n";
            s += this.seasonEnd.Year + this.seasonEnd.Month + this.seasonEnd.Day + "\n";
            s += this.playoffStart.Year + this.playoffStart.Month + this.playoffStart.Day + "\n";
            s += this.TeamsToString();
            return s;
        }

        private String TeamsToString() // Puts all of the league teams into storable string format. Team and employee names have spaces replaced with underscores
        {
            String s = "";
            foreach(Team t in leagueTeams)
            {
                s += "TEAM_" + t.TeamName.Replace(' ', '_') +" " + t.Place + " " + t.Members.Count + "\n";
                foreach(Employee e in t.Members) 
                    s += e.GetName().Replace(' ','_') + " " + e.GetPoints() + " " + e.GetRole() + "\n"; // Hopefully, only employee name will be needed since it can be cross-referenced with Samantha (?)
            }
            s += "FREEAGENTS\n";
            foreach(Employee e in freeAgents)
            {
                s += e.GetName().Replace(' ','_') + " " + e.GetPoints() + " " + e.GetRole() + "\n";
            }
            return s;
        }

        public void ToFile() // Automatically overwrites existing files; creates otherwise
        {
            File.WriteAllText(storedRules.FullName, this.ToString());
        }
        #endregion
    }
}
