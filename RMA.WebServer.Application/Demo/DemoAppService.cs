using Abp;
using Abp.Authorization;
using Abp.Notifications;
using Abp.RealTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RMA.WebServer.Demo
{
    public class DemoAppService : AppServiceBase, IDemoAppService
    {
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly INotificationPublisher _notificationPublisher;

        public DemoAppService(
            IOnlineClientManager onlineClientManager,
            INotificationPublisher notificationPublisher)
        {
            _onlineClientManager = onlineClientManager;
            _notificationPublisher = notificationPublisher;
        }

        public object Get()
        {
            return new string[] { "value1", "value2" };
        }

        public object GetAllClient()
        {
            return _onlineClientManager.GetAllClients();
        }

        [AbpAuthorize]
        public object GetUser()
        {
            return AbpSession.UserId;
        }

        public async Task PublishMessage(long userId, string message)
        {
            var user = new UserIdentifier(1, userId);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { user }
            );
        }
    }
}
