using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class DecisionTreesTest
{

    public DecisionTreesTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void PositiveBuildingDecisionTreesTest()
    {
        string path = "../../../Trees.txt";
        Mock<IStrategy> getDecisionTreesStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDecisionTrees", (object[] args) => getDecisionTreesStrategy.Object.Strategy(args)).Execute();
        getDecisionTreesStrategy.Setup(t => t.Strategy(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();

        var bdts = new BuildingDecisionTrees(path);

        bdts.Execute();

        getDecisionTreesStrategy.Verify();
    }

    [Fact]
    public void NegativeBuildingDecisionTreesTestThrowsException()
    {
        string path = "";
        Mock<IStrategy> getDecisionTreesStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDecisionTrees", (object[] args) => getDecisionTreesStrategy.Object.Strategy(args)).Execute();
        getDecisionTreesStrategy.Setup(t => t.Strategy(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();

        var bdts = new BuildingDecisionTrees(path);

        Assert.Throws<Exception>(() => bdts.Execute());

        getDecisionTreesStrategy.Verify();
    }

    [Fact]
    public void NegativeBuildingDecisionTreesTestThrowsFileNotFoundException()
    {
        string path = "./DT_File.txt";
        Mock<IStrategy> getDecisionTreesStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetDecisionTrees", (object[] args) => getDecisionTreesStrategy.Object.Strategy(args)).Execute();
        getDecisionTreesStrategy.Setup(t => t.Strategy(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();

        var bdts = new BuildingDecisionTrees(path);

        Assert.Throws<FileNotFoundException>(() => bdts.Execute());

        getDecisionTreesStrategy.Verify();
    }
}
