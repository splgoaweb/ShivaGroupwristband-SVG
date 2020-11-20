
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Plugin.NopFeatures.ShipRocket.Domain;
using Nop.Services.Affiliates;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.NopFeatures.ShipRocket.Service
{
    public partial class ShipRocketMessageService : IShipRocketMessageService
    {
        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IAddressService _addressService;
        private readonly IAffiliateService _affiliateService;
        private readonly ICustomerService _customerService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ITokenizer _tokenizer;

        #endregion

        #region Ctor

        public ShipRocketMessageService(CommonSettings commonSettings,
            EmailAccountSettings emailAccountSettings,
            IAddressService addressService,
            IAffiliateService affiliateService,
            ICustomerService customerService,
            IEmailAccountService emailAccountService,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            IOrderService orderService,
            IProductService productService,
            IQueuedEmailService queuedEmailService,
            IStoreContext storeContext,
            IStoreService storeService,
            ITokenizer tokenizer)
        {
            _commonSettings = commonSettings;
            _emailAccountSettings = emailAccountSettings;
            _addressService = addressService;
            _affiliateService = affiliateService;
            _customerService = customerService;
            _emailAccountService = emailAccountService;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _localizationService = localizationService;
            _messageTemplateService = messageTemplateService;
            _messageTokenProvider = messageTokenProvider;
            _orderService = orderService;
            _productService = productService;
            _queuedEmailService = queuedEmailService;
            _storeContext = storeContext;
            _storeService = storeService;
            _tokenizer = tokenizer;
        }

        #endregion

        #region Method
        /// <summary>
        /// Method to send error notification to store owner
        /// </summary>
        /// <param name="order"></param>
        /// <param name="languageId"></param>
        /// <param name="nopShiprocketOrder"></param>
        /// <returns></returns>
        public virtual int SendOrderShiprocketErrorStoreOwnerNotification(Order order, int languageId, NopShiprocketOrder nopShiprocketOrder)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplates = GetActiveMessageTemplates(MessageTemplateSystemNames.OrderPlacedStoreOwnerNotification, store.Id);
            if (!messageTemplates.Any())
                return 0;

            //email account
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplates.FirstOrDefault(), languageId);

            //tokens
            var tokens = new List<Token>();
            _messageTokenProvider.AddStoreTokens(tokens, store, emailAccount);
            _messageTokenProvider.AddOrderTokens(tokens, order, languageId);
            _messageTokenProvider.AddCustomerTokens(tokens, _customerService.GetCustomerById(order.CustomerId));

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplates.FirstOrDefault(), tokens);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;

            return SendErrorNotification(messageTemplates.FirstOrDefault(), emailAccount, languageId, tokens, toEmail, toName, null, null, null, null, null, null, null, nopShiprocketOrder);
        }

        /// <summary>
        /// Active Language cheking method
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        protected virtual int EnsureLanguageIsActive(int languageId, int storeId)
        {
            //load language by specified ID
            var language = _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = _languageService.GetAllLanguages(storeId: storeId).FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }

        /// <summary>
        /// Method to get active message templates
        /// </summary>
        /// <param name="messageTemplateName"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        protected virtual IList<MessageTemplate> GetActiveMessageTemplates(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = _messageTemplateService.GetMessageTemplatesByName(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        /// <summary>
        /// getting email of template
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        protected virtual EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = _localizationService.GetLocalized(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (_emailAccountService.GetEmailAccountById(emailAccountId) ?? _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId)) ??
                               _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;
        }

        /// <summary>
        /// method to send error notification
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="emailAccount"></param>
        /// <param name="languageId"></param>
        /// <param name="tokens"></param>
        /// <param name="toEmailAddress"></param>
        /// <param name="toName"></param>
        /// <param name="attachmentFilePath"></param>
        /// <param name="attachmentFileName"></param>
        /// <param name="replyToEmailAddress"></param>
        /// <param name="replyToName"></param>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="subject"></param>
        /// <param name="shiprocketorder"></param>
        /// <returns></returns>
        public virtual int SendErrorNotification(MessageTemplate messageTemplate,
         EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
         string toEmailAddress, string toName,
         string attachmentFilePath = null, string attachmentFileName = null,
         string replyToEmailAddress = null, string replyToName = null,
         string fromEmail = null, string fromName = null, string subject = null, NopShiprocketOrder shiprocketorder = null)
        {

            string AdditionalBody = null;


            if (shiprocketorder != null)
            {
                if (shiprocketorder.ShiprocketStatues == true || string.IsNullOrEmpty(shiprocketorder.ErrorResponse))
                {
                    AdditionalBody = string.Format("We fount that this order {0} has been successfully uploaded so we don't proceess this order to shiprocket. ShipRocket Order Id is {1}", shiprocketorder.OrderId, shiprocketorder.ShiprocketOrderId);
                }
                else
                {
                    AdditionalBody = string.Format("We found some error while creating this order {0} at ship rocket below is Error Response of this order < br/> {1}", shiprocketorder.OrderId, shiprocketorder.ErrorResponse);
                }
            }
            else
            {
                return 0;

            }

            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            if (emailAccount == null)
                throw new ArgumentNullException(nameof(emailAccount));

            //retrieve localized message template data
            var bcc = _localizationService.GetLocalized(messageTemplate, x => x.BccEmailAddresses, languageId);
            if (string.IsNullOrEmpty(subject))
                subject = _localizationService.GetLocalized(messageTemplate, x => x.Subject, languageId);
            var body = _localizationService.GetLocalized(messageTemplate, x => x.Body, languageId);

            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            bodyReplaced = bodyReplaced + "</br>" + AdditionalBody;

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                    : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }

        #endregion

    }
}
