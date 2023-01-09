namespace SpaceBattle.Lib.Test;

public class AngleTest
{
    [Fact]
    public void TestPrintAngle()
    {
        var a = new Angle(60, 1);

        Assert.Equal("60", a.ToString());
    }

    [Fact]
    public void TestNegativeZeroDenominatorAngle()
    {
        Assert.Throws<Exception>(() => new Angle(30, 0));
    }

    [Fact]
    public void TestPositiveAddAngle()
    {
        var a1 = new Angle(30, 1);
        var a2 = new Angle(60, 1);

        Assert.Equal(new Angle(90, 1), a1 + a2);
    }

    [Fact]
    public void TestPositiveEqualAngle()
    {
        var a1 = new Angle(30, 1);
        var a2 = new Angle(60, 2);

        Assert.True(a1 == a2);
    }
    
    [Fact]
    public void TestNegativeEqualAngle()
    {
        var a1 = new Angle(45, 1);
        var a2 = new Angle(120, 2);

        Assert.False(a1 == a2);
    }

    [Fact]
    public void TestPositiveNotEqualAngle()
    {
        var a1 = new Angle(30, 1);
        var a2 = new Angle(120, 2);

        Assert.True(a1 != a2);
    }

       [Fact]
    public void TestNegativeNotEqualAngle()
    {
        var a1 = new Angle(30, 1);
        var a2 = new Angle(180, 3);

        Assert.True(a1 != a2);
    }

    [Fact]
    public void TestPositiveGetHashCodeAngle()
    {
        var a1 = new Angle(90, 3);
        var a2 = new Angle(90, 3);

        Assert.True(a1.GetHashCode() == a2.GetHashCode());
    }

    [Fact]
    public void TestNegativeGetHashCodeAngle()
    {
        var a1 = new Angle(90, 1);
        var a2 = new Angle(60, 1);

        Assert.False(a1.GetHashCode() == a2.GetHashCode());
    }
}
