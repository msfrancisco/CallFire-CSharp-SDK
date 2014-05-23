﻿using CallFire_csharp_sdk.Common.DataManagement;
using CallFire_csharp_sdk.Common.Resource;
using CallFire_csharp_sdk.Common.Resource.Mappers;
using CallFire_csharp_sdk.Common.Result;
using CallFire_csharp_sdk.Common.Result.Mappers;

namespace CallFire_csharp_sdk.API.Soap
{
    public class SoapBroadcastClient : BaseSoapClient<IBroadcastClient>, IBroadcastClient
    {
        public SoapBroadcastClient(string username, string password)
            : base(username, password)
        {
        }

        internal SoapBroadcastClient(IBroadcastServicePortTypeClient client) : base(client)
        {
        }

        public long CreateBroadcast(CfBroadcastRequest createBroadcast)
        {
            return BroadcastService.CreateBroadcast(new BroadcastRequest(createBroadcast.RequestId, BroadcastMapper.ToSoapBroadcast(createBroadcast.Broadcast)));
        }

        public CfBroadcastQueryResult QueryBroadcasts(CfQueryBroadcasts queryBroadcasts)
        {
            var type = EnumeratedMapper.ToSoapEnumerated(queryBroadcasts.Type);
            var running = queryBroadcasts.Running.HasValue && queryBroadcasts.Running.Value;
            return BroadcastQueryResultMapper.FromSoapBroadcastQueryResult(
                BroadcastService.QueryBroadcasts(new QueryBroadcasts(queryBroadcasts.MaxResults,
                    queryBroadcasts.FirstResult, type, running, queryBroadcasts.LabelName)));
        }

        public CfBroadcast GetBroadcast(long id)
        {
            return BroadcastMapper.FromSoapBroadCast(BroadcastService.GetBroadcast(new IdRequest(id)));
        }

        public void UpdateBroadcast(CfBroadcastRequest updateBroadcast)
        {
            BroadcastService.UpdateBroadcast(new BroadcastRequest(updateBroadcast.RequestId, BroadcastMapper.ToSoapBroadcast(updateBroadcast.Broadcast)));
        }

        public CfBroadcastStats GetBroadcastStats(CfGetBroadcastStats getBroadcastStats)
        {
            return BroadcastStatsMapper.FromSoapBroadcastStats(BroadcastService.GetBroadcastStats(new GetBroadcastStats(getBroadcastStats.Id,
                        getBroadcastStats.IntervalBegin, getBroadcastStats.IntervalEnd)));
        }

        public void ControlBroadcast(CfControlBroadcast controlBroadcast)
        {
            BroadcastService.ControlBroadcast(new ControlBroadcast(controlBroadcast.Id, controlBroadcast.RequestId,
                BroadcastCommandMapper.ToSoapContactBatch(controlBroadcast.Command), controlBroadcast.MaxActive));
        }

        public long CreateContactBatch(CfCreateContactBatch createContactBatch)
        {
            return
                BroadcastService.CreateContactBatch(new CreateContactBatch(createContactBatch.RequestId,
                    createContactBatch.BroadcastId, createContactBatch.Name, createContactBatch.Items,
                    createContactBatch.ScrubBroadcastDuplicates));
        }

        public CfContactBatchQueryResult QueryContactBatches(CfQueryBroadcastData queryBroadcastData)
        {
            return ContactBatchQueryResultMapper.FromSoapContactBatchQueryResult(
                BroadcastService.QueryContactBatches(new QueryContactBatches(queryBroadcastData.MaxResults,
                    queryBroadcastData.FirstResult, queryBroadcastData.BroadcastId)));
        }

        public CfContactBatch GetContactBatch(long id)
        {
            return ContactBatchMapper.FromSoapContactBatch(BroadcastService.GetContactBatch(new IdRequest(id)));
        }

        public void ControlContactBatch(CfControlContactBatch controlContactBatch)
        {
            BroadcastService.ControlContactBatch(new ControlContactBatch(controlContactBatch.Id,
                controlContactBatch.Name, controlContactBatch.Enabled));
        }

        public long CreateBroadcastSchedule(CfCreateBroadcastSchedule createBroadcastSchedule)
        {
            return
                BroadcastService.CreateBroadcastSchedule(new CreateBroadcastSchedule(createBroadcastSchedule.RequestId,
                    createBroadcastSchedule.BroadcastId,
                    BroadcastScheduleMapper.ToSoapBroadcastSchedule(createBroadcastSchedule.BroadcastSchedule)));
        }

        public CfBroadcastScheduleQueryResult QueryBroadcastSchedule(CfQueryBroadcastData queryBroadcastSchedule)
        {
            return
                BroadcastScheduleQueryResultMapper.FromSoapBroadcastScheduleQueryResult(
                    BroadcastService.QueryBroadcastSchedule(
                        new QueryBroadcastSchedules(queryBroadcastSchedule.MaxResults,
                            queryBroadcastSchedule.FirstResult, queryBroadcastSchedule.BroadcastId)));
        }

        public CfBroadcastSchedule GetBroadcastSchedule(long id)
        {
            return BroadcastScheduleMapper.FromSoapBroadcastSchedule(BroadcastService.GetBroadcastSchedule(new IdRequest(id)));
        }

        public void DeleteBroadcastSchedule(long id)
        {
            BroadcastService.DeleteBroadcastSchedule(new IdRequest(id));
        }
    }
}
