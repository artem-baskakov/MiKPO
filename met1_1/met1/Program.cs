using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace met1
{
    public class Program
    {
        public static double GradToRad(double x)
        {
            x = x * Math.PI / 180;
            return x;
        }

        public static double RadToGrad(double x)
        {
            x = x * 180 / Math.PI;
            return x;
        }

        public struct data
        {
            public double a;
            public double b;
            public double c;
        }

        public static double[] CalcTriangle(double a, double b, double gamma)
        {
            if ((a > 0) && (b > 0) && ((gamma > 0) && (gamma < 180)))
            {
                double gam = GradToRad(gamma);
                double alf, bet, c;
                c = Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(gam));
                alf = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
                bet = Math.Acos((a * a + c * c - b * b) / (2 * a * c));
                gam = RadToGrad(gam);
                alf = RadToGrad(alf);
                bet = RadToGrad(bet);
                return new double[] { a, b, Math.Round(c, 2), Math.Round(alf, 2), Math.Round(bet, 2), gam };
            }
            else
            {
                return new double[] { -1 };
            }
        }

        public static void work(string path, string outpath)
        {
            List<data> d = new List<data>();
            List<string> output = new List<string>();
            List<string> log = new List<string>();
            StreamWriter stw = new StreamWriter(outpath);
            if (File.Exists(path))
            {
                StreamReader str = new StreamReader(path);
                string s = "", s1 = "";
                //s1 = File.ReadAllText(path);
                if (str.BaseStream.Length > 0)
                {
                    while ((s = str.ReadLine()) != null)
                    {
                        if (s.Length != 0)
                        {
                            string[] ss = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (ss.Length == 3)
                            {
                                d.Add(new data
                                {
                                    a = Convert.ToDouble(ss[0]),
                                    b = Convert.ToDouble(ss[1]),
                                    c = Convert.ToDouble(ss[2])
                                });
                            }
                            else
                            {
                                output.Add("Количество параметров неверно");
                            }
                        }

                    }
                    str.Close();
                }
                else
                {
                    Console.WriteLine("Файл пуст");
                }
            }
            else
            {
                Console.WriteLine("Файл не существует");
            }

            for (int i = 0; i < d.Count; i++)
            {
                double[] res = CalcTriangle(d[i].a, d[i].b, d[i].c);
                if (res[0] != -1)
                {
                    output.Add(string.Join("; ", res));
                    log.Add(res[2] + "; " + res[3] + "; " + res[4]);
                }
                else
                {
                    output.Add("Параметры некорректны");
                    log.Add("Параметры некорректны");
                }

            }

            for (int i = 0; i < output.Count; i++)
            {
                stw.WriteLine(output[i]);                
            }
            for (int i = 0; i < log.Count; i++)
            {
                Console.WriteLine(log[i]);
            }
            stw.Close();
        }


        static void Main(string[] args)
        {
            bool manualInput = false;
            if (args.Length != 2)
            {
                args = new string[2];
                args[0] = Console.ReadLine();
                args[1] = Console.ReadLine();
                manualInput = true;
            }

            string path = args[0];
            string outpath = args[1]; 
           

            work(path, outpath);

            if (manualInput) Console.ReadLine();
        }
    }
}
