using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace VotingApp
{
    class Voting
    {
        public int BlockCount { get; private set; }
        public int NumberCandidate { get; private set; }
        public Dictionary<int, Candidate> Candidats { get; private set; }
        public List<Bulletin> Bulletins { get; private set; }
        public string Path { get; private set; }
        private string Winers;

        public Voting(string path)
        {
            Path = path;
        }
        
        public string StsartVoiting()
        {
            try
            {
                using (ReadParams readParams = new ReadParams(Path))
                {
                    BlockCount = readParams.BlockCount;

                    for (int i = 0; i < BlockCount; i++)
                    {
                        try
                        {
                            readParams.GetParamsFromFile();
                            Candidats = readParams.Candidats;
                            Bulletins = readParams.Bull;
                            BlockCount = readParams.BlockCount;
                            NumberCandidate = readParams.NumberCandidate;
                            GetWinners();
                        }
                        catch (FormatException ex)
                        {
                            WriteError(ex);
                        }
                        catch (ArgumentNullException ex)
                        {
                            WriteError(ex);
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Logger.WriteLog(ex, "Файл не найден");
            }
            return Winers;
        }

        private void WriteError(Exception ex)
        {
            string error = "в блоке " + BlockCount;
            Logger.WriteLog(ex, error);
        }

        private void GetWinners()
        {
            bool isWin = false;
            Candidate[] winers = null;
            while (!isWin)
            {
                isWin = CheckWin(out winers);
            }
            foreach (var item in winers)
            {
                Winers += item.Name + "\n";
            }
            Winers += "\n";
        }

        private bool CheckWin(out Candidate[] winers)
        {
            winers = null;
            var can = from c in Candidats.Values
                      where c.IsActive != false
                      select c;

            Candidate[] candidats = can.ToArray();

            if (candidats.Count() == 0)
            {
                var v = from c in Candidats.Values
                        where c.Vote == Candidats.Values.Max(x => x.Vote)
                        select c;
                winers = v.ToArray();
                return true;
            }

            Array.Sort(candidats.ToArray());
            if (candidats.First().Vote > Bulletins.Count / 2d)
            {
                winers = new Candidate[] { candidats.First() };
                return true;
            }
            else
            {
                candidats.Last().ChangeState();
                return false;
            }
        }
    }
}
