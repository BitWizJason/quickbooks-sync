﻿using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    public interface IMessageValidator
    {
        /// <summary>
        /// Verifies if the version and filename is allowed to start exchanging information.
        /// If the version is incorrect, return false.
        /// Another message will be sent to "IsValidTicket" with the ticket only to check if the
        /// version is allowed.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="strCompanyFileName">Company File Name.</param>
        /// <param name="qbXMLCountry">Country code.</param>
        /// <param name="qbXMLMajorVers">QbXml Major Version.</param>
        /// <param name="qbXMLMinorVers">QbXml Minor Version.</param>
        /// <returns>False if the version is too low.</returns>
        Task<bool> ValidateMessageAsync(string ticket, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers);

        /// <summary>
        /// Returns false if the ticket has been marked as invalid version.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>False if the version is too low for the ticket.</returns>
        Task<bool> IsValidTicketAsync(string ticket);
    }
}
