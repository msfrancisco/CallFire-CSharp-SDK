﻿using System;
using CallFire_csharp_sdk.Common.DataManagement;
using CallFire_csharp_sdk.Common.Resource.Mappers;

// ReSharper disable once CheckNamespace - This is an extension from API.Soap

namespace CallFire_csharp_sdk.API.Soap
{
    public partial class VoiceBroadcastConfig
    {
        public VoiceBroadcastConfig()
        {
        }
        
        public VoiceBroadcastConfig(CfVoiceBroadcastConfig source)
        {
            if (source.Id.HasValue)
            {
                id = source.Id.Value;
                idSpecified = true;
            }
            if (source.Created.HasValue)
            {
                Created = source.Created.Value;
                CreatedSpecified = true;
            }
            FromNumber = source.FromNumber;
            LocalTimeZoneRestriction = LocalTimeZoneRestrictionMapper.ToSoapLocalTimeZoneRestriction(source.LocalTimeZoneRestriction);
            RetryConfig = BroadcastConfigRetryConfigMapper.ToBroadcastConfigRetryConfig(source.RetryConfig);

            if (source.AnsweringMachineConfig.HasValue)
            {
                AnsweringMachineConfig = EnumeratedMapper.ToSoapEnumerated<AnsweringMachineConfig>(source.AnsweringMachineConfig.ToString());
                AnsweringMachineConfigSpecified = true;
            }
            Item = source.Item == null ? null : source.Item is string ? source.Item : Convert.ToInt64(source.Item);
            LiveSoundTextVoice = source.LiveSoundTextVoice;
            Item1 = source.Item1 == null ? null : source.Item1 is string ? source.Item1 : Convert.ToInt64(source.Item1);
            MachineSoundTextVoice = source.MachineSoundTextVoice;
            Item2 = source.Item2 == null ? null : source.Item2 is string ? source.Item2 : Convert.ToInt64(source.Item2);
            TransferSoundTextVoice = source.TransferSoundTextVoice;
            TransferDigit = source.TransferDigit;
            TransferNumber = source.TransferNumber;
            Item3 = source.Item3 == null ? null : source.Item3 is string ? source.Item3 : Convert.ToInt64(source.Item3);
            DncSoundTextVoice = source.DncSoundTextVoice;
            DncDigit = source.DncDigit;
            if (source.MaxActiveTransfers.HasValue)
            {
                MaxActiveTransfers = source.MaxActiveTransfers.Value;
                MaxActiveTransfersSpecified = true;
            }
        }
    }
}
