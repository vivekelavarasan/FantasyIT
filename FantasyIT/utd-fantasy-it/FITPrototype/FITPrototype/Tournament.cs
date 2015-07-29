using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITPrototype
{
    /// <summary>
    /// Manages match-ups and rankings throughout the playoffs
    /// </summary>
    
    public class Match
    {
        private Team home, visitor, winner;
        private int ranking; // equal to depth in the tree; determines what kind of match this is 
        private Match left, right; // for use in binary tree; null is valid here so as to denote leaves

        public int Ranking { get; set; }
        public Team Home { get; set; }
        public Team Visitor { get; set; }
        public Match Left { get; set; }
        public Match Right { get; set; }

        public Match()
        {
            home = null;
            visitor = null;
            winner = null;
            ranking = 0;
            left = null;
            right = null;
        }

        public Match(int a)
        {
            home = null;
            visitor = null;
            winner = null;
            ranking = a;
            left = null;
            right = null;
        }

        public Match(Team a, Team b)
        {
            home = a;
            visitor = b;
            winner = null;
            ranking = 0;
            left = null;
            right = null;
        }

        public Match(Team a, Team b, int c)
        {
            home = a;
            visitor = b;
            winner = null;
            ranking = c;
            left = null;
            right = null;
        }

        public Match(Team a, Team b, int c, Match d, Match e)
        {
            home = a;
            visitor = b;
            winner = null;
            ranking = c;
            left = d;
            right = e;
        }

        // Important for when we don't know what the match-ups will be yet, but we need to construct a tree
        public Match(Match a, Match b)
        {
            home = null;
            visitor = null;
            winner = null;
            ranking = 0;
            left = a;
            right = b;
        }

        #region Equatable methods
        public override bool Equals (Object obj)
        {
            return obj is Match && this == (Match)obj;
        }

        public override int GetHashCode()
        {
 	        return home.GetHashCode() ^ visitor.GetHashCode() ^ ranking.GetHashCode();
        }

        public static bool operator ==(Match a, Match b)
        {
            if(a.Ranking == b.Ranking)
            {
                if (a.Home == b.Home && a.Visitor == b.Visitor)
                    return true;
            }
            return false;
        }

        public static bool operator !=(Match a, Match b)
        {
            return !(a == b);
        }
        #endregion
    }

    class Tournament
    {
        private Match superbowl;
        private int depth;

        public Match Superbowl { get; set; }

        public Tournament()
        {
            superbowl = new Match();
            depth = 1;
        }

        // Adds a new match on the shallowest level possible; i.e., in a "breadth-first" pattern
        public void AddMatch(Team a, Team b)
        {
            if(!(superbowl.Left == null && superbowl.Right == null))
            {
                if(superbowl.Left == null)
                    superbowl.Left = new Match(a, b, superbowl.Ranking+1);
                else
                    superbowl.Right = new Match(a, b, superbowl.Ranking + 1);
            }
        }

        private void AddMatch(Match root, Team a, Team b)
        {

        }
    }
}
