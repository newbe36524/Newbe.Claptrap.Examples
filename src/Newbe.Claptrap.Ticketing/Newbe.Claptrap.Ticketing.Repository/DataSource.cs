using System.Collections.Generic;
using System.Linq;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public static class DataSource
    {
        public static readonly IReadOnlyDictionary<int, string> StationNames = new Dictionary<int, string>
        {
            {10001, "bei jing"},
            {10002, "shang hai"},
            {10003, "tian jin"},
            {10004, "chong qing"},
            {10005, "ha er bin"},
            {10006, "chang chun"},
            {10007, "shen yang"},
            {10008, "hu he hao te"},
            {10009, "shi jia zhuang"},
            {10010, "wu lu mu qi"},
            {10011, "lan zhou"},
            {10012, "xi ning"},
            {10013, "xi an"},
            {10014, "yin chuan"},
            {10015, "zheng zhou"},
            {10016, "ji nan"},
            {10017, "tai yuan"},
            {10018, "he fei"},
            {10019, "wu han"},
            {10020, "chang sha"},
            {10021, "nan jing"},
            {10022, "cheng du"},
            {10023, "gui yang"},
            {10024, "kun ming"},
            {10025, "nan ning"},
            {10026, "la sa"},
            {10027, "hang zhou"},
            {10028, "nan chang"},
            {10029, "guang zhou"},
            {10030, "fu zhou"},
            {10031, "tai bei"},
            {10032, "hai kou"},
            {10033, "xiang gang"},
            {10034, "ao men"},
        };

        public static readonly IReadOnlyDictionary<int, int[]> TrainStations =
            From2ToEnd().Concat(FromEndTo2())
                .ToDictionary(x => x.trainId, x => x.stattionIds);

        private static IEnumerable<(int trainId, int[] stattionIds)> From2ToEnd()
        {
            return Enumerable.Range(2, StationNames.Count - 1)
                .Select((x, i) => (trainId: x, stattionIds: StationNames.Keys.Take(i + 2).ToArray()));
        }

        private static IEnumerable<(int trainId, int[] stattionIds)> FromEndTo2()
        {
            var reversKeys = StationNames.Keys.Reverse().ToArray();
            return Enumerable.Range(102, StationNames.Count - 1)
                .Select((x, i) => (trainId: x, stattionIds: reversKeys.Take(i + 2).ToArray()));
        }

        public static readonly IReadOnlyDictionary<int, int[]> StationTrains
            = TrainStations
                .SelectMany(x => x.Value.Select(stationId => (trainId: x.Key, stationId)))
                .GroupBy(x => x.stationId, x => x.trainId)
                .ToDictionary(x => x.Key, x => x.ToArray());
    }
}