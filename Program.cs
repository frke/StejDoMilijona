using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace StejDoMilijona
{
    internal class Program
    {
        private const string steviloNicel = "123";
        private const int steviloPonovitev = 1000000000;

        private static void Main(string[] args)
        {
            var overallwatch = System.Diagnostics.Stopwatch.StartNew();
            //Creating Threads
            Thread t1 = new Thread(Method1)
            {
                Name = "Thread1"
            };
            Thread t2 = new Thread(Method2)
            {
                Name = "Thread2"
            };
            Thread t3 = new Thread(Method3)
            {
                Name = "Thread3"
            };

            //Executing the methods
            t1.Start();
            t2.Start();
            t3.Start();
            Console.WriteLine("Main Thread Ended");

            overallwatch.Stop();
            var overallelapsedMs = overallwatch.ElapsedMilliseconds;
            Console.WriteLine("Elapsed miliseconds in Main thread: {0}, pritisni tipko za izhod\n", overallelapsedMs);
            Console.ReadKey();
        }

        private static void Method1()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string threadName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            long elapsedMs;
            for (int i = 0; i < steviloPonovitev; i++)
            {
                string hash = CalculateHash(CalculateHash(i.ToString()));

                if (hash.StartsWith(steviloNicel))
                {
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine("Thread: {0} zaporedna {1}, hash {2}", threadName, i, hash);
                    Console.WriteLine("Thread: {0} ElapsedMs: {1}\n", threadName, elapsedMs);
                    break;
                }
            }
            
        }

        private static void Method2()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i > -steviloPonovitev; i--)
            {
                string hash = CalculateHash(CalculateHash(i.ToString()));

                if (hash.StartsWith(steviloNicel))
                {
                    watch.Stop();
                    string threadName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine("Thread: {0} zaporedna {1}, hash {2}", threadName, i, hash);
                    Console.WriteLine("Thread: {0} ElapsedMs: {1}\n", threadName, elapsedMs);
                    break;
                }
            }
        }

        private static void Method3()
        {
            var watch1 = System.Diagnostics.Stopwatch.StartNew();
            for (int i = steviloPonovitev; i > 0; i--)
            {
                string hash = CalculateHash(i.ToString());

                if (hash.StartsWith(steviloNicel))
                {
                    watch1.Stop();
                    string threadName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    var elapsedMs = watch1.ElapsedMilliseconds;
                    Console.WriteLine("Thread: {0} zaporedna {1}, hash {2}", threadName, i, hash);
                    Console.WriteLine("Thread: {0} ElapsedMs: {1}\n", threadName, elapsedMs);
                    break;
                }
            }
        }

        /// <summary>
        /// Izračunam hash
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string CalculateHash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // ToString("x2") vrne hexadecimalno predstavitev byta, npr 13 vrne "0d"
                    // npr ToString("x") bi vrnil samo d
                    // x2 pomeni, da vrne 2 znaka, na začetku doda 0, če je potrebno
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}