﻿using QbSync.QbXml;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    public interface IStepQueryRequest
    {
        /// <summary>
        /// Returns the step name.
        /// </summary>
        /// <returns>Step name.</returns>
        string Name { get; }

        /// <summary>
        /// Returns the string that has to be sent to the Web Connector.
        /// Return null if your step has nothing to do this time. The next step will be executed immediately.
        /// </summary>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <returns>QbXml or null to execute the next step.</returns>
        Task<string> SendXMLAsync(IAuthenticatedTicket authenticatedTicket);

        /// <summary>
        /// Sets the options to be attached with the step.
        /// </summary>
        /// <param name="qbXmlResponseOptions">Options.</param>
        Task SetOptionsAsync(QbXmlResponseOptions qbXmlResponseOptions);
    }
}
