using Binance.API.Csharp.Client.Domain.Abstract;
using Binance.API.Csharp.Client.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Binance.API.Csharp.Client.Utils;
using Binance.API.Csharp.Client.Models.Enums;
using WebSocketSharp;
using Binance.API.Csharp.Client.Models.WebSocket;
using System.Threading;
namespace Binance.API.Csharp.Client
{
    public class ApiClient : ApiClientAbstract, IApiClient
    {

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="apiKey">Key used to authenticate within the API.</param>
        /// <param name="apiSecret">API secret used to signed API calls.</param>
        /// <param name="apiUrl">API base url.</param>
        public ApiClient(string apiKey, string apiSecret, string apiUrl = @"https://www.binance.com", string webSocketEndpoint = @"wss://stream.binance.com:9443/", bool addDefaultHeaders = true) : base(apiKey, apiSecret, apiUrl, webSocketEndpoint, addDefaultHeaders)
        {
        }

        /// <summary>
        /// Calls API Methods.
        /// </summary>
        /// <typeparam name="T">Type to which the response content will be converted.</typeparam>
        /// <param name="method">HTTPMethod (POST-GET-PUT-DELETE)</param>
        /// <param name="endpoint">Url endpoing.</param>
        /// <param name="isSigned">Specifies if the request needs a signature.</param>
        /// <param name="parameters">Request parameters.</param>
        /// <returns></returns>
        public async Task<T> CallAsync<T>(ApiMethod method, string endpoint, bool isSigned = false, string parameters = null)
        {
            var finalEndpoint = endpoint + (string.IsNullOrWhiteSpace(parameters) ? "" : $"?{parameters}");

            if (isSigned)
            {
                parameters += (!string.IsNullOrWhiteSpace(parameters) ? "&timestamp=" : "timestamp=") + Utilities.GenerateTimeStamp(DateTime.Now);
                var signature = Utilities.GenerateSignature(_apiSecret, parameters);
                finalEndpoint = $"{endpoint}?{parameters}&signature={signature}";
            }

            var request = new HttpRequestMessage(Utilities.CreateHttpMethod(method.ToString()), finalEndpoint);
            //Thread.Sleep(500);
            Console.WriteLine(finalEndpoint);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(true);            
            
            //Console.WriteLine(response.Headers);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            if (result.StartsWith("{\"code\":-"))
            {
                throw new Exception(result);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        /// <summary>
        /// Connects to a Websocket endpoint.
        /// </summary>
        /// <typeparam name="T">Type used to parsed the response message.</typeparam>
        /// <param name="parameters">Paremeters to send to the Websocket.</param>
        /// <param name="messageDelegate">Deletage to callback after receive a message.</param>
        /// <param name="useCustomParser">Specifies if needs to use a custom parser for the response message.</param>
        public void ConnectToWebSocket<T>(string parameters, MessageHandler<T> messageHandler, int useCustomParser = 0)
        {
            var finalEndpoint = _webSocketEndpoint + parameters;

            var ws = new WebSocket(finalEndpoint);

            ws.OnMessage += (sender, e) =>
            {
                dynamic eventData;

                if (useCustomParser == 0)
                //DEPTH
                {
                    var customParser = new CustomParser();
                    eventData = customParser.GetParsedDepthMessage(JsonConvert.DeserializeObject<dynamic>(e.Data));
                }
                else if (useCustomParser == 1)
                //TRADEAGG
                {
                    //eventData = JsonConvert.DeserializeObject<T>(e.Data);
                    var customParser = new CustomParser();
                    eventData = customParser.GetAggTradeMessage(JsonConvert.DeserializeObject<dynamic>(e.Data));
                }
                else// if (useCustomParser == 2)
                //PARTIAL_DEPTH
                {
                    var customParser = new CustomParser();
                    eventData = customParser.GetParsedPartialDepthMessage(JsonConvert.DeserializeObject<dynamic>(e.Data));
                }

                messageHandler(eventData);
            };

            ws.OnClose += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.OnError += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.Connect();
            _openSockets.Add(ws);
        }

        /// <summary>
        /// Connects to a UserData Websocket endpoint.
        /// </summary>
        /// <param name="parameters">Paremeters to send to the Websocket.</param>
        /// <param name="accountHandler">Deletage to callback after receive a account info message.</param>
        /// <param name="tradeHandler">Deletage to callback after receive a trade message.</param>
        /// <param name="orderHandler">Deletage to callback after receive a order message.</param>
        public void ConnectToUserDataWebSocket(string parameters, MessageHandler<AccountUpdatedMessage> accountHandler, MessageHandler<OrderOrTradeUpdatedMessage> tradeHandler, MessageHandler<OrderOrTradeUpdatedMessage> orderHandler)
        {
            var finalEndpoint = _webSocketEndpoint + "ws/" + parameters;

            var ws = new WebSocket(finalEndpoint);

            ws.OnMessage += (sender, e) =>
            {
                var eventData = JsonConvert.DeserializeObject<dynamic>(e.Data);                                
                if (eventData.e == "outboundAccountInfo")
                {
                    accountHandler(JsonConvert.DeserializeObject<AccountUpdatedMessage>(e.Data));                    
                }

                else if (eventData.e == "executionReport")
                {
                    var isTrade = ((string)eventData.x).ToLower() == "trade";                    
                    if (isTrade)
                    {
                        tradeHandler(JsonConvert.DeserializeObject<OrderOrTradeUpdatedMessage>(e.Data));
                    }
                    else
                    {
                        orderHandler(JsonConvert.DeserializeObject<OrderOrTradeUpdatedMessage>(e.Data));
                    }
                }

                        
                
            };

            ws.OnClose += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.OnError += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.Connect();
            _openSockets.Add(ws);
        }

        public Exception GetExceptionMessage(string responseMsg)
        {
            //-1000 UNKNOWN An unknown error occured while processing the request.
            //-1001 DISCONNECTED Internal error; unable to process your request.Please try again.
            //-1002 UNAUTHORIZED You are not authorized to execute this request.
            //-1003 TOO_MANY_REQUESTS
            //Too many requests.
            //Too many requests queued.
            //Too many requests; current limit is % s requests per minute.
            //Way too many requests; IP banned until % s.
            //-1006 UNEXPECTED_RESP An unexpected response was received from the message bus.Execution status unknown.
            //-1007 TIMEOUT Timeout waiting for response from backend server. Send status unknown; execution status unknown.
            //-1013 INVALID_MESSAGE
            //-1014 UNKNOWN_ORDER_COMPOSITION Unsupported order combination.
            //-1015 TOO_MANY_ORDERS
            //Too many new orders.
            //Too many new orders; current limit is % s orders per % s.

            return new Exception(responseMsg);
        }
    }
}
