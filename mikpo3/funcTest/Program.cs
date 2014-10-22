using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace funcTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            string dir = Environment.CurrentDirectory + "\\..\\..\\..\\mikpo3\\bin\\Debug\\";
            p.StartInfo.FileName = dir + "mikpo3.exe";

            string[] pars = new string[]{
                "4 20 2 6",
                "a 0 l a",
                "-100 7 8 0",
            };

            foreach (string par in pars)
            {
                p.StartInfo.Arguments = par;
                p.Start();
                p.Close();
            }

        }
    }
}
