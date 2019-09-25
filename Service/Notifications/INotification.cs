using System;

namespace Service.Notifications
{
    public interface INotification
    {
        void NotifyById(Guid id);

        void NotifyAll();
    }
}