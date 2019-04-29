using System;
using System.Collections.Generic;

namespace VotingApp
{
    class Candidate : IComparable<Candidate>
    {
        public string Name { get; private set; }
        public int Vote { get; private set; }
        public bool IsActive { get; private set; } = true;
        public List<Bulletin> Bulletins { get; private set; } = new List<Bulletin>();

        private const int maxLenghthName = 80;

        public Candidate(string name)
        {
            if (name.Length > maxLenghthName)
            {
                // throw new Exception("Слишком большая длина");
                // варианта обработки
                name = name.Substring(0, maxLenghthName);
            }
            Name = name;
        }

        public void AddBulletin(Bulletin bulletin)
        {
            if (bulletin.Peek() == this)
            {
                Bulletins.Add(bulletin);
                TakeVote();
            }
        }

        private void TakeVote()
        {
            Vote++;
        }

        public void ChangeState()
        {
            IsActive = false;
            foreach (var bul in Bulletins)
            {
                Candidate candidate = bul.NextCandidate();
                if (candidate != null)
                    candidate.AddBulletin(bul);
            }
        }

        public int CompareTo(Candidate other)
        {
            // return this.Vote.CompareTo(other.Vote);

            if (this.Vote > other.Vote)
                return 1;
            if (this.Vote < other.Vote)
                return -1;
            else
                return 0;
        }
    }
}
