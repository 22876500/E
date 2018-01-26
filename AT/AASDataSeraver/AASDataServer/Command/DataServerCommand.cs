using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AASDataServer.Command
{
    /**
     * 访问入口
     **/
    public class DataServerCommand
    {
        private static RoutedUICommand _startServer;
        private static RoutedUICommand _stopServer;
        private static RoutedUICommand _clearCache;
        private static RoutedUICommand _startIceDataServer;
        private static RoutedUICommand _stopIceDataServer;
        private static RoutedUICommand _startDataSource;
        private static RoutedUICommand _stopDataSource;
        private static RoutedUICommand _startPubServer;
        private static RoutedUICommand _stopPubServer;

        public static RoutedUICommand StartServer
        {
            get {
                return _startServer;
            }
        }

        public static RoutedUICommand StopServer
        {
            get {
                return _stopServer;
            }
        }

        public static RoutedUICommand ClearCache
        {
            get {
                return _clearCache;
            }
        }

        public static RoutedUICommand StartIceDataServer
        {
            get
            {
                return _startIceDataServer;
            }
        }

        public static RoutedUICommand StopIceDataServer
        {
            get
            {
                return _stopIceDataServer;
            }
        }
        public static RoutedUICommand StartDataSource
        {
            get
            {
                return _startDataSource;
            }
        }

        public static RoutedUICommand StopDataSource
        {
            get
            {
                return _stopDataSource;
            }
        }

        public static RoutedUICommand StartPubServer
        {
            get
            {
                return _startPubServer;
            }
        }

        public static RoutedUICommand StopPubServer
        {
            get
            {
                return _stopPubServer;
            }
        }

        static DataServerCommand()
        {
            _startServer = new RoutedUICommand();
            _stopServer = new RoutedUICommand();
            _clearCache = new RoutedUICommand();
            _startIceDataServer = new RoutedUICommand();
            _stopIceDataServer = new RoutedUICommand();
            _startDataSource = new RoutedUICommand();
            _stopDataSource = new RoutedUICommand();
            _startPubServer = new RoutedUICommand();
            _stopPubServer = new RoutedUICommand();
        }

    }
}
