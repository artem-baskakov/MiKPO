using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FuncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            string dir=Environment.CurrentDirectory+"\\..\\..\\..\\met1\\bin\\Debug\\";
            p.StartInfo.FileName= dir+"met1.exe";

            string[] pars = new string[]{
                "a.txt b.txt",
                "big.txt bigres.txt",
                "empty.txt emptyres.txt",
                "notExists.txt notExistsRes.txt"
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
