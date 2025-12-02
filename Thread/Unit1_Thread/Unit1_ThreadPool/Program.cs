using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Unit1_ThreadPool
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();

            
            // workThreads : CPU-Bound thread 개수
            // completionPortThreads : I/O-Bound thread 개수
            ThreadPool.SetMinThreads(4, 4);
            ThreadPool.SetMaxThreads(4, 8);

            // Threadpool 의 이용가능한 스레드가 확보될 때
            // 해당 쓰레드에 작업을 할당할 대기열에 작업을 등록
            ThreadPool.QueueUserWorkItem(_ =>
            {
                
            }
            );
        }

        public static class Workload
        {
            public static void Compute()
            {
                int limit = 100_000;
                long result = 0;

                for (int i = 1; i < limit; i++)
                {
                    result += i;
                }
            }
        }
        [SimpleJob]
        public class Benchmarks
        {
            [Benchmark] public void Workload_Thread() => Program.Workload.Compute();
            [Benchmark] public void Workload_ThreadPool() => ThreadPool.QueueUserWorkItem(_ => Program.Workload.Compute());
        }
       
    }
   
}
