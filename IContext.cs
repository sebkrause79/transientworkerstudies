namespace TransientWorkerStudies
{
    public interface IContext : IDisposable
    {
        void Increase();
    }
}