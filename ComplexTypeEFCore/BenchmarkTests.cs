using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using ComplexTypeEFCore.Persistence;
using Xunit.Abstractions;

namespace ComplexTypeEFCore;

public class BenchmarkTests(ITestOutputHelper output)
{
    private readonly ManualConfig _config = DefaultConfig.Instance
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddLogger(new ConsoleLogger());

    [Fact]
    public void RunGoatReadBenchmark()
    {
        var summary = BenchmarkRunner.Run<GoatReadBenchmark>(_config);
        output.WriteLine(summary.ToString());
    }
    
    [Fact]
    public void RunGoatWriteBenchmark()
    {
        var summary = BenchmarkRunner.Run<GoatWriteBenchmark>(_config);
        output.WriteLine(summary.ToString());
    }
}


[MemoryDiagnoser]
public class GoatWriteBenchmark
{
    private GoatDbCtx _context;
    private List<Goat> _largeBatch;
    private List<GoatJson> _largeBatchJson;

    [GlobalSetup]
    public void Setup()
    {
        _context = new GoatDbCtx();
        _largeBatch = Enumerable.Range(1, 1000000).Select((_,idx) => new Goat()
        {
            Name = $"Terry{idx}",
            Age = idx,
            Field = new Field
            {
                Street = $"Biggest street {idx}",
                City = "Lyon",
                Hectare = idx
            }
        }).ToList();
        
        _largeBatchJson = Enumerable.Range(1, 1000000).Select((_,idx) => new GoatJson()
        {
            Name = $"Terry{idx}",
            Age = idx,
            Field = new FieldJson
            {
                Street = $"Biggest street {idx}",
                City = "Lyon",
                Hectare = idx
            }
        }).ToList();
    }
    
    [Benchmark]
    public void Write1MGoats()
    {
        using var transaction = _context.Database.BeginTransaction();
        _context.AddRange(_largeBatch);
        _context.SaveChanges();
        transaction.Rollback();
    }
    
    [Benchmark]
    public void Write1MGoatsJson()
    {
        using var transaction = _context.Database.BeginTransaction();
        _context.AddRange(_largeBatchJson);
        _context.SaveChanges();
        transaction.Rollback();
    }
    
    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }
}


[MemoryDiagnoser]
public class GoatReadBenchmark
{
    private GoatDbCtx _context;

    [GlobalSetup]
    public void Setup()
    {
        _context = new GoatDbCtx();
    }

    [Benchmark]
    public void QueryAllGoats()
    {
        var goats = _context.Set<Goat>().ToList();
    }
    
    [Benchmark]
    public void QueryAllGoatsJson()
    {
        var goats = _context.Set<GoatJson>().ToList();
    }
    
    [Benchmark]
    public void QueryGoatsWithWhereCondition()
    {
        var goats = _context.Set<Goat>().Where(g => g.Age % 10 == 0).ToList();
    }
    
    [Benchmark]
    public void QueryGoatsJsonWithWhereCondition()
    {
        var goats = _context.Set<Goat>().Where(g => g.Age % 10 == 0).ToList();
    }

    [Benchmark]
    public void QueryAllGoatsWithOnlyOneProperty()
    {
        var goats = _context.Set<Goat>().Select(g => new
        {
            g.Id,
            g.Age,
            g.Name,
            g.Field.Street
        }).ToList();
    }
    
    [Benchmark]
    public void QueryAllGoatsJsonWithOnlyOneProperty()
    {
        var goats = _context.Set<GoatJson>().Select(g => new
        {
            g.Id,
            g.Age,
            g.Name,
            g.Field.Street
        }).ToList();
    }
    
    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }
}

public class FillDatabaseTests
{
    [Fact]
    public void FillDatabaseWithValues()
    {
        var context = new GoatDbCtx();
        var goats = Enumerable.Range(1, 1000000).Select((_,idx) => new Goat()
        {
            Name = $"Terry{idx}",
            Age = idx,
            Field = new Field
            {
                Street = $"Biggest street {idx}",
                City = "Lyon",
                Hectare = idx
            }
        });
        context.AddRange(goats);
        var goatsJson = Enumerable.Range(1, 1000000).Select((_,idx) => new GoatJson()
        {
            Name = $"Terry{idx}",
            Age = idx,
            Field = new FieldJson
            {
                Street = $"Biggest street {idx}",
                City = "Lyon",
                Hectare = idx
            }
        });
        context.AddRange(goatsJson);
        context.SaveChanges();
    }
}
