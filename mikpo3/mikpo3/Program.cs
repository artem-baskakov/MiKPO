using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace mikpo3
{
    public class Program
    {
        class SMO
        {
            Queue<int>[] kassa;
            static Object obj = new object();
            static Object[] kassaObj;

            public int doneClients
            {
                get;
                private set;
            }

            public SMO(int KassaCount)
            {
                kassa = new Queue<int>[KassaCount];
                kassaObj = new object[KassaCount];
                doneClients = 0;

                for (int i = 0; i < kassa.Count(); i++)
                {
                    kassa[i] = new Queue<int>();
                    kassaObj[i] = new object();
                }
            }

            private List<KeyValuePair<int, int>> getSortedKassa()
            {
                List<KeyValuePair<int, int>> sortKassa = new List<KeyValuePair<int, int>>();

                for (int i = 0; i < kassa.Count(); i++)
                {
                    KeyValuePair<int, int> elem = new KeyValuePair<int, int>(i, kassa[i].Count());
                    sortKassa.Add(elem);
                }
                sortKassa.Sort((a, b) => a.Value.CompareTo(b.Value));
                return sortKassa;
            }

            public void clientComming(int clientCount)
            {
                int j = 0;
                List<KeyValuePair<int, int>> sortKassa = getSortedKassa();

                for (int k = 0; k < clientCount; k++)
                {
                    lock (kassaObj[j])
                    {
                        kassa[sortKassa[j].Key].Enqueue(k);
                    }
                    j = j == kassa.Count() - 1 ? 0 : j + 1;
                }
            }

            public void doneSevice()
            {
                Parallel.For(0, kassa.Count(), i =>
                {
                    if (kassa[i].Count() > 0)
                    {
                        lock (kassaObj[i])
                        {
                            kassa[i].Dequeue();
                        }
                        lock (obj)
                        {
                            doneClients++;
                        }
                    }

                });
            }

            public string getSMO()
            {
                string res = "";
                for (int i = 0; i < kassa.Count(); i++)
                {
                    res += (i + 1).ToString() + " Касса (длина очереди=" + kassa[i].Count() + ")" + ": " + String.Join(", ", kassa[i].ToArray()) + "\n";
                }
                res += "Всего обслужено: " + doneClients;

                return res;
            }
        }


        public static int work(int kassaCount, int clients, int servTime, int totalTime)
        {
            int timeUnit = 1000;
            int currentTime = 0;

            SMO smo = new SMO(kassaCount);

            while (currentTime <= totalTime)
            {
                Console.WriteLine("тик " + currentTime / 1000);

                if (currentTime > 0)
                    smo.clientComming(clients);
                Console.WriteLine(smo.getSMO());

                if (currentTime % servTime == 0 && currentTime != 0)
                {
                    smo.doneSevice();
                    Console.WriteLine("Обслужено");
                    Console.WriteLine(smo.getSMO());
                }

                currentTime += timeUnit;
                System.Threading.Thread.Sleep(timeUnit);
            }
            Console.WriteLine("end");
            return smo.doneClients;
        }

        static void Main(string[] args)
        {
            int kassaCount, clients, servTime, totalTime;
            if (args.Length != 4)
            {
                args = new string[4];
                args[0] = Console.ReadLine();
                args[1] = Console.ReadLine();
                args[2] = Console.ReadLine();
                args[3] = Console.ReadLine();
            }
            try
            {
                kassaCount = Convert.ToInt32(args[0]);
                clients = Convert.ToInt32(args[1]);
                servTime = Convert.ToInt32(args[2]) * 1000;
                totalTime = Convert.ToInt32(args[3]) * 1000;
                work(kassaCount, clients, servTime, totalTime);
            }
            catch
            {
                Console.WriteLine("Неверные данные");
            }
            Console.ReadLine();

        }
    }
}
