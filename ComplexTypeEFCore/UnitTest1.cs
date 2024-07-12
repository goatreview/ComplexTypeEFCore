using ComplexTypeEFCore.Persistence;

namespace ComplexTypeEFCore;

public class UnitTest1
{
    [Fact]
    public void Test_UsingOfComplexType()
    {
        var context = new GoatDbCtx();
        context.Set<Goat>().Add(new Goat
        {
            Name = "Terry",
            Age = 18,
            Field = new Field
            {
                Street = "Biggest street",
                City = "Lyon",
                // Size = 158
            }
        });
        context.SaveChanges();

        var goats = context.Set<Goat>().ToList();
    }
    
    [Fact]
    public void Test_UsingOfJsonColumn()
    {
        var context = new GoatDbCtx();
        context.Set<GoatJson>().Add(new GoatJson
        {
            Name = "Terry",
            Age = 18,
            Field = new FieldJson
            {
                Street = "Biggest street",
                City = "Lyon",
                // Size = 158
            }
        });
        context.SaveChanges();

        var goats = context.Set<GoatJson>().ToList();
    }
    
    [Fact]
    public void Test_RetrieveAfterRemovingColumn()
    {
        var context = new GoatDbCtx();
        var goats = context.Set<Goat>().ToList();
        var goatsjson = context.Set<GoatJson>().ToList();
    }
}
