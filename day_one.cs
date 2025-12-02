#:project utils\utils.csproj
using System.Diagnostics.CodeAnalysis;
using Utils;

Dial dial = new(50);
var lines = Utilities.GetInput();
var solutionOneValue = lines.SolutionOne(dial);
var solutionTwoValue = lines.SolutionTwo(dial);
Console.WriteLine($"Solution One Answer: {solutionOneValue}");
Console.WriteLine($"Solution Two Answer: {solutionTwoValue}");

readonly record struct Dial(int Position, int TimesRotated = 0);
readonly record struct Rotation(Direction Direction, int Count);
enum Direction
{
    Left,
    Right
}

static class Extensions
{
    extension(string[] lines)
    {
        public int SolutionTwo(Dial dial)
        {
            var zeroHits = 0;
            foreach(var rotation in lines.AsRotations())
            {
                var previousDial = dial;
                dial = dial.ApplyRotation(rotation);
                if (dial.Position == 0 
                    || (rotation.Direction is Direction.Left && dial.Position > previousDial.Position && previousDial.Position != 0 ) 
                    || (rotation.Direction is Direction.Right && dial.Position < previousDial.Position)
                   ) 
                    zeroHits = ++zeroHits;
            }
            return zeroHits + dial.TimesRotated;
        }


        public int SolutionOne(Dial dial)
        {
            var zeroHits = 0;
            foreach(var rotation in lines.AsRotations())
            {
                dial = dial.ApplyRotation(rotation);
                if (dial.Position == 0) zeroHits++;
            }
            return zeroHits;
        }

        private IEnumerable<Rotation> AsRotations()
        {
            foreach (var line in lines)
            {
                if (!Direction.TryParse(line[0], out var direction)) throw new Exception("Invalid direction input");
                if (!int.TryParse(line[1..], out var count)) throw new Exception("Invalid count input");
                yield return new Rotation((Direction)direction!, count);
            }
        }
    }
    extension(Dial dial)
    {
        public Dial ApplyRotation(Rotation rotation) => rotation.Direction switch
        {
            Direction.Left when dial.Position - (rotation.Count % 100) < 0 => new Dial(100 - Math.Abs(dial.Position - (rotation.Count % 100))) with { TimesRotated = (rotation.Count / 100) + dial.TimesRotated },
            Direction.Left when dial.Position - (rotation.Count % 100) >= 0 => new Dial(dial.Position - (rotation.Count % 100)) with { TimesRotated = (rotation.Count / 100) + dial.TimesRotated},
            Direction.Right when dial.Position + (rotation.Count % 100) > 99 => new Dial(dial.Position + (rotation.Count % 100) - 100) with { TimesRotated = (rotation.Count / 100) + dial.TimesRotated},
            Direction.Right when dial.Position + (rotation.Count % 100) <= 99 => new Dial(dial.Position + (rotation.Count % 100)) with { TimesRotated = (rotation.Count / 100) + dial.TimesRotated},
            _ => dial
        };
    }

    extension(Direction)
    {
        public static bool TryParse(char value, [NotNullWhen(true)] out Direction? direction)
        {
            direction = value switch
            {
                'L' or 'l' => Direction.Left,
                'R' or 'r' => Direction.Right,
                _ => null
            };

            return direction is not null;
        }
    }
}