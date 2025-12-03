// Channel : 생산자의 생산속도와 소비자의 소비속도 차이에 의한 병목현상을 핸들링하는 객체
namespace _4_ChannelPipeline
{
    internal class Program
    {
        static int s_total = 20; // 처리해야하는 아이템 갯수
        static int s_bufferCapacity = 10;
        static Queue<int> s_buffer = new(s_bufferCapacity);
        static bool s_isProduceCompleted;

        static void Main(string[] args)
        {
            Task produce = Task.Run(ProduceAsync);
            Task consume = Task.Run(ConsumeAsync);
            Task.WaitAll(produce, consume);
        }

        static async Task ProduceAsync()
        {
            for (int i = 0; i < s_total; i++)
            {
                await WaitUntilBufferHasSpaceAsync();
                s_buffer.Enqueue(i);
                Console.WriteLine($"[생산자] : 아이템 {i} 생산");
            }

            s_isProduceCompleted = true;
        }

        static async Task ConsumeAsync()
        {
            while (true)
            {
                if (s_buffer.Count > 0)
                {
                    int item = s_buffer.Dequeue();
                    Console.WriteLine($"[소비자] : 아이템 {item} 소비 시작");
                    await Task.Delay(100);
                    Console.WriteLine($"[소비자] : 아이템 {item} 소비 완료");
                }
            }
        }

        static async Task WaitUntilBufferHasSpaceAsync()
        {
            while (true)
            {
                if (s_buffer.Count < s_bufferCapacity)
                    return;

                await Task.Delay(10);
            }
        }
    }
}
