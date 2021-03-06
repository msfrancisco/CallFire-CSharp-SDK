﻿using CallFire_csharp_sdk.Common.DataManagement;
using CallFire_csharp_sdk.Common.Resource.Mappers;

// ReSharper disable once CheckNamespace - This is an extension from API.Soap
namespace CallFire_csharp_sdk.API.Soap
{
    public partial class IvrBroadcastConfig
    {
        public IvrBroadcastConfig()
        {
        }
        
        public IvrBroadcastConfig(CfIvrBroadcastConfig source)
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
            DialplanXml = source.DialplanXml;
        }
    }
}
