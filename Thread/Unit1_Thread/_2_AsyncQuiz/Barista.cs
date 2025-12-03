namespace _AsyncQuiz
{
    public class Barista
    {
        internal async Task<Coffee> PourCoffee()
        {
            Console.WriteLine("커피 따르기 시작");
            Coffee coffee = new Coffee();
            //TODO : 1초 비동기 대기
            await Task.Delay(1000);
            Console.WriteLine("커피 따르기 완료");
            return coffee;
        }

        internal class Coffee{ }
    }

}
