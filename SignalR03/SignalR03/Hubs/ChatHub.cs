using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using SignalR03.Models;

namespace SignalR03.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly List<User> Users = new List<User>();

        // Отправка сообщений
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (Users.All(x => x.Id != id))
            {
                Users.Add(User.CreateUser(id, userName));

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.Id == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
