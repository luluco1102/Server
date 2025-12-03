namespace _AsyncQuiz
{
    public class Cook
    {
        internal async Task<EggFried> FryEgg()
        {
            EggFried eggFried = new EggFried();
            Console.WriteLine("달걀 굽기 시작");
            await Task.Delay(1000);
            Console.WriteLine("달걀 굽기 완료");
            return eggFried;
        }
        internal async Task<BaconFried> FryBacon()
        {
            BaconFried baconFried = new BaconFried();
            Console.WriteLine("베이컨 굽기 시작");
            await Task.Delay(1000);
            Console.WriteLine("베이컨 굽기 완료");
            return baconFried;
        }
        
        internal async Task<Toast> MakeToast()
        {
            Toast toast = new Toast();
            Console.WriteLine("토스트 만들기 시작");
            await Task.Delay(500);
            Console.WriteLine("토스트 만들기 완료");
            return toast;
        }

        internal async Task<Toast> JamOnToast(Toast toast)
        {
            Console.WriteLine("토스트에 잼 바르기 시작");
            await Task.Delay(500);
            toast.isJamOnIt = true;
            Console.WriteLine("토스트에 잼 바르기 완료");
            return toast;
        }
        
        internal class EggFried{}
    
        internal class BaconFried{}

        internal class Toast()
        {
            internal bool isJamOnIt;
        }
    }
}

