using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.ArticleManager.IGrains;

namespace Newbe.Claptrap.ArticleManager.Grains
{
    public class UpdateArticleEventHandler : IEventHandler
    {
        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public Task<IState> HandleEvent(IEventContext eventContext)
        {
            var eventData = (UpdateArticleEventData) eventContext.Event.Data;
            var stateData = (ArticleStateData) eventContext.State.Data;
            stateData.ArticleHistories ??= new List<ArticleData>();

            if (stateData.Current != null)
            {
                stateData.ArticleHistories.Add(stateData.Current);
            }

            stateData.Current = eventData.ArticleData;
            stateData.Current.CreateTime = DateTime.Now;
            return Task.FromResult(eventContext.State);
        }
    }
}