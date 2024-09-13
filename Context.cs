using Microsoft.EntityFrameworkCore;

namespace TransientWorkerStudies;

public class Data
{
    public int Value { get; set; }
    public int Id { get; init; }
}

public sealed class Context : DbContext, IContext
{
    private readonly Workertype _type;
    private int _value;
    private DbSet<Data> Datas { get; set; } = null!;

    public Context(Workertype type)
    {
        _type = type;
        Database.EnsureCreated();
        Console.WriteLine($"{type.Name}: 1. created new context.");
    }

    public void Increase()
    {
        var data = Datas.FirstOrDefault(x => x.Id == _type.Id);
        if (data is null)
        {
            data = new Data { Id = _type.Id };
            Datas.Add(data);
        }
        data.Value++;
        _value = data.Value;
        SaveChanges();
        Console.WriteLine($"{_type.Name}: 2. set value {_value}");
    }

    public override void Dispose()
    {
        Console.WriteLine($"{_type.Name}: 3. disposed. (Value: {_value})");
        base.Dispose();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=db.db");
        base.OnConfiguring(optionsBuilder);
    }
}
public interface IContext : IDisposable
{
    void Increase();
}