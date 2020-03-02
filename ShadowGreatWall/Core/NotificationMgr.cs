using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Core
{
    class NotificationMgr
    {
        #region [单例]
        private readonly static NotificationMgr instance = new NotificationMgr();

        public static NotificationMgr Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region [通知]
        public delegate void OnNotificationDelegate(NotifycationType type);
        public event OnNotificationDelegate OnNotification;
        #endregion

        private NotificationMgr()
        { }

        public void TryNotify(NotifycationType type)
        {
            OnNotification(type);
        }
    }

    enum NotifycationType
    { 
        SubscribeUpdated,
    }
}
