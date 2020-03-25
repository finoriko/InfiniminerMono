using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniminerShared
{
    public class ChatMessage
    {
        public string message;
        public ChatMessageType type;
        public float timestamp;
        public int newlines;

        public ChatMessage(string message, ChatMessageType type, float timestamp, int newlines)
        {
            this.message = message;
            this.type = type;
            this.timestamp = timestamp;
            this.newlines = newlines;
        }
    }
}
