/*
 * Spinlock
 * 자원점유 모니터링 하는것보다 while 루프로 cpu 를 잡아먹으면서 기다리는게 더 빠를 것 같을때
 * 내부구현은 대충 이런식
 * while (lock taken)
 *  Thread.Sleep(1);
 * 
 * Critical Section 이 아주 빠른 단위 작업이라 busy-wait 하는게 더 낫다면 Spinlock 을 고려해야한다.
 * 
 * ReaderWriterLockSlim
 * 다중읽기 & 단일쓰기.. 상황에서 쓰기를 할때 짧은 Spinlock 을 활용하는.. 
 */
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace _7_Spinlock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //| Method | Mean | Error | StdDev |
            //| ------------------------- | -------------:| ------------:| ------------:|
            //| ComputeWriteWithSpinlock | 732,286.0 ns | 2,698.17 ns | 2,523.87 ns |
            //| ComputeWriteWithLock | 180,456.4 ns | 1,555.15 ns | 1,298.62 ns |
            //| ComputeWriteWithRWSlim | 857.1 ns | 5.00 ns | 4.68 ns |
            //| ComputeReadWithRWSlim | 1,470.0 ns | 2.71 ns | 2.54 ns |
            BenchmarkRunner.Run<Benchmarks>();
        }
    }

    public class Benchmarks
    {
        [Benchmark] 
        public void ComputeWriteWithSpinlock()
        {
            Task[] tasks = new Task[1];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(Workload.WriteWithSpinlock);
            }

            Task.WaitAll(tasks);
        }

        [Benchmark] 
        public void ComputeWriteWithLock()
        {
            Task[] tasks = new Task[1];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(Workload.WriteWithLock);
            }

            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void ComputeWriteWithRWSlim()
        {
            Task[] tasks = new Task[1];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(Workload.WriteWithRWSlim);
            }

            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void ComputeReadWithRWSlim()
        {
            Task[] tasks = new Task[3];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(Workload.ReadWithRWSlim);
            }

            Task.WaitAll(tasks);
        }
    }
    
    public static class Workload
    {
        const int TOTAL = 10_000;
        static SpinLock s_spin = new SpinLock();
        static object s_gate = new object();
        static ReaderWriterLockSlim s_rwSlim = new ReaderWriterLockSlim();
        static int s_rwResult;

        public static void WriteWithSpinlock()
        {
            int result = 0;

            for (int i = 0; i < TOTAL; i++)
            {
                bool taken = false;
                s_spin.Enter(ref taken);
                result++;

                if (taken)
                    s_spin.Exit();
            }
        }

        public static void WriteWithLock()
        {
            int result = 0;

            for (int i = 0; i < TOTAL; i++)
            {
                lock (s_gate)
                {
                    result++;
                }
            }
        }

        public static void WriteWithRWSlim()
        {
            s_rwSlim.EnterWriteLock();
            s_rwResult++;
            s_rwSlim.ExitWriteLock();
        }

        public static void ReadWithRWSlim()
        {
            s_rwSlim.EnterReadLock();
            _ = s_rwResult;
            s_rwSlim.ExitReadLock();
        }
    }
}
