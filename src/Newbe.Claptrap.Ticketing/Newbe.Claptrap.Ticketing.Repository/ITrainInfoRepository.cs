﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public interface ITrainInfoRepository
    {
        Task<int[]> GetStationsAsync(int trainId);
        Task<int[]> GetTrainsAsync(int fromStationId, int toStationId);
        Task<TrainBasicInfo> GetTrainInfoAsync(int trainId);
        Task<IEnumerable<TrainBasicInfo>> GetAllTrainInfoAsync();
    }
}