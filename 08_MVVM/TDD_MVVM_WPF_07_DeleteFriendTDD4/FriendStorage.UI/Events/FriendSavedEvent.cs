using FriendStorage.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Aktualizacja nazwiska w nawigacji (lista nazwisk)
/// </summary>
namespace FriendStorage.UI.Events
{
    public class FriendSavedEvent : PubSubEvent<Friend>
    {
    }
}
