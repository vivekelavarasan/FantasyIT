using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FITPrototype
{
	//[Serializable()]
    public class Employee : IComparable<Employee>, ISerializable
    {
        private String name;
        private int pointsTotal;
		private String teamName, role;
		private List<int> pointsByWeek;
        private bool injured;
		
		public String Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
		public int Points
		{
			get
			{
				return pointsTotal;
			}
			set
			{
				pointsTotal = value;
			}
		}
		public String Role
		{
			get
			{
				return role;
			}
			set
			{
				role = value;
			}
		}
		public String Team
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
        public String TeamName { get { return teamName; } set { teamName = value; } }
        public bool Injured { get { return injured; } set { injured = value; } }
		
		public List<int> PointsByWeek
		{
			get
			{
				return pointsByWeek;
			}
			set
			{
				pointsByWeek = value;
			}
		}
		
	
        public Employee()
        {
            name = "John Q. Public";
            pointsTotal = 0;
			pointsByWeek = new List<int>();
            role = "";
            injured = false;
			teamName = "Unnamed Team";
        }
        public Employee(String n, int p, String t)
        {
            name = n;
            pointsTotal = p;
			pointsByWeek = new List<int>();
			teamName = t;
            role = "";
            injured = false;
			pointsByWeek.Add(p);
        }
        public Employee(String n, int p, String r, String t)
        {
            name = n;
            pointsTotal = p;
			pointsByWeek = new List<int>();
            role = r;
			teamName = t;
            injured = false;
			
			pointsByWeek.Add(p);
        }
        public Employee(String n, List<int> pbw, String r, String t)
        {
            name = n;            
            pointsByWeek = pbw;
            pointsTotal = 0;
            foreach(int i in pointsByWeek)
            {
                pointsTotal += i;
            }
            role = r;
            teamName = t;
            injured = false;
        }
		public Employee(SerializationInfo info,StreamingContext ctxt)
		{
			name = (string)info.GetValue("EmployeeName",typeof(string));
			pointsTotal = (int)info.GetValue("EmployeePointsTotal",typeof(int));
			pointsByWeek = (List<int>)info.GetValue("EmployeePointsArray",typeof(List<int>));
			role = (String)info.GetValue("EmployeeRole",typeof(string));
			teamName = (string)info.GetValue("EmployeeTeam",typeof(string));
            injured = (bool)info.GetValue("EmployeeInjured",typeof(bool));
		}
		public int GetScoreOfWeek(int w)
		{
			return pointsByWeek[w];
		}
        public int GetTotalForWeek(int k)
        {
            int diff = 0;
            int weeks = pointsByWeek.Count;
                for (int j = k + 1; j < weeks; j++)
                {
                    if (j >= weeks)
                        break;
                    diff += GetScoreOfWeek(j);
                }

            return pointsTotal - diff;
        }
        public String GetName()
        {
            return name;
        }
        public int GetPoints()
        {
            return pointsTotal;
        }
        public String GetRole()
        {
            return role;
        }
		public String GetTeam()
		{
			return teamName;
		}
        public void SetName(String s)
        {
            name = s;
        }
        public void SetPoints(int p)
        {
            pointsTotal = p;
        }
        public void SetRole(String r)
        {
            role = r;
        }
		public void SetTeam(String t)
		{
			teamName = t;
		}
		public void AddPoints(int n)
		{
			pointsByWeek.Add(n);
			pointsTotal+=n;
		}
		//method used to retrieve serialized data
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("EmployeeName",name);
			info.AddValue("EmployeePointsTotal",pointsTotal);
			info.AddValue("EmployeePointsArray",pointsByWeek);
			info.AddValue("EmployeeRole",role);
			info.AddValue("EmployeeTeam",teamName);
            info.AddValue("EmployeeInjured", injured);
		}
        public override String ToString()
        {
            String s = name + ", " + role + " : " + pointsTotal;
            return s;
        }
		
		public int CompareTo(Employee e)
		{
			if(e==null)
				return 1;
			//return this.points.CompareTo(e.GetPoints());
            return e.GetPoints().CompareTo(this.GetPoints());
        }
    }
}
