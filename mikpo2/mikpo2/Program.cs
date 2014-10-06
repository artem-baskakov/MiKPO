using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace mikpo2
{
    class Program
    {
        static Bitmap input, output;
        static byte[,] r0, g0, b0;
        static int m = 3, stride = m / 2, mid_el = m*m/2; //N - настраиваемый параметр размера окна
        static object obj = new object();
        

        static void ToArrays()
        {
            r0 = new Byte[input.Height, input.Width];
            g0 = new Byte[input.Height, input.Width];
            b0 = new Byte[input.Height, input.Width];
            output = new Bitmap(input);
            for (int x = 0; x < input.Height; x++)
                for (int y = 0; y < input.Width; y++)
                {
                    Color c = input.GetPixel(y, x);
                    r0[x, y] = (Byte)c.R;
                    g0[x, y] = (Byte)c.G;
                    b0[x, y] = (Byte)c.B;
                } 
        }

        static void Filter(int startY, int endY)
        {
             int i, x1, x2, x3, y1, y2, y3; 
             Byte[] r = new Byte[m*m], g = new Byte[m*m], b = new Byte[m*m]; 
             for ( x1=stride; x1 < input.Height-stride; x1++ )
                {
                    for (y1 = startY; y1 < endY; y1++)
                        { 
                            i = 0;
                            for (x2 = -stride; x2 <= stride; x2++) 
                                { 
                                    x3 = x1 + x2;
                                    for (y2 = -stride; y2 <= stride; y2++)
                                    { 
                                        y3 = y1 + y2; 
                                        r[i] = r0[x3,y3]; 
                                        g[i] = g0[x3,y3]; 
                                        b[i] = b0[x3,y3]; 
                                        i++; 
                                     } 
                                 } 
                            Array.Sort(r);
                            Array.Sort(g); 
                            Array.Sort(b);
                            lock (obj)
                            {
                                output.SetPixel(y1, x1, Color.FromArgb(r[mid_el], g[mid_el], b[mid_el]));
                            }
                             
                         } 
                   } 
            } 

        
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                args = new string[2];
                args[0] = Console.ReadLine();
                args[1] = Console.ReadLine();
            }
            string path = args[0];
            string outpath = args[1];
            try
            {
                input = (Bitmap)Image.FromFile(path);
                ToArrays();
                int startY = 0, endY = 0, w = input.Width - stride;
                int N = 8;

              //  DateTime start = DateTime.Now;
                 Parallel.For(0, N, i =>
                    {
                        startY = stride + i * (w / N);
                        endY = startY + w / N;
                        if (endY > w) endY = w;
                        Filter(startY, endY);
                    });
                
               /*
                startY = stride;
                endY = input.Width - stride;
                Filter(startY, endY);*/
                
              //  DateTime end = DateTime.Now;
               // Console.WriteLine(end - start);
               // Console.ReadLine();

                output.Save(outpath);
            }
            catch
            {
                Console.WriteLine("Файл не существует или у него неверный формат");
                Console.ReadLine();
            }
        }
    }
}
