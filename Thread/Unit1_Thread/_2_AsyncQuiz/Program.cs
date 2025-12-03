using static _AsyncQuiz.Cook;

namespace _AsyncQuiz
{
    class Program
    {
        // Main 함수는 async 붙이지 말것
        static void Main(string[] args)
        {
            // 바리스타와 요리사를 생성하여
            // 바리스타가 커피를 다 내리면
            // 요리사가 달걀굽기, 베이컨굽기, 토스트만들기 를 동시 진행.
            // 토스트는 만들어지고 나면 이어서 잼을 발라 완성해야함.
            // 이 과정이 다 끝나면 식사준비 완료
            Barista barista = new Barista();
            Cook cook = new Cook();
            
            Task.Run(barista.PourCoffee);
            barista.PourCoffee().Wait();
            Task<Barista.Coffee> pourCoffeeTask = barista.PourCoffee();
            pourCoffeeTask.Wait();
            
            Task<EggFried> frrEggTask = cook.FryEgg();
            Task<BaconFried> fryBaconTask = cook.FryBacon();
            Task<Toast> toastTask = ToastTask(cook);
            
            // Task<Toast> makeToastTask = cook.MakeToast();
            // makeToastTask.ContinueWith(toast => cook.JamOnToast(toast.Result))
            //     .Wait();
            // frrEggTask.Wait();
            // fryBaconTask.Wait();
            // toastTask.Wait();
            
            Task.WhenAll(frrEggTask, fryBaconTask, toastTask);
            Console.WriteLine("식사준비 완료");
        }

        static async Task<Toast> ToastTask(Cook cook)
        {
            Toast toast = await cook.MakeToast();
            return await cook.JamOnToast(toast);
        }

    }

}

