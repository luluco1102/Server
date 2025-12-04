namespace _1_Lock
{
    internal class Counter
    {
        public int Value => _value;
        
        int _value;
        readonly object _gate = new object();
        
        public void Increment_ThreadUnsafe()
        {
            _value++;
        }

        public void Increment_ThreadSafe()
        {
            // lock : Application 내에서 쓰레드 간 동기화를 하기위한 키워드
            lock (_gate)
            // <Critical Section>
            {
                _value++;
            }
            // </Critical Section>
            
            /*
             * while 루프로 대기 (spinlock) : busy-wait , CPU 가 계속 바쁘게 연산하면서 기다림
             * lock : lock object 를 확인해서 경쟁 상태가 발생하지 않으면 그냥 패스 ,
             * 스케쥴링의 우선순위 뒤로 밀어내기 위해 기다려야 하는 쓰레드로 분류하도록 한다. (쓰레드 잠자는 상태..) 
             */
            
            // Monitor.Enter(_gate);
            //
            // _value++;
            //
            // Monitor.Exit(_gate);
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 100_000;
            Counter counter_threadUnsafe = new Counter();

            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i <  n; i++)
                    counter_threadUnsafe.Increment_ThreadUnsafe();
            });
            Thread t2 = new Thread(() =>
            {
                for (int i = 0; i < n; i++)
                    counter_threadUnsafe.Increment_ThreadUnsafe();
            });
            
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine($"Increment_threadUnsafe 결과 {counter_threadUnsafe.Value}, 기대값 : {n * 2}");
            
            Counter counter_threadsafe = new Counter();

            t1 = new Thread(() =>
            {
                for (int i = 0; i <  n; i++)
                    counter_threadsafe.Increment_ThreadSafe();
            });
            t2 = new Thread(() =>
            {
                for (int i = 0; i < n; i++)
                    counter_threadsafe.Increment_ThreadSafe();
            });
            
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine($"Increment_threadsafe 결과 {counter_threadsafe.Value}, 기대값 : {n * 2}");
        }
    }
}


