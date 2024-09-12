namespace Test04_Worker
{
    public interface IContext : IDisposable
    {
        void Increase();
    }
}