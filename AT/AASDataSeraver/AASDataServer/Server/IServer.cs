using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Server
{
    public interface IServer
    {
        bool IsRunning { get; }

        int Start();

        int Stop();
    }
}
