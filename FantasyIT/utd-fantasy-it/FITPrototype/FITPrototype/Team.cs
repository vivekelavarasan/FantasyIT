using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FITPrototype
{
    public class Team : IComparable<Team>,ISerializable
    {
        #region Class members
        private List<Employee> members;
        private int total, place;
        private String teamName;
        private List<Employee> substitutes;
        private Team freeAgents;

        public List<Employee> Members
        {
            get
            {
                return members;
            }
        }
        public int Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }
        public int Place
        {
            get
            {
                return place;
            }
            set
            {
                place = value;
            }
        }
        public String TeamName
        {
            get
            {
                return teamName;
            }
            set
            {
                teamName = value;
            }
        }
        public List<Employee> Substitutes { get { return substitutes; } set { substitutes = value; } }
        public Team FreeAgents { get { return freeAgents; } set { freeAgents = value; } }
        #endregion

        #region Constructors
        public Team()
        {
            teamName = "Unnamed Team";
            substitutes = new List<Employee>();
            members = new List<Employee>();
            total = 0;
            place = 0;
        }
        public Team(String n)
        {
            teamName = n;
            members = new List<Employee>();
            substitutes = new List<Employee>();
            total = 0;
            place = 0;
        }
        public Team(List<Employee> empList)
        {
            members = empList;
			teamName = "Team "+members[0].GetName();
            substitutes = new List<Employee>();
            foreach (Employee e in members)
			{
                total += e.GetPoints();
				e.SetTeam(teamName);
            }
			place = 0;
            
        }
        public Team(String n, List<Employee> empList)
        {
            teamName = n;
            members = empList;
            substitutes = new List<Employee>();
            foreach (Employee e in members)
			{
                total += e.GetPoints();
				e.SetTeam(teamName);
            }
			place = 0;
        }
        public Team(List<Employee> empList, int p)
        {
            members = empList;
            substitutes = new List<Employee>();
            foreach (Employee e in members)
                total += e.GetPoints();
            place = p;
            teamName = "Team " + members[0].GetName();
        }
        public Team(String n, List<Employee> empList, int p)
        {
            teamName = n;
            members = empList;
            substitutes = new List<Employee>();
            foreach (Employee e in members)
                total += e.GetPoints();
            place = p;
        }
        public Team(SerializationInfo info, StreamingContext ctxt)
        {
            teamName = (string)info.GetValue("TeamName", typeof(string));
            members = (List<Employee>)info.GetValue("TeamMembers", typeof(List<Employee>));
            place = (int)info.GetValue("TeamPlace", typeof(int));
            substitutes = (List<Employee>)info.GetValue("TeamSubstitutes", typeof(List<Employee>));
            freeAgents = (Team)info.GetValue("TeamFreeAgents", typeof(Team));
        }
        #endregion

        public void AddMember(Employee e)
        {
            members.Add(e);
            total += e.GetPoints();
        }
        public void AddMembers(List<Employee> es)
        {
            members.AddRange(es);
            foreach (Employee e in es)
                total += e.GetPoints();
        }
        public void RemoveMember(String s)
        {
            Employee m = members.Find(x => x.GetName() == s);
            members.Remove(m);
            total -= m.GetPoints();
        }
		public void RemoveAll()
		{
            members.Clear();
		}
        #region Output functions
		
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("TeamName",teamName);
			info.AddValue("TeamMembers",members);
			info.AddValue("TeamPlace",place);
            info.AddValue("TeamSubstitutes", substitutes);
            info.AddValue("TeamFreeAgents", freeAgents);
		}
		
        public override String ToString()
        {
            String s = teamName + " : " + place;
            switch(place%10)
            {
                case 1: s += "st"; break;
                case 2: s += "nd"; break;
                case 3: s += "rd"; break;
                default: s += "th"; break;
            }
            s += " : " + total + " points\n";
            foreach (Employee e in members)
                s += e.ToString() +"\n";
            return s;
        }
        public List<Object> ToGrid()
        {
            List<Object> li = new List<Object>();
            li.Add(teamName);
            li.Add(place);
            li.Add(total);
            foreach (Employee e in members)
                li.Add(e.GetName());
            return li;
        }
        #endregion

        public int CompareTo(Team t)
        {
            if (t == null)
                return 1;
            return this.total.CompareTo(t.Total);
        }
		
        public void SubCheck(int k) // Check if we need subs for week k, or if we need to return free agents to their teams on week k
        {
            bool subbing = false;
            int injuredCount = 0;
            foreach (Employee e in members)
            {
                if (e.PointsByWeek[k] == 0)
                {
                    injuredCount++;
                    Trace.WriteLine(e.Name + " was injured on week " + k);
                }
            }

            while (injuredCount > substitutes.Count)
            {
                Employee sub = new Employee();
                int highestPoints = 0;
                foreach (Employee ee in freeAgents.Members)
                {
                    if (ee.PointsByWeek[k] >= highestPoints)
                    {
                        sub = ee;
                        highestPoints = ee.PointsByWeek[k];
                    }
                }
                if (sub.Team != "Unnamed Team")
                {
                    Trace.WriteLine("Substituting in " + sub.Name + " for week " + k + "...");
                    freeAgents.Members.Remove(sub);
                    substitutes.Add(sub);
                    subbing = true;
                }
                else
                {
                    Trace.WriteLine(teamName + " is out of luck; no more free agents this week, but they need a sub...");
                    break;
                }
            }

            if (!subbing)
            {
                if (substitutes.Count > 0)
                {
                    foreach (Employee e in substitutes)
                    {
                        Trace.WriteLine("Returning " + e.Name + " to free agency.");
                        freeAgents.AddMember(e);
                    }
                    substitutes.Clear();
                }
            }
        }

        public void ClearSubs()
        {
            foreach (Employee e in substitutes)
            {
                Trace.WriteLine("Returning " + e.Name + " to free agency.");
                freeAgents.AddMember(e);
            }
            substitutes.Clear();
        }

		public int GetPointsGainedForWeek(int k)
		{
			int gained = 0;
            int sc = 0;
            //SubCheck(k);
            foreach (Employee e in members)
            {
                if(e.PointsByWeek[k] > 0)
                    gained += e.GetScoreOfWeek(k);
                else
                {
                    if (substitutes.Count > sc)
                    {
                        gained += substitutes[sc].GetScoreOfWeek(k);
                        sc++;
                    }
                }
            }
				
			return gained;
		}

        public int GetTotalForWeek(int k)
        {
            int gained = 0;
            int sc = 0;
            //SubCheck(k);
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].PointsByWeek[k] > 0)
                    gained += members[i].GetTotalForWeek(k);
                else
                {
                    if (substitutes.Count > sc)
                    {
                        gained += substitutes[sc].GetTotalForWeek(k);
                        sc++;
                    }
                }
            }

            return gained;
        }

       /* public override String ToString()
        {
            String s = "";
            foreach(Employee e in members)
            {
                s += e.ToString();
            }
            return s;
        }*/

        #region Equatable methods
        public override int GetHashCode()
        {
            return members.GetHashCode() ^ teamName.GetHashCode() ^ total.GetHashCode() ^ place.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            return obj is Team && this == (Team)obj;
        }

        public static bool operator ==(Team a, Team b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Members == b.Members && a.Total == b.Total && a.Place == b.Place && a.TeamName == b.TeamName;
        }

        public static bool operator !=(Team a, Team b)
        {
            return !(a == b);
        }
        #endregion
    }
}
