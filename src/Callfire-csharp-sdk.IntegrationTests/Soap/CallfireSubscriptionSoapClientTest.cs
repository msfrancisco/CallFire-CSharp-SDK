﻿using CallFire_csharp_sdk.API.Soap;
using CallFire_csharp_sdk.Common.DataManagement;
using CallFire_csharp_sdk.Common.Resource;
using NUnit.Framework;

namespace Callfire_csharp_sdk.IntegrationTests.Soap
{
    [TestFixture]
    public class CallfireSubscriptionSoapClientTest : CallfireSubscriptionClientTest
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Client = new SoapSubscriptionClient(MockClient.User(), MockClient.Password());

            var subscriptionFilter = new CfSubscriptionSubscriptionFilter(1, 5, "fromNumber", "toNumber", true);
            CfSubscription = new CfSubscription(1, true, "endPoint", CfNotificationFormat.Soap, 
                CfSubscriptionTriggerEvent.UndefinedEvent, subscriptionFilter);
            CfSubscriptionRequest = new CfSubscriptionRequest("", CfSubscription);

            QuerySubscription = new CfQuery();

            var subscription = new CfSubscription(140553001, true, "endPoint", CfNotificationFormat.Soap, 
                CfSubscriptionTriggerEvent.CampaignStarted, subscriptionFilter);
            CfUpdateSubscription = new CfSubscriptionRequest("", subscription);
        }
    }
}
