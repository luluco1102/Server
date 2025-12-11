using System.Threading.Channels;

namespace _5_Channel
{
    internal class Program
    {
        const int TOTAL = 20;

        static void Main(string[] args)
        {
            Channel<int> channel = Channel.CreateBounded<int>(10);

            // 생산자 작업
            Task producer = Task.Run(async () =>        // 람다식으로도 비동기함수 정의 가능하다
            {
                for (int i = 0; i < TOTAL; i++)
                {
                    await channel.Writer.WriteAsync(i);
                    Console.WriteLine($"[생산자] : {i} 생산함");
                }

                channel.Writer.Complete();
                Console.WriteLine($"[생산자] : 작업완료");
            });

            // 소비자 작업
            Task consumer = Task.Run(async () =>
            {
                // await foreach : IAsyncEnumerable 순회 구문 (await MoveNextAsync)
                await foreach (int item in channel.Reader.ReadAllAsync())
                {
                    Console.WriteLine($"[소비자] : {item} 소비시작");
                    await Task.Delay(100); // 소비 시간 시뮬레이션
                    Console.WriteLine($"[소비자] : {item} 소비완료");
                }

                Console.WriteLine($"[소비자] : 작업완료");
            });

            Task.WaitAll(producer, consumer);
        }
    }
}
