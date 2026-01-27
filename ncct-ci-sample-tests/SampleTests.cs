using System.Collections.Generic;
using Xunit;

namespace Ncct.Blazor.Sample.Tests;

public class BasicTests
{
    [Fact]
    public void SimpleTest()
    {
        var result = 2 + 2;
        Assert.Equal(4, result);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 5)]
    public void DataTests(int a, int b, int expected)
    {
        var result = a + b;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("test", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void StringValidationTests(string? input, bool expectedResult)
    {
        var isValid = !string.IsNullOrEmpty(input);
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void ListOperationsTest()
    {
        var list = new List<int> { 1, 2, 3 };
        list.Add(4);
        
        Assert.Equal(4, list.Count);
        Assert.Contains(3, list);
    }
}
