using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Trigger
    {
        public static void DropTableTrigger()
        {
            SqlTriggerContext triggContext = SqlContext.TriggerContext;

            switch (triggContext.TriggerAction)
            {
                case TriggerAction.DropTable:
                    SqlContext.Pipe.Send("Table dropped! Here's the EventData:");
                    SqlContext.Pipe.Send(triggContext.EventData.Value);
                    break;

                default:
                    SqlContext.Pipe.Send("Something happened! Here's the EventData:");
                    SqlContext.Pipe.Send(triggContext.EventData.Value);
                    break;
            }
        }
    }
}
