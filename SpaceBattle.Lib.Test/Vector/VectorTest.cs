namespace SpaceBattle.Lib.Test;

public class VectorTest
{
    [Fact]
    public void TestPrintVector()
    {
        var v = new Vector(3, 5);

        Assert.Equal("(3, 5)", v.ToString());
    }

    [Fact]
    public void TestGetIndexVector()
    {
        var v = new Vector(3, 5, 8);

        Assert.True(v[2] == 8);
    }

    [Fact]
    public void TestSetIndexVector()
    {
        var v = new Vector(3, 5);
        v[1] = 8;

        Assert.True(v[1] == 8);
    }

    [Fact]
    public void TestPositiveAddVector()
    {
        var v1 = new Vector(3, 5);
        var v2 = new Vector(0, 8);

        Assert.Equal(new Vector(3, 13), v1 + v2);
    }

    [Fact]
    public void TestNegativeAddVector()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(5, 8);

        var action = () => v1 + v2;

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void TestPositiveSubVector()
    {
        var v1 = new Vector(3, 12);
        var v2 = new Vector(0, 8);

        Assert.Equal(new Vector(3, 4), v1 - v2);
    }

    [Fact]
    public void TestNegativeSubVector()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(5, 8);

        var action = () => v1 - v2;

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void TestPositiveEqualsVector()
    {
        var v1 = new Vector(5, 8);
        var v2 = new Vector(5, 8);

        Assert.True(v1 == v2);
    }

    [Fact]
    public void TestNegativeEqualsVector()
    {
        var v1 = new Vector(3, 5);
        var v2 = new Vector(5, 8);

        Assert.False(v1 == v2);
    }

    [Fact]
    public void TestPositiveNotEqualsVector()
    {
        var v1 = new Vector(3, 5);
        var v2 = new Vector(5, 8);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void TestNegativeNotEqualsVector1()
    {
        var v1 = new Vector(3, 5);
        var v2 = new Vector(3, 5);

        Assert.False(v1 != v2);
    }

    [Fact]
    public void TestNegativeNotEqualsVector2()
    {
        var v1 = new Vector(3, 5, 8);
        var v2 = new Vector(5, 8);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void TestPositiveGetHashCodeVector()
    {
        var v1 = new Vector(3, 5, 8);
        var v2 = new Vector(3, 5, 8);

        Assert.True(v1.GetHashCode() == v2.GetHashCode());
    }

    [Fact]
    public void TestNegativeGetHashCodeVector()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(3, 5, 8);

        Assert.True(v1.GetHashCode() != v2.GetHashCode());
    }
}