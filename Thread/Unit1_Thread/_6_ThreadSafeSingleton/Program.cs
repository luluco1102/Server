namespace _6_ThreadSafeSingleton
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                GameManager.Instance.StartGame();
            }).Start();

            new Thread(() =>
            {
                GameManager.Instance.StartGame();
            }).Start();

            new Thread(() =>
            {
                GameManager.Instance.StartGame();
            }).Start();

            new Thread(() =>
            {
                GameManager.Instance.StartGame();
            }).Start();
        }
    }
}
