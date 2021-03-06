﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Dapr.Core;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;

namespace Newbe.Claptrap.Ticketing.IActor
{
    [ClaptrapState(typeof(TrainInfo), ClaptrapCodes.TrainActor)]
    [ClaptrapEvent(typeof(UpdateCountEvent), ClaptrapCodes.UpdateCount)]
    public interface ITrainGran : IClaptrapActor
    {
        /// <summary>
        /// get left count
        /// </summary>
        /// <param name="fromStationId"></param>
        /// <param name="toStationId"></param>
        /// <returns></returns>
        Task<int> GetLeftSeatCountAsync(int fromStationId, int toStationId);

        /// <summary>
        /// get left count
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<int, IDictionary<int, int>>> GetAllCountAsync();

        /// <summary>
        /// update count
        /// </summary>
        /// <param name="fromStationId"></param>
        /// <param name="toStationId"></param>
        /// <returns></returns>
        Task UpdateCountAsync(int fromStationId, int toStationId);
    }
}