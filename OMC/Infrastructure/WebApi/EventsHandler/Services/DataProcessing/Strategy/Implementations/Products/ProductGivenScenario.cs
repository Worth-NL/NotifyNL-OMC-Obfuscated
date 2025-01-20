// © 2025, Worth Systems.

using Common.Settings.Configuration;
using EventsHandler.Services.DataProcessing.Strategy.Base;
using EventsHandler.Services.DataProcessing.Strategy.Base.Interfaces;
using WebQueries.DataQuerying.Adapter.Interfaces;
using WebQueries.DataQuerying.Proxy.Interfaces;
using WebQueries.DataSending.Interfaces;
using WebQueries.DataSending.Models.DTOs;
using ZhvModels.Mapping.Models.POCOs.NotificatieApi;
using ZhvModels.Mapping.Models.POCOs.OpenKlant;
using ZhvModels.Mapping.Models.POCOs.OpenProduct;

namespace EventsHandler.Services.DataProcessing.Strategy.Implementations.Products
{
    /// <summary>
    /// <inheritdoc cref="INotifyScenario"/>
    /// The strategy for "Decision made" scenario.
    /// </summary>
    /// <seealso cref="BaseScenario"/>
    internal sealed class ProductGivenScenario : BaseScenario
    {
        private IQueryContext _queryContext = null!;
        private Product _product;
        private ProductType _productType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionMadeScenario"/> class.
        /// </summary>
        public ProductGivenScenario(
            OmcConfiguration configuration,
            IDataQueryService<NotificationEvent> dataQuery,
            INotifyService<NotifyData> notifyService)  // Dependency Injection (DI)
            : base(configuration, dataQuery, notifyService)
        {
        }

        #region Polymorphic (PrepareDataAsync)
        /// <inheritdoc cref="BaseScenario.PrepareDataAsync(NotificationEvent)"/>
        protected override async Task<PreparedData> PrepareDataAsync(NotificationEvent notification)
        {
            // Setup
            this._queryContext = this.DataQuery.From(notification);

            // Preparing party details
            return new PreparedData(
                party: await this._queryContext.GetPartyDataAsync(
                    this._product.Uri,
                    this._product.Bsn),
                caseUri: this._product.Uri);
        }
        #endregion

        protected override Guid GetEmailTemplateId()
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, object> GetEmailPersonalization(CommonPartyData partyData)
        {
            throw new NotImplementedException();
        }

        protected override Guid GetSmsTemplateId()
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, object> GetSmsPersonalization(CommonPartyData partyData)
        {
            throw new NotImplementedException();
        }

        protected override string GetWhitelistEnvVarName()
        {
            throw new NotImplementedException();
        }
    }
}
