using System.Collections.Generic;

namespace VotingApp
{
    class Bulletin
    {
        Queue<Candidate> bulls = new Queue<Candidate>();

        public Bulletin(Candidate[] candidates)
        {
            foreach (var candid in candidates)
            {
                bulls.Enqueue(candid);
            }
            Candidate candidate = bulls.Peek();
            candidate.AddBulletin(this);
        }

        public Candidate Peek()
        {
            if (bulls.Count == 0)
                return null;
            return bulls.Peek();
        }

        public Candidate NextCandidate()
        {
            if (bulls.Count == 0)
                return null;
            bulls.Dequeue();
            if (bulls.Count == 0)
                return null;
            return bulls.Peek();
        }
    }
}
