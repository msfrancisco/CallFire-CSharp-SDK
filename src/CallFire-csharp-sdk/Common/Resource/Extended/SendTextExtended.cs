﻿using CallFire_csharp_sdk.Common.Resource;
using CallFire_csharp_sdk.Common.Resource.Mappers;
// ReSharper disable once CheckNamespace - This is an extension from API.Soap


namespace CallFire_csharp_sdk.API.Soap
{
    public partial class SendText
    {
        public SendText(CfSendText cfSendText)
        {
            RequestId = cfSendText.RequestId;
            Type = EnumeratedMapper.ToSoapEnumerated<BroadcastType>(cfSendText.Type.ToString()).ToString();
            BroadcastName = cfSendText.BroadcastName;
            ToNumber = ToNumberMapper.ToToNumber(cfSendText.ToNumber);
            ScrubBroadcastDuplicates = cfSendText.ScrubBroadcastDuplicates;
            TextBroadcastConfig = TextBroadcastConfigMapper.ToSoapTextBroadcastConfig(cfSendText.TextBroadcastConfig);
            if (cfSendText.BroadcastId.HasValue)
            {
                BroadcastId = cfSendText.BroadcastId.Value;
                BroadcastIdSpecified = true;
            }
            UseDefaultBroadcast = cfSendText.UseDefaultBroadcast;
        }
    }
}
