namespace SpaceBattle.Lib;
using Hwdtech;

public class BuildingDecisionTrees : ICommand
{
    private string path;
    public BuildingDecisionTrees(string path)
    {
        this.path = path;
    }

    public void Execute()
    {
        var strategy = IoC.Resolve<Dictionary<int, object>>("SpaceBattle.GetDecisionTrees");
        try
        {
            using (StreamReader reader = File.OpenText(path))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var record = line.Split().Select(int.Parse).ToList();
                    PutInTrees(record, strategy);
                }
            }
        }
        catch (FileNotFoundException e)
        {
            throw new FileNotFoundException(e.ToString());
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }

    }

    private void PutInTrees(List<int> list, IDictionary<int, object> root)
    {
        var trees = root;
        foreach (var item in list)
        {
            trees.TryAdd(item, new Dictionary<int, object>());
            trees = (Dictionary<int, object>)trees[item];
        }
    }
}
