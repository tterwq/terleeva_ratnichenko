namespace SpaceBattle.Lib;
using System.Collections;

public class GetPositionIterator : IEnumerator
{
    private Vector position;
    private Vector step;
    private int currentObj;
    private IEnumerator enumerator;
    private int count;
    private int i;
    public GetPositionIterator(Vector startPosition, Vector step, IEnumerable<int> queuePositions)
    {
        if (queuePositions.Count() % 2 != 0)
        {
            throw new Exception();
        }
        this.position = startPosition;
        this.step = step;
        this.enumerator = queuePositions.GetEnumerator();
        this.currentObj = 0;
        this.i = 0;
        this.count = queuePositions.Count();
    }
    public object Current => new Vector(position[0], position[1]);

    public bool MoveNext()
    {
        enumerator.MoveNext();
        var n = (int)enumerator.Current;
        if (2 * i == count)
        {
            position[0] -= step[1];
            position[1] -= step[0];
            step[0] = -step[0];
            step[1] = -step[1];
            currentObj++;
        }
        if (currentObj == n)
        {
            i++;
            return true;
        }
        while (n != currentObj)
        {
            position += step;
            currentObj++;
        }
        i++;
        return true;
    }
    public void Reset() => throw new NotImplementedException();
}
