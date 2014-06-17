using System;
using System.Linq;
using System.Net;
using System.ServiceModel;
using CallFire_csharp_sdk.API;
using CallFire_csharp_sdk.API.Rest.Clients;
using CallFire_csharp_sdk.API.Soap;
using CallFire_csharp_sdk.Common.DataManagement;
using CallFire_csharp_sdk.Common.Resource;
using NUnit.Framework;

namespace Callfire_csharp_sdk.IntegrationTests
{
    [TestFixture]
    public abstract class CallfireBroadcastClientTest
    {
        protected IBroadcastClient Client;
        protected CfBroadcast ExpectedBroadcastDefault;
        protected CfBroadcast ExpectedBroadcast;
        protected CfBroadcast ExpectedBroadcastVoice;
        protected CfBroadcast ExpectedBroadcastText;
        protected CfBroadcast ExpectedBroadcastIvr;
        protected CfQueryBroadcasts CfQueryBroadcasts;
        protected CfQueryBroadcastData QueryContactBatches;
        protected CfControlContactBatch ControlContactBatches;
        protected CfGetBroadcastStats GetBroadcastStats;
        protected CfBroadcastRequest UpdateBroadcast;
        protected CfControlBroadcast ControlBroadcast;
        protected CfCreateContactBatch CreateContactBatch;

        protected const string VerifyFromNumber = "+19196991764";
        protected const string VerifyShortCode = "67076";

        private void AssertClientException<TRest, TSoap>(TestDelegate test) 
            where TRest : Exception 
            where TSoap : Exception
        {
            if (Client.GetType() == typeof(RestBroadcastClient))
            {
                Assert.Throws<TRest>(test);
            }
            else
            {
                Assert.Throws<TSoap>(test);
            }
        }
        
        //Create Broadcast
        [Test]
        public void Test_CreateBroadcast()
        {
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastDefault);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }
        
        [Test]
        public void Test_CreateBroadcastNull()
        {
            var broadcastRequest = new CfBroadcastRequest(string.Empty, null);
            AssertClientException<WebException, FaultException>(() => Client.CreateBroadcast(broadcastRequest));
        }

        [Test]
        public void Test_CreateBroadcast_WithNameOnly()
        {
            ExpectedBroadcast = new CfBroadcast
            {
                Name = "Name"
            };

            var broadcastRequest = new CfBroadcastRequest(null, ExpectedBroadcast);
            AssertClientException<WebException, FaultException>(() => Client.CreateBroadcast(broadcastRequest));
        }

        [Test]
        public void Test_CreateBroadcast_WithNameandTypeVoice()
        {
            ExpectedBroadcast = new CfBroadcast
            {
                Name = "Name", 
                Type = CfBroadcastType.Voice
            };

            var broadcastRequest = new CfBroadcastRequest(null, ExpectedBroadcast);
            AssertClientException<WebException, FaultException>(() => Client.CreateBroadcast(broadcastRequest));
        }

        //Create Voice Broadcast
        [Test]
        public void Test_CreateBroadcast_VoiceBroadcastConfigFaildNumber() 
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = "45879163710",
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee"
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            AssertClientException<WebException, FaultException<ServiceFaultInfo>>(() => Client.CreateBroadcast(broadcastRequest));
        }

        [Test]
        public void Test_CreateBroadcast_VoiceBroadcastConfigComplete() 
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyFromNumber,
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee",
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_VoiceLocalTimeZoneRestrictionComplete() 
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyFromNumber,
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee",
                    LocalTimeZoneRestriction = new CfLocalTimeZoneRestriction
                    {
                        BeginTime = new DateTime(2014, 01, 01, 09, 00, 00),
                        EndTime = new DateTime(2014, 01, 01, 17, 00, 00)
                    },
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_VoiceLocalTimeZoneRestrictionBeginTimeOnly() 
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyFromNumber,
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee",
                    LocalTimeZoneRestriction = new CfLocalTimeZoneRestriction
                    {
                        BeginTime = new DateTime(2014, 01, 01, 09, 00, 00),
                    },
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            if (Client.GetType() == typeof (RestBroadcastClient))
            {
                var id = Client.CreateBroadcast(broadcastRequest);
                Assert.IsNotNull(id);
            }
            else
            {
                Assert.Throws<FaultException<ServiceFaultInfo>>(() => Client.CreateBroadcast(broadcastRequest));
            }
        }

        [Test]
        public void Test_CreateBroadcast_VoiceRetryConfigCompleteItem_Item1_Item2_Item3Long()
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyFromNumber,
                    Item = 426834001,
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = 426834001,
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryResults = new[] { CfResult.La }
                    }
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_VoiceRetryConfigComplete()
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyFromNumber,
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee",
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        MaxAttempts = 2,
                        MinutesBetweenAttempts = 5,
                        RetryPhoneTypes = new[] { CfRetryPhoneType.HomePhone },
                        RetryResults = new[] { CfResult.NoAns }
                    }
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }
       
        [Test]
        public void Test_CreateBroadcast_VoiceRetryConfigNotAllComplete() 
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyFromNumber,
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee",
                    RetryConfig = new CfBroadcastConfigRetryConfig()
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        //Create Text Broadcast
        [Test]
        public void Test_CreateBroadcast_TextConfigSuccess()
        {
            ExpectedBroadcastText = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    FromNumber = VerifyShortCode,
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryPhoneTypes = new[] { CfRetryPhoneType.FirstNumber },
                        RetryResults = new[] { CfResult.NoAns }
                    },
                    Message = "Message Test",
                },
            };

            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastText);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_TextLocalTimeZoneRestrictionEndTimeOnly() 
        {
            ExpectedBroadcastText = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyShortCode,
                    LocalTimeZoneRestriction = new CfLocalTimeZoneRestriction
                    {
                        EndTime = new DateTime(2014, 01, 01, 17, 00, 00)
                    },
                    Message = "Message Test",
                    BigMessageStrategy = CfBigMessageStrategy.DoNotSend
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastText);
            if (Client.GetType() == typeof(RestBroadcastClient))
            {
                var id = Client.CreateBroadcast(broadcastRequest);
                Assert.IsNotNull(id);
            }
            else
            {
                Assert.Throws<FaultException<ServiceFaultInfo>>(() => Client.CreateBroadcast(broadcastRequest));
            }
        }

        [Test]
        public void Test_CreateBroadcast_TextRetryConfigNotAllComplete() 
        {
            ExpectedBroadcastText = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyShortCode,
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryPhoneTypes = new[] { CfRetryPhoneType.FirstNumber },
                        RetryResults = new[] { CfResult.NoAns }
                    },
                    Message = "Message Test",
                    BigMessageStrategy = CfBigMessageStrategy.Trim
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastText);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }
        
        [Test]
        public void Test_CreateBroadcast_TextRetryConfigMessage160caracters()
        {
            ExpectedBroadcastText = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    Id = 1,
                    Created = new DateTime(2012, 10, 26),
                    FromNumber = VerifyShortCode,
                    Message = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adineque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adi",
                    BigMessageStrategy = CfBigMessageStrategy.SendMultiple
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastText);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_TextRetryConfigMessage161caractersANDRetryResultsSENT() 
        {
            ExpectedBroadcastText = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    FromNumber = VerifyShortCode,
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryPhoneTypes = new[] { CfRetryPhoneType.FirstNumber },
                        RetryResults = new[] { CfResult.Sent }
                    },
                    Message = "Xneque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adineque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adi",
                    BigMessageStrategy = CfBigMessageStrategy.DoNotSend
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastText);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        //Create Ivr Broadcast
        [Test]
        public void Test_CreateBroadcast_IvrBroadcastConfigComplete()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.</play></dialplan>"
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_IvrLocalTimeZoneRestrictionComplete()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.</play></dialplan>",
                    LocalTimeZoneRestriction = new CfLocalTimeZoneRestriction
                    {
                        BeginTime = new DateTime(2014, 01, 01, 09, 00, 00),
                        EndTime = new DateTime(2014, 01, 01, 17, 00, 00)
                    }
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_IvrRetryConfigNODialplanXml()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryPhoneTypes = new[] { CfRetryPhoneType.FirstNumber },
                        RetryResults = new[] { CfResult.TooBig }
                    },
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr);
            AssertClientException<WebException, FaultException<ServiceFaultInfo>>(() => Client.CreateBroadcast(broadcastRequest));
        }

        [Test]
        public void Test_CreateBroadcast_IvrRetryConfigVALIDDialplanXml()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.</play></dialplan>",
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryPhoneTypes = new[] { CfRetryPhoneType.WorkPhone },
                        RetryResults = new[] { CfResult.InternalError }
                    },
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_CreateBroadcast_IvrRetryConfigINVALIDDialplanXml()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.",
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        RetryPhoneTypes = new[] { CfRetryPhoneType.WorkPhone },
                        RetryResults = new[] { CfResult.CarrierError }
                    },
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr);
            AssertClientException<WebException, FaultException<ServiceFaultInfo>>(() => Client.CreateBroadcast(broadcastRequest));
        }

        [Test]
        public void Test_CreateBroadcast_IvrRetryConfigNotallComplete()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.</play></dialplan>",
                    RetryConfig = new CfBroadcastConfigRetryConfig()
                },
            };
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr);
            var id = Client.CreateBroadcast(broadcastRequest);
            Assert.IsNotNull(id);
        }

        //QueryBroadcasts
        [Test]
        public void Test_QueryBroadcast()
        {
            var queryResult = Client.QueryBroadcasts(CfQueryBroadcasts);
            Assert.NotNull(queryResult);
            Assert.NotNull(queryResult.Broadcast);
        }

        [Test]
        public void Test_QueryBroadcastsEmpty()
        {
            if (Client.GetType() == typeof(RestBroadcastClient))
            {
                var broadcastQueryResult = Client.QueryBroadcasts(new CfQueryBroadcasts());
                Assert.IsNotNull(broadcastQueryResult);
            }
            else
            {
                Assert.Throws<CommunicationException>(() => Client.QueryBroadcasts(new CfQueryBroadcasts()));
            }
        }

        [Test]
        public void Test_QueryBroadcastsCompleteTrue()
        {
            var cfQueryBroadcasts = new CfQueryBroadcasts
            {
                FirstResult = 1,
                MaxResults = 50,
                Type = new[] {CfBroadcastType.Voice},
                Running = true
            };
            var broadcastQueryResult = Client.QueryBroadcasts(cfQueryBroadcasts);
            Assert.IsNotNull(broadcastQueryResult);
        }

        [Test]
        public void Test_QueryBroadcastsCompleteFalse()
        {
            var cfQueryBroadcasts = new CfQueryBroadcasts
            {
                MaxResults = 50,
                Type = new[] { CfBroadcastType.Text },
                Running = true
            };
            var broadcastQueryResult = Client.QueryBroadcasts(cfQueryBroadcasts);
            Assert.IsNotNull(broadcastQueryResult);
        }

        //GetBroadcast
        [Test]
        public void Test_GetBroadcast()
        {
            var broadcast = Client.GetBroadcast(651);
            Assert.IsNotNull(broadcast);
            Assert.AreEqual(broadcast.Type, CfBroadcastType.Text);
        }

        [Test]
        public void Test_GetBroadcastSuccess()
        {
            var broadcast = Client.GetBroadcast(1903343001);
            Assert.IsNotNull(broadcast);
        }

        //UpdateBroadcast
        [Test]
        public void Test_UpdateBroadcast()
        {
            Client.UpdateBroadcast(UpdateBroadcast);
        }

        [Test]
        public void Test_UpdateBroadcastChangeTypeVoice()
        {
            ExpectedBroadcastVoice = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    Item = "TTS: eeee",
                    MachineSoundTextVoice = "SPANISH1",
                    Item1 = "TTS: eeee",
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        MaxAttempts = 2,
                        MinutesBetweenAttempts = 5,
                        RetryPhoneTypes = new[] { CfRetryPhoneType.HomePhone },
                        RetryResults = new[] { CfResult.NoAns }
                    }
                },
            };
            var id = Client.CreateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcastVoice));

            const string newName = "changeType";
            const long newSoundId = 454556001;
            const int newMaxAttempts = 10;
            ExpectedBroadcast = new CfBroadcast
            {
                Id = id,
                Name = newName,
                Type = CfBroadcastType.Voice,
                Item = new CfVoiceBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    Item = newSoundId,
                    RetryConfig = new CfBroadcastConfigRetryConfig
                    {
                        MaxAttempts = newMaxAttempts,
                        RetryPhoneTypes = new[] { CfRetryPhoneType.MobilePhone },
                    }
                },
            };
            Client.UpdateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcast));
            var broadcast = Client.GetBroadcast(id);

            Assert.AreEqual(newName, broadcast.Name);
            Assert.AreEqual(newSoundId, ((CfVoiceBroadcastConfig)broadcast.Item).Item);
            Assert.AreEqual(newMaxAttempts, broadcast.Item.RetryConfig.MaxAttempts);
            Assert.IsTrue(broadcast.Item.RetryConfig.RetryPhoneTypes.Any(t => t.Equals(CfRetryPhoneType.MobilePhone)));
        }

        [Test]
        public void Test_UpdateBroadcastChangeTypeText()
        {
            ExpectedBroadcastText = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    FromNumber = VerifyShortCode,
                    RetryConfig = new CfBroadcastConfigRetryConfig(),
                    Message = "Message Test",
                    BigMessageStrategy = CfBigMessageStrategy.DoNotSend
                },
            };

            var id = Client.CreateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcastText));

            const string newName = "changeTypeText";
            const string newMessage = "UpdateMessage";
            const CfBigMessageStrategy newBigMessageStrategy = CfBigMessageStrategy.SendMultiple;
           
            ExpectedBroadcast = new CfBroadcast
            {
                Id = id,
                Name = newName,
                Type = CfBroadcastType.Text,
                Item = new CfTextBroadcastConfig
                {
                    FromNumber = VerifyShortCode,
                    Message = newMessage,
                    BigMessageStrategy = newBigMessageStrategy
                },
            };
            Client.UpdateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcast));
            var broadcast = Client.GetBroadcast(id);

            Assert.AreEqual(newName, broadcast.Name);
            Assert.AreEqual(newMessage, ((CfTextBroadcastConfig)broadcast.Item).Message);
            Assert.AreEqual(newBigMessageStrategy, ((CfTextBroadcastConfig)broadcast.Item).BigMessageStrategy);
        }

        [Test]
        public void Test_UpdateBroadcastChangeTypeIVR()
        {
            ExpectedBroadcastIvr = new CfBroadcast
            {
                Name = "Name",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Congratulations! You have successfully configured a CallFire I V R.</play></dialplan>",
                    RetryConfig = new CfBroadcastConfigRetryConfig(),
               },
            };
            var id = Client.CreateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcastIvr));

            const string newName = "changeTypeIVR";
            const string newDialplanXml = "<dialplan><play type=\"tts\">Updated DialplanXml</play></dialplan>";
            ExpectedBroadcast = new CfBroadcast
            {
                Id = id,
                Name = newName,
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = newDialplanXml,
                },
            };

            Client.UpdateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcast));
            var broadcast = Client.GetBroadcast(id);

            Assert.AreEqual(newName, broadcast.Name);
            Assert.AreEqual(newDialplanXml, ((CfIvrBroadcastConfig)broadcast.Item).DialplanXml);
        }

        [Test]
        public void Test_UpdateBroadcastChangeTypeINVALID()
        {
            ExpectedBroadcast = new CfBroadcast
            {
                Id = 789,
                Name = "changeType",
                Type = CfBroadcastType.Ivr,
                Item = new CfIvrBroadcastConfig
                {
                    FromNumber = VerifyFromNumber,
                    DialplanXml = "<dialplan><play type=\"tts\">Fail Updated DialplanXml</play></dialplan>",
                },
            };
            AssertClientException<WebException, FaultException<ServiceFaultInfo>>(() => Client.UpdateBroadcast(new CfBroadcastRequest(string.Empty, ExpectedBroadcast)));
        }

        //GetBroadcastStats


        [Test]
        public void Test_QueryContactBatches()
        {
            var contactBatchQueryResult = Client.QueryContactBatches(QueryContactBatches);
            Assert.IsNotNull(contactBatchQueryResult);
            Assert.IsNotNull(contactBatchQueryResult.ContactBatch);
            Assert.IsTrue(contactBatchQueryResult.ContactBatch.Any(c => c.Id.Equals(1092170001)));
        }

        [Test]
        public void Test_GetContactBatches()
        {
            var contactBatch = Client.GetContactBatch(1092170001);
            Assert.IsNotNull(contactBatch);
            Assert.AreEqual(contactBatch.Status, CfBatchStatus.Active);
            Assert.AreEqual(contactBatch.Size, 1);
        }

        [Test]
        public void Test_ControlContactBatches()
        {
            Client.ControlContactBatch(ControlContactBatches);
        }

        [Test]
        public void Test_GetBroadcastStats()
        {
            var broadcastStats = Client.GetBroadcastStats(GetBroadcastStats);
            Assert.IsNotNull(broadcastStats);
            Assert.IsNotNull(broadcastStats.ActionStatistics);
            Assert.AreEqual(broadcastStats.ActionStatistics.Unattempted, 1);
            Assert.IsNotNull(broadcastStats.UsageStats);
            Assert.AreEqual(broadcastStats.UsageStats.Duration, 0);
        }

        [Test]
        public void Test_ControlBroadcast()
        {
            var broadcastRequest = new CfBroadcastRequest(string.Empty, ExpectedBroadcastDefault);
            ControlBroadcast.Id = Client.CreateBroadcast(broadcastRequest);
            Client.ControlBroadcast(ControlBroadcast);
        }

        [Test]
        public void Test_CreateContactBatch()
        {
            var id = Client.CreateContactBatch(CreateContactBatch);
            Assert.IsNotNull(id);
        }
    }
}
