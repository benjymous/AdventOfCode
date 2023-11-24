namespace AoC.Utils
{
    public static class IterationExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(int x, int y)> Iterate(this (int minx, int miny, int maxx, int maxy) dimensions)
        {
            for (int y = dimensions.miny; y <= dimensions.maxy; ++y)
                for (int x = dimensions.minx; x <= dimensions.maxx; ++x)
                    yield return (x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(int x, int y, int z, int w)> Iterate(this (int minx, int miny, int minz, int minw, int maxx, int maxy, int maxz, int maxw) dimensions)
        {
            for (int w = dimensions.minw; w <= dimensions.maxw; ++w)
                for (int z = dimensions.minz; z <= dimensions.maxz; ++z)
                    for (int y = dimensions.miny; y <= dimensions.maxy; ++y)
                        for (int x = dimensions.minx; x <= dimensions.maxx; ++x)
                            yield return (x, y, z, w);
        }
    }
}
