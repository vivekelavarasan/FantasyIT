using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FITPrototype
{
	public class PlayoffTeamParser
	{
		private List<string> teamNames;
		
		public PlayoffTeamParser()
		{
			if(File.Exists("playoffplayers.xml"))
			{
				teamNames = LoadPlayers("playoffplayers.xml");;
			}
		}
		
		public PlayoffTeamParser(List<string> tn)
		{
			teamNames = tn;
			SavePlayers(teamNames,"playoffplayers.xml");
		}
		
		public List<string> GetPlayoffTeamNames()
		{
			return teamNames;
		}
		
		public static List<string> LoadPlayers(string file)
		{
			List<string> names;
		
			string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			string fileName = dir + @"\" + file;
			
			if(!File.Exists(fileName))
				throw new FileNotFoundException("The playoff team list could not be found",fileName);
				
			
			using(FileStream fstream = new FileStream(fileName,FileMode.Open))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
				names = (List<string>)serializer.Deserialize(fstream);
			}
		
			return names;
		}
		
		public static void SavePlayers(List<string> pList,string file)
		{
			string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			string fileName = dir + @"\" + file;
			
			//creates a backup file that we can revert to
			string backup = Path.ChangeExtension(fileName,".old");
			if(File.Exists(fileName))
			{
				if(File.Exists(backup))
					File.Delete(backup);
					
				File.Move(fileName,backup);
			}
			
			//serializes the list of teams
			using (FileStream fstream = new FileStream(fileName,FileMode.Create))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
				serializer.Serialize(fstream,pList);
				fstream.Flush();
				fstream.Close();
			}
		}
		
	}
}