namespace SpaceBattle.Lib;

public class Angle
{
    private int numerator, denominator;
    public Angle(int n, int d)
    {
        if (d == 0)
        {
            throw new Exception();
        }
        numerator = n;
        denominator = d;
    }

    public override string ToString()
    {
        return $"{numerator / denominator}";
    }

    public static Angle operator +(Angle a1, Angle a2)
    {
        int num = a1.numerator * a2.denominator + a2.numerator * a1.denominator;
        int den = a1.denominator * a2.denominator;
        int gsd = GCD(num, den);

        return new Angle(num / gsd, den / gsd);
    }

    public static bool operator ==(Angle a1, Angle a2)
    {   
        a1.numerator /= GCD(a1.numerator, a1.denominator);
        a1.denominator /= GCD(a1.numerator, a1.denominator);

        a2.numerator /= GCD(a2.numerator, a2.denominator);
        a2.denominator /= GCD(a2.numerator, a2.denominator);
        
        return a1.Equals(a2);
    }

    public static bool operator !=(Angle a1, Angle a2)
    {
        return !(a1 == a2);
    }

    private static int GCD(int n, int d)
    {
        return Math.Abs(d) == 0 ? Math.Abs(n) : GCD(Math.Abs(d), Math.Abs(n) % Math.Abs(d));
    }

    public override bool Equals(object? obj)
    {
        return obj is Angle a && numerator == a.numerator && denominator == a.denominator;
    }
   
    public override int GetHashCode() 
    {
        return $"{numerator}/{denominator}".GetHashCode();
    }
}
