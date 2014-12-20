using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTCOS.Network
{
    public class AIRequest
    {

        public string requestMessage;
        public AISocket socket;
        public string type;

        public AIRequest( AISocket socket , string message )
        {
            requestMessage = message;
            this.socket = socket;
        }

        public void deal()
        {
            socket.messageManager.dealWith(requestMessage);
        }

    }
}
