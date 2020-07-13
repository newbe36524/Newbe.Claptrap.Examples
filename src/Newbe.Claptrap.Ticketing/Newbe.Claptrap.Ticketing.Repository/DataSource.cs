using System.Collections.Generic;
using System.Linq;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public static class DataSource
    {
        public static readonly IReadOnlyDictionary<int, string> LocationNames = new Dictionary<int, string>
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

        public static readonly IReadOnlyDictionary<int, int[]> TrainLocations =
            Enumerable.Range(2, LocationNames.Count - 1)
                .ToDictionary(x => x, x => LocationNames.Keys.Take(x).ToArray());

        public static readonly IReadOnlyDictionary<int, int[]> LocationTrains
            = TrainLocations
                .SelectMany(x => x.Value.Select(locationId => (trainId: x.Key, locationId)))
                .GroupBy(x => x.locationId, x => x.trainId)
                .ToDictionary(x => x.Key, x => x.ToArray());
    }
}