﻿using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Messages.Responses;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Messages;
using System;
using System.Xml;

namespace QbSync.WebConnector.Tests
{
    [TestFixture]
    class StepQueryWithIteratorTests
    {
        [Test]
        public void CustomerQueryWithNoInitialMessage()
        {
            var defaultMaxResult = 100;
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = 4
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRequest, CustomerQueryResponse, Customer[]>>(authenticatedTicket);
            stepQueryWithIteratorMock.CallBase = true;

            var xml = stepQueryWithIteratorMock.Object.SendXML();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(defaultMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public void CustomerQueryWithPreviousIterator()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = 4
            };
            var iteratorID = "123456";
            var iteratorKey = StepQueryWithIterator<CustomerQueryRequest, CustomerQueryResponse, Customer[]>.IteratorKey;
            var updateCustomerMock = new Mock<StepQueryWithIterator<CustomerQueryRequest, CustomerQueryResponse, Customer[]>>(authenticatedTicket);
            updateCustomerMock
                .Protected()
                .Setup<string>("RetrieveMessage", ItExpr.Is<string>(s => s == authenticatedTicket.Ticket), ItExpr.Is<int>(n => n == authenticatedTicket.CurrentStep), ItExpr.Is<string>(s => s == iteratorKey))
                .Returns(iteratorID);
            updateCustomerMock.CallBase = true;

            var xml = updateCustomerMock.Object.SendXML();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Continue", node.Attributes.GetNamedItem("iterator").Value);
            Assert.AreEqual(iteratorID, node.Attributes.GetNamedItem("iteratorID").Value);
        }

        [Test]
        public void CustomerQueryWithResponseWithNoIterators()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = 4
            };
            var xml = @"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">" +
                "<CustomerRet>" +
                    "<ListID>110000-1232697602</ListID>" +
                    "<TimeCreated>2009-01-23T03:00:02-05:00</TimeCreated>" +
                    "<TimeModified>2009-01-23T03:00:02-05:00</TimeModified>" +
                    "<EditSequence>1232697602</EditSequence>" +
                    "<Name>10th Customer</Name>" +
                    "<FullName>ABC Customer</FullName>" +
                    "<IsActive>true</IsActive>" +
                    "<Sublevel>0</Sublevel>" +
                    "<Balance>0.00</Balance>" +
                    "<TotalBalance>0.00</TotalBalance>" +
                    "<JobStatus>None</JobStatus>" +
                "</CustomerRet>" +
                "</CustomerQueryRs></QBXMLMsgsRs></QBXML>";

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRequest, CustomerQueryResponse, Customer[]>>(authenticatedTicket);
            stepQueryWithIteratorMock.CallBase = true;

            var ret = stepQueryWithIteratorMock.Object.ReceiveXML(xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public void CustomerQueryWithResponseWithIterators()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = 4
            };
            var iteratorID = "{eb05f701-e727-472f-8ade-6753c4f67a46}";
            var xml = @"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"" iteratorRemainingCount=""18"" iteratorID=""" + iteratorID + @""">" +
                "<CustomerRet>" +
                    "<ListID>110000-1232697602</ListID>" +
                    "<TimeCreated>2009-01-23T03:00:02-05:00</TimeCreated>" +
                    "<TimeModified>2009-01-23T03:00:02-05:00</TimeModified>" +
                    "<EditSequence>1232697602</EditSequence>" +
                    "<Name>10th Customer</Name>" +
                    "<FullName>ABC Customer</FullName>" +
                    "<IsActive>true</IsActive>" +
                    "<Sublevel>0</Sublevel>" +
                    "<Balance>0.00</Balance>" +
                    "<TotalBalance>0.00</TotalBalance>" +
                    "<JobStatus>None</JobStatus>" +
                "</CustomerRet>" +
                "</CustomerQueryRs></QBXMLMsgsRs></QBXML>";

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRequest, CustomerQueryResponse, Customer[]>>(authenticatedTicket);
            stepQueryWithIteratorMock.CallBase = true;

            var ret = stepQueryWithIteratorMock.Object.ReceiveXML(xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }
    }
}
