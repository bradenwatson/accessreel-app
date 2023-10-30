using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.Messages
{
    public class OpenPageMessage : ValueChangedMessage<string>
    {
        public OpenPageMessage(string value) : base(value)
        {
            // add custom logic or initilisation here (only if required)
        }
    }
}
