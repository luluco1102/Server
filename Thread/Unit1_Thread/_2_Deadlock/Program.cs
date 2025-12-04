namespace _2_Deadlock
{
    internal class Program
    {
        static readonly object s_gate1 = new object();
        static readonly object s_gate2 = new object();
        
        
        static void Main(string[] args)
        {
            Thread t1 = new Thread(() => Work(s_gate1, s_gate2));
            Thread t2 = new Thread(() => Work(s_gate2, s_gate1));
            
            t1.Start();
            t2.Start();
            
            Thread.Sleep(500);
        }

        static void Work(object gate1 , object gate2)
        {
            lock (gate1)
            {
                Thread.Sleep(100);
                
                lock (gate2)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}작업 완료.");
                }
            }
        }
    }
}


