#:project utils

using Utils;
var line = Utilities.GetInput()[0];
var answerOne = line.SolutionOne();
var answerTwo = line.SolutionTwo();
//Console.WriteLine(answerOne);
Console.WriteLine(answerTwo);



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

        public long SolutionTwo()
        {
            long productIdSum = 0;
            foreach (var range in line.AsRanges())
            {
                foreach (var productId in range.GetAllProductIds())
                {
                    string strId = productId.Value.ToString();

                    var maxWindowSize = strId.Length / 2;
                    Console.WriteLine($"Item: {strId}");
                    var isInvalid = true;

                    while (maxWindowSize > 0)
                    {
                        var possibleItemsCount = strId.Length / maxWindowSize;
                        Console.WriteLine($"WindowSize: {maxWindowSize}");
                        //Console.WriteLine($"PossibleItemCounts: {possibleItemsCount}");
                        if (strId.Length - (maxWindowSize * possibleItemsCount) < 0)
                        {
                            isInvalid = false;
                            break;
                        }
                        var first = strId[..maxWindowSize];
                        Console.WriteLine($"First: {first}");
                        for (int offset = 0; offset <= possibleItemsCount; offset++)
                        {
                            var current = offset == 0
                                        ? strId[..maxWindowSize]
                                        : strId[(maxWindowSize * (offset - 1))..(maxWindowSize * offset)];

                            Console.WriteLine($"offset: {offset}");
                            Console.WriteLine($"Current: {current}");
                            if (!first.Equals(current, StringComparison.InvariantCultureIgnoreCase) )
                            {
                                //Console.WriteLine($"Is not equal dawg");
                                isInvalid = false;
                                break;
                            }
                        }
                        
                        if (isInvalid)
                        {
                            Console.WriteLine($"ProductId: {productId} is invalid");
                            productIdSum += productId;
                        }
                        maxWindowSize--;
                    }



                    //2121212121
                    //2121212124
                }
            }
            return productIdSum;
        }

        public long SolutionOne()
        {
            long productIdSum = 0;
            foreach (var range in line.AsRanges())
            {
                foreach (var productId in range.GetAllProductIds())
                {
                    string strId = productId.Value.ToString();

                    if (strId.Length % 2 == 0)
                    {
                        var mid = strId.Length / 2;
                        var firstHalf = strId[..mid];
                        var lastHalf = strId[mid..];
                        //Console.WriteLine(firstHalf);
                        //Console.WriteLine(lastHalf);
                        if (firstHalf == lastHalf)
                        {
                            //Console.WriteLine($"Buffer: {productId.Value} is repeated:");
                            productIdSum += productId;
                        }
                    }
                }
            }
            return productIdSum;
        }

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
}