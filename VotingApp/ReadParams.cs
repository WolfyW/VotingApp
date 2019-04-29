using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VotingApp
{
    class ReadParams : IDisposable
    {
        public int BlockCount { get; private set; }
        public int NumberCandidate { get; private set; }
        public Dictionary<int, Candidate> Candidats { get; private set; }
        public List<Bulletin> Bull { get; private set; }
        public string Path { get; private set; }

        private StreamReader Reader;
        private const int naxNumberCandidate = 20;
        private const int maxBulleteins = 1000;

        public ReadParams(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            Path = path;
            Reader = new StreamReader(Path, Encoding.Default);
            GetBlockCount();
        }

        private void GetBlockCount()
        {
            string numberTestBlock = Reader.ReadLine();
            Reader.ReadLine();
            bool flag = int.TryParse(numberTestBlock, out int number);
            if (flag)
            {
                BlockCount = number;
            }
            else
            {
                throw new FormatException("Неверное начало файла");
            }
        }

        public void GetParamsFromFile()
        {
            string numberCandidate;
            
            string[] names;
            List<string> bulletin = new List<string>();

            numberCandidate = Reader.ReadLine();
            NumberCandidate = int.Parse(numberCandidate);
                
            if (NumberCandidate > naxNumberCandidate)
                throw new FormatException("Количество участников не больше 20");

            names = new string[NumberCandidate];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = Reader.ReadLine();
            }

            string bul;
            int count = 0;
            while((bul = Reader.ReadLine()) != "" && bul != null)
            {
                bulletin.Add(bul);
                count++;
                if (count > maxBulleteins)
                    throw new FormatException("Слишком много билютеней");
            }

            if (count == 0)
                throw new FormatException("Отсутсвуют бюллетени");

            GetCandidate(names);
            GetBulletens(bulletin);
        }

        private void GetCandidate(string[] names)
        {
            Candidats = new Dictionary<int, Candidate>();
            for (int i = 0; i < names.Length; i++)
            {
                Candidats.Add(i + 1, new Candidate(names[i]));
            }
        }

        private void GetBulletens(List<string> bul)
        {
            Bull = new List<Bulletin>();
            string[] temp;
            for (int numBul = 0; numBul < bul.Count; numBul++)
            {
                Candidate[] candidate = new Candidate[NumberCandidate];
                List<int> numberOfCandidates = Enumerable.Range(1, NumberCandidate).ToList();
                temp = bul[numBul].Split(' ');
                if (temp.Length > NumberCandidate)
                    continue;
                for (int j = 0; j < temp.Length; j++)
                {
                    bool flag = int.TryParse(temp[j], out int numCandidate);
                    if (flag)
                    {
                        // Проверяем, что каждый кандидат был записан только один раз
                        if (numberOfCandidates.Contains(numCandidate))
                        {
                            candidate[j] = Candidats[numCandidate];
                            numberOfCandidates.Remove(numCandidate);
                        }
                        else
                        {
                            Logger.WriteLog($"Пропущена бюллетень {numBul} в блоке {BlockCount}");
                            //что лучше прервать весь блок или просто исключить эту бюллетень?
                            break;
                        }

                    }
                    else
                    {
                        Logger.WriteLog($"Пропущена бюллетень {numBul} в блоке {BlockCount}");
                        // Переходим к следующей бюллетене
                        break;
                    }
                }
                Bull.Add(new Bulletin(candidate));
            }   
        }

        public void Dispose()
        {
            Reader.Close();
        }
    }
}
