using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Day9
{
    internal class Program
    {
        public class Deriver
        {
            public List<List<long>> derivations = new List<List<long>>();
        }
        static void Main(string[] args)
        {
            var txt = File.ReadAllText("input.txt");

            txt = txt.Replace("\r", "");
            var lines = txt.Split('\n');

            var ders = new List<Deriver>();
            for (int ia = 0; ia < lines.Length; ia++)
            {
                var line = lines[ia];
                if (String.IsNullOrEmpty(line))
                    continue;

                var d = new Deriver();
                var fd = new List<long>();
                d.derivations.Add(fd);
                var ps = line.Split(' ');
                foreach(var p in ps)
                {
                    fd.Add(long.Parse(p));
                }
                ders.Add(d);
            }
            Derive(ders);

            bool first = false;
            long answer = 0;
            if (first)
            {
                Integrate(ders);
                answer = ders.Sum(d => d.derivations[0].Last());
            }
            else
            {
                IntegrateLeft(ders);
                answer = ders.Sum(d => d.derivations[0].First());
            }            
            
            Console.WriteLine("ANSWER : " + answer);
            Console.ReadLine();
        }

        static void Integrate(List<Deriver> derivers)
        {
            foreach (var d in derivers)
            {
                for(int i=d.derivations.Count-1;i>0;i--)
                {
                    var cd = d.derivations[i];
                    var nd = d.derivations[i - 1];
                    var nl = cd.Last() + nd.Last();
                    d.derivations[i-1].Add(nl);
                }
            }
        }

        static void IntegrateLeft(List<Deriver> derivers)
        {
            foreach (var d in derivers)
            {
                for (int i = d.derivations.Count - 1; i > 0; i--)
                {
                    var cd = d.derivations[i];
                    var nd = d.derivations[i - 1];
                    var nl =  nd.First() - cd.First();
                    d.derivations[i - 1].Insert(0,nl);
                }
            }
        }

        static void Derive(List<Deriver> derivers)
        {
            foreach(var d in derivers)
            {
                bool gotToZero = false;
                int curDer = 0;
                while(!gotToZero)
                {
                    int dcnt = d.derivations[curDer].Count() - 1;
                    var nd = new List<long>();
                    d.derivations.Add(nd);
                    for(int i = 0;i<dcnt;i++)
                    {
                        var dif = d.derivations[curDer][i + 1] - d.derivations[curDer][i];
                        nd.Add(dif);
                    }
                    // Check if all 0
                    bool allZero = true;
                    foreach(var n in nd)
                    {
                        if (n != 0)
                        {
                            allZero = false;
                            break;
                        }
                    }
                    gotToZero = allZero;
                    curDer++;
                }
            }
        }
    }

    
}
