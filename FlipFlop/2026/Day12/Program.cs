// #define Sample

using System.Text.RegularExpressions;

{
#if Sample
    string fileName = @"D:\Dropbox\Work\AdventOfCode\FlipFlop\2026\Day12\Sample.txt";
#else
    string fileName = @"D:\Dropbox\Work\AdventOfCode\FlipFlop\2026\Day12\Full.txt";
#endif

    var lines = File.ReadAllLines(fileName);

    var block1 = lines.TakeWhile(s => s != "").ToArray();
    var block2 = lines.SkipWhile(s => s != "").Skip(1).ToArray();

    Console.WriteLine("Part 1: " + Part1(block1, block2));
    Console.WriteLine("Part 2: " + Part2());
    Console.WriteLine("Part 3: " + Part3());

    Console.ReadLine();
}

long Part1(string[] block1, string[] block2)
{
    int[] numbersToCall = Tools.ParseIntegers(string.Join(" ", block1));
    BingoCard[] cards = block2.Select(s => new BingoCard(s)).ToArray();

    int bingoCount = 0;

    foreach((var index, var number) in numbersToCall.Index())
    {
        bool bingo = false;

        foreach (var card in cards)
        {
            bool cardBingo = card.CallNumber(number); 
            bingo = bingo || cardBingo; 
        }

        if (bingo)
        {
            bingoCount++;
        }

        if (bingoCount >= 5) return number;
    }

    return 0;
}

long Part2()
{
    return 0;
}
long Part3()
{
    return 0;
}

class BingoCard
{
    private int[,] card;

    public int BingoCount { get; private set; } = 0;

    public BingoCard(string text)
    {
        card = new int[5, 5];

        var numbers = Tools.ParseIntegers(text);

        foreach (var y in Enumerable.Range(0, 5))
        {
            foreach (var x in Enumerable.Range(0, 5))
            {
                card[x, y] = numbers[y * 5 + x];
            }
        }
    }

    public bool CallNumber(int number)
    {
        bool bingo = false;

        foreach (var y in Enumerable.Range(0, 5))
        {
            foreach (var x in Enumerable.Range(0, 5))
            {
                if (card[x, y] == number && card[x, y] >= 0)
                {
                    card[x, y] = -1;
                    bingo = CheckPotentialBingo(x, y);
                }
            }
        }

        return bingo;
    }

    private bool CheckPotentialBingo(int x, int y)
    {
        var bingo = false;

        bingo = bingo || CheckBingoOnRow(y);
        bingo = bingo || CheckBingoOnColumn(x);

        if (x == y) 
        {
            bingo = bingo || CheckBingoOnDownwardDiagonal();
        }

        if (x == 4 - y)
        {
            bingo = bingo || CheckBingoOnUpwardDiagonal();
        }

        if(bingo) BingoCount++;

        return bingo;
    }

    private bool CheckBingoOnUpwardDiagonal()
    {
        if (Enumerable.Range(0, 5).All(c => card[c, 4 - c] < 0))
        {
            return true;
        }

        return false;
    }

    private bool CheckBingoOnDownwardDiagonal()
    {
        if (Enumerable.Range(0, 5).All(c => card[c, c] < 0))
        {
            return true;
        }

        return false;
    }

    private bool CheckBingoOnColumn(int x)
    {
        if (Enumerable.Range(0, 5).All(y => card[x, y] < 0))
        {
            return true;
        }

        return false;
    }

    private bool CheckBingoOnRow(int y)
    {
        if (Enumerable.Range(0, 5).All(x => card[x, y] < 0))
        {
            return true;
        }

        return false;
    }
}


record class Point(int X, int Y)
{
    public Point GetNeightboringPoint(Direction direction)
    {
        return direction switch
        {
            Direction.South => this with { Y = this.Y + 1 },
            Direction.West => this with { X = this.X - 1 },
            Direction.North => this with { Y = this.Y - 1 },
            Direction.East => this with { X = this.X + 1 },
            _ => throw new ArgumentException("Unknown direction", nameof(direction)),
        };
    }

    public enum Direction
    {
        None = '\0',
        South = 'v',
        West = '<',
        North = '^',
        East = '>'
    }
}

public static class Tools
{
    public static int[] ParseIntegers(string text)
    {
        return Regex.Matches(text, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
    }
}

// 298 wrong
// 304 wrong