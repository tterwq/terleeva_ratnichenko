namespace SpaceBattle.Lib;
using System.Collections;


public class GetFuelIterator : IEnumerator
{
    private IEnumerator enumerator;
    public GetFuelIterator(IEnumerable<int> listFuel)
    {
        this.enumerator = listFuel.GetEnumerator();
    }
    public object Current => enumerator.Current;
    public bool MoveNext() => enumerator.MoveNext();
    public void Reset() => throw new NotImplementedException();
}
