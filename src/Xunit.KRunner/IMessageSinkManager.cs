using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Xunit.KRunner
{
    public interface IMessageSinkManager
    {
        IMessageSinkFactory GetMessageSinkFactory(string messageSinkName);
    }
}
