namespace AkkaPlayground
{
    public sealed class ReadTemperature
    {
        public ReadTemperature(long reguestId)
        {
            ReguestId = reguestId;
        }

        public long ReguestId { get; }
    }

}