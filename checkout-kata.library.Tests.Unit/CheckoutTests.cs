using Microsoft.Extensions.Logging;
using Moq;

namespace checkout_kata.library.Tests.Unit;

[TestFixture]
public class CheckoutTests
{
    private Mock<ILogger<Checkout>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<Checkout>>();
    }

    [Test]
    public void Scan_InvalidItemIsScanned_ArgumentExceptionIsThrown()
    {
        //Arrange
        var checkout = new Checkout([], _loggerMock.Object);

        //Act
        var ex = Assert.Throws<ArgumentException>(() => checkout.Scan("X"));

        //Assert
        Assert.That(ex.Message, Is.EqualTo("item X is invalid item. (Parameter 'item')"));
        Assert.That(ex.ParamName, Is.EqualTo("item"));
    }

    [Test]
    public void Scan_EmptyStringIsPassedIn_ArgumentExceptionIsThrown()
    {
        //Arrange
        var checkout = new Checkout([], _loggerMock.Object);

        //Act
        var ex = Assert.Throws<ArgumentException>(() => checkout.Scan(""));

        //Assert
        Assert.That(ex.Message, Is.EqualTo("item can not be empty string. (Parameter 'item')"));
        Assert.That(ex.ParamName, Is.EqualTo("item"));
    }

    [Test]
    public void GetTotalPrice_NothingScanned_ReturnsZeroAndLogsWarning()
    {
        //Arrange
        var checkout = new Checkout([], _loggerMock.Object);

        //Act
        var totalPrice = checkout.GetTotalPrice();

        //Assert
        Assert.That(totalPrice, Is.EqualTo(0));
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No Items Scanned")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
            Times.Once);
    }

    [TestCaseSource(nameof(GetTotalPriceTestCases))]
    public void GetTotalPrice_ItemsAreScanned_CorrectTotalCalculated(List<PricingRule> pricingRules, List<string> items, int expectedTotal)
    {
        //Arrange
        var checkout = new Checkout(pricingRules, _loggerMock.Object);

        //Act
        foreach (var item in items)
            checkout.Scan(item);

        var actualTotal = checkout.GetTotalPrice();

        //Assert
        Assert.That(actualTotal, Is.EqualTo(expectedTotal));
    }

    public static IEnumerable<TestCaseData> GetTotalPriceTestCases()
    {
        yield return new TestCaseData(
            new List<PricingRule>
            {
                new("A", 50, 3, 130),
                new("B", 30, 2, 45),
                new("C", 20, null, null),
                new("D", 15, null, null)
            },
            new List<string> { "A", "A", "C", "B", "B", "A", "D" },
            210
        );
        
        yield return new TestCaseData(
            new List<PricingRule>
            {
                new("A", 50, 3, 130),
                new("B", 30, 2, 45),
                new("C", 20, null, null),
                new("D", 15, null, null)
            },
            new List<string> { "A", "B", "A", "C", "B","D", "B", "A", "D", "D" },
            270
        );

        yield return new TestCaseData(
            new List<PricingRule>
            {
                new("A", 50, 3, 130),
                new("B", 30, 2, 45),
                new("C", 20, null, null),
                new("D", 15, null, null)
            },
            new List<string> { "B", "A", "C" },
            100
        );

        yield return new TestCaseData(
            new List<PricingRule>
            {
                new("E", 60, 3, 135),
            },
            new List<string> { "E", "E", "E", "E" },
            195
        );
    }
}