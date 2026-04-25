using BracketsAnalysis;

namespace BracketsValidator;

public class BracketsValidatorTests
{
    [Fact]
    public void EmptyString_Valid()
    {
        string input = "";
        bool result = BracketsAnalyzer.IsValid(input);
        Assert.True(result);
    }

    [Fact]
    public void SimpleBrackets_Valid()
    {
        Assert.True(BracketsAnalyzer.IsValid("()"));
    }

    [Fact]
    public void DiffrientBracketsInRow_Valid()
    {
        Assert.True(BracketsAnalyzer.IsValid("()[]{}"));
    }

    [Fact]
    public void MixedOrderBrackets_Valid()
    {
        Assert.True(BracketsAnalyzer.IsValid("([)]"));
    }

    [Fact]
    public void NestedBrackets_Valid()
    {
        Assert.True(BracketsAnalyzer.IsValid("{{{()}}}"));
    }

    [Fact]
    public void ClosingBeforeOpening_InValid()
    {
        Assert.False(BracketsAnalyzer.IsValid(")("));
    }

    [Fact]
    public void WrongClosing_InValid()
    {
        Assert.False(BracketsAnalyzer.IsValid("([{)]"));
    }

    [Fact]
    public void NestedBrackets_InValid()
    {
        Assert.False(BracketsAnalyzer.IsValid("{{{()}})"));
    }
}