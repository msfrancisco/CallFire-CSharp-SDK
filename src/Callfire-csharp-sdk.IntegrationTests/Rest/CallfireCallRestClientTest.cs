﻿using System;
using CallFire_csharp_sdk.API.Rest.Clients;
using CallFire_csharp_sdk.Common.DataManagement;
using CallFire_csharp_sdk.Common.Resource;
using NUnit.Framework;

namespace Callfire_csharp_sdk.IntegrationTests.Rest
{
    [TestFixture]
    public class CallfireCallRestClientTest : CallfireCallClientTest
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Client = new RestCallClient(MockClient.User(), MockClient.Password());

            var localTimeZoneRestriction = new CfLocalTimeZoneRestriction(DateTime.Now, DateTime.Now);
            CfResult[] result = { CfResult.Received };
            CfRetryPhoneType[] phoneTypes = { CfRetryPhoneType.FirstNumber };
            var broadcastConfigRestryConfig = new CfBroadcastConfigRetryConfig(1000, 2, result, phoneTypes);
            var ivrBroadcastConfig = new CfIvrBroadcastConfig(1, DateTime.Now, "14252163710", localTimeZoneRestriction, broadcastConfigRestryConfig,
                "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.</play></dialplan>");

            var toNumber = new[] { new CfToNumber("Data", null, "14252163710") };
            SendCall = new CfSendCall(String.Empty, CfBroadcastType.Ivr, "broadcastSoap", toNumber, false, ivrBroadcastConfig);

            ActionQuery = new CfActionQuery(100, 0, 1836940001, 1092170001, new[] { CfActionState.Finished }, new[] { CfResult.La },
                false, new DateTime(2014, 1, 1), new DateTime(2014, 12, 1), "12132609784", "14252163710", string.Empty);

            QuerySoundMeta = new CfQuery();
        }
    }
}
