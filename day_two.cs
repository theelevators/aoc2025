#:project utils

using Utils;

var line = Utilities.GetInput()[0];
var answerOne = line.SolutionOne();
Console.WriteLine($"Solution One: {answerOne}");
var answerTwo = line.SolutionTwo();
Console.WriteLine($"Solution Two: {answerTwo}");



readonly record struct ProductId(long Value)
{
    public static implicit operator long(ProductId value) => value.Value;
    public static implicit operator ProductId(long value) => new(value);
}
readonly record struct ProductRange(ProductId StartingId, ProductId EndingId);
static class Extensions
{
    extension(string line)
    {
        public long SolutionOne() => line.AsRanges().Select(range => range.GetAllProductIds().FindRepetitions(p =>
            p.Value.ToString() is string strId
                && strId.Length % 2 == 0
                && strId[..(strId.Length / 2)] == strId[(strId.Length / 2)..]
            ? p.Value
            : 0
        )).Sum();

        public long SolutionTwo() => line.AsRanges().Select(range => range.GetAllProductIds().FindRepetitions(pId =>
        {
            string strId = pId.Value.ToString();
            var maxWindowSize = strId.Length / 2;
            var isInvalid = false;
            while (maxWindowSize > 0)
            {
                var chunks = strId.Chunk(maxWindowSize);
                var first = new string(chunks.ElementAt(0));
                if (chunks.All(c => new string(c) == first))
                {
                    isInvalid = true;
                    break;
                }

                maxWindowSize--;
            }

            if (isInvalid)
                return pId.Value;
            else
                return 0;
        }
        )).Sum();

        private List<ProductRange> AsRanges()
        {
            List<ProductRange> ranges = [];
            foreach (var strRange in line.Split(','))
            {
                var parsedRange = strRange.Split('-');
                if (!long.TryParse(parsedRange[0], out var firstId)) throw new Exception("Unable to read first id");
                if (!long.TryParse(parsedRange[1], out var lastId)) throw new Exception("Unable to read last id");
                ranges.Add(new ProductRange(firstId, lastId));
            }
            return ranges;
        }
    }

    extension(ProductRange range)
    {
        public IEnumerable<ProductId> GetAllProductIds()
        {
            for (long i = range.StartingId; i <= range.EndingId; i++)
                yield return i;
        }
    }

    extension(IEnumerable<ProductId> productIds)
    {
        public long FindRepetitions(Func<ProductId, long> repetitionFinder)
        {

            long repetitionCount = 0;

            foreach (var productId in productIds)
            {
                repetitionCount += repetitionFinder(productId);
            }

            return repetitionCount;
        }
    }
}