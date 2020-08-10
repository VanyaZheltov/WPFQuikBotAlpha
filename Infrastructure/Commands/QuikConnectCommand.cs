using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFQuikBotTest.Infrastructure.Commands.Base;
using WPFQuikBotTest.QuikSharpConnect;

namespace WPFQuikBotTest.Infrastructure.Commands
{
    class QuikConnectCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            
        }
    }
}
