using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using GAF.Network;
using GAF.Network.Serialization;

namespace GAF.ServiceDiscovery.Consul
{
	public class Client : IServiceDiscovery
	{
		#region IServiceDiscovery

		public List<IPEndPoint> GetActiveServices (string serviceName)
		{
			var endpoints = new List<IPEndPoint> ();

			var services = GetServices (serviceName, true);

			if (services == null || services.Count == 0) 
			{
				throw new GAF.Network.SocketException ("Unable to retrieve services from Service Discovery.");
			}

			foreach (var service in services) {
				var address = service.Service.Address;
				var port = service.Service.Port;

				if (!endpoints.Where (e => e.Address.ToString ().Equals (address) && e.Port == port).Any ()) {
					endpoints.Add (EvaluationClient.CreateEndpoint (string.Format ("{0}:{1}", address, port)));
				}
			}
			return endpoints;
		}

		public bool RegisterService (string serviceId, IPEndPoint serviceEndPoint, IPEndPoint checkEndPoint) {

			//create service definition
            var serviceDefinition = CreateServiceDefinition(serviceId,serviceEndPoint,checkEndPoint);
            return Register (serviceDefinition);	
		}

		public bool DeRegisterService (string serviceId) {
			return DeRegister (serviceId);
		}

		#endregion

		#region API Endpoints

		//private const string baseUrl = "http://192.168.1.90:8500";
		private const int _defaultPort = 8500;

		#endregion

		#region Members

		private HttpClient _httpClient;
		//private readonly IPAddress _ipAddress;
		//private readonly EndPoint _endPoint;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RemoteEvaluationServer.ConsulClient"/> class.
		/// Uses a default endpoint for Consul Agent of localhost:8500.
		/// </summary>
		public Client ()
		{
			//nothing specified so use local host details
			var ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
			var remoteEndPoint = new IPEndPoint (ipHostInfo.AddressList [0], _defaultPort);

			this.NodeEndPoint = remoteEndPoint;
			InitialiseHttpClient ();

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Plugins.Consul.ConsulClient"/> class.
		/// </summary>
		/// <param name="ipAddress">IP address.</param>
		/// <param name="port">Port.</param>
		public Client (string ipAddress, int port)
		{
			var url = string.Format ("{0}:{1}", ipAddress, port);
			this.NodeEndPoint = CreateEndpoint (url);
			InitialiseHttpClient ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Consul.ConsulClient"/> class.
		/// </summary>
		/// <param name="ipAddress">Ip address.</param>
		/// <param name="port">Port.</param>
		public Client (IPAddress ipAddress, int port)
		{
			var url = string.Format ("{0}:{1}", ipAddress, port);
			this.NodeEndPoint = CreateEndpoint (url);
			InitialiseHttpClient ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RemoteEvaluationServer.ConsulClient"/> class.
		/// </summary>
		/// <param name="endPoint">End point of Consul Agent.</param>
		public Client (EndPoint endPoint)
		{
			this.NodeEndPoint = endPoint;
			InitialiseHttpClient ();
		}

		private void InitialiseHttpClient ()
		{
			var cookieContainer = new CookieContainer ();

			var handler = new HttpClientHandler {
				//Credentials = new NetworkCredential (Username, Password),
				UseCookies = true,
				CookieContainer = cookieContainer,
				UseDefaultCredentials = false
			};

			_httpClient = new HttpClient (handler);

		}
		#region Properties

		public EndPoint NodeEndPoint { get; private set; }

		#endregion

		#region API Methods

        private ServiceDefinition CreateServiceDefinition(string serviceId, IPEndPoint serviceEndPoint, IPEndPoint checkEndPoint)
        {
            var serviceDefinition = new ServiceDefinition () {
                Name = "gaf-evaluation-server", //format suitable for dns
                Id = string.Format("GAF-Server:{0}:{1}",serviceEndPoint.Address.ToString(), serviceEndPoint.Port),
                Address = serviceEndPoint.Address.ToString(),
                Port = serviceEndPoint.Port
            };
                    
            serviceDefinition.Check.TCP = string.Format ("{0}:{1}", serviceEndPoint.Address.ToString(), serviceEndPoint.Port);
            serviceDefinition.Check.Interval = "10s";
            serviceDefinition.Check.Timeout = "1s";
            serviceDefinition.Check.Notes = "TCP connection check.";

            return serviceDefinition;
        }

		private bool Register (ServiceDefinition serviceDefinition)
		{
			var url = string.Format ("http://{0}/v1/agent/service/register", this.NodeEndPoint);

			var jsonDoc = Json.Serialize<ServiceDefinition> (serviceDefinition);
			var result = SendRequest (url, HttpMethod.Put, jsonDoc);
			return result.StatusCode == HttpStatusCode.OK;
		}
		private bool DeRegister (string serviceId)
		{
			var url = string.Format ("http://{0}/v1/agent/service/deregister/{1}", this.NodeEndPoint, serviceId);

			//var result = Get (url);
			var result = SendRequest (url, HttpMethod.Get);
			return result.StatusCode == HttpStatusCode.OK;
		}

		private List<AvailableServices> GetServices (string serviceName, bool passing)
		{
			var url = string.Format ("http://{0}/v1/health/service/{1}", this.NodeEndPoint, serviceName);

			if (passing) {
				url = string.Format ("{0}?{1}", url, "passing");
			}

			var result = SendRequest (url, HttpMethod.Get);
			var json = result.Content.ReadAsStringAsync ().Result;

			var knownTypes = new List<Type> ();
			knownTypes.Add (typeof (Node));
			knownTypes.Add (typeof (Check));
			knownTypes.Add (typeof (Service));
			knownTypes.Add (typeof (TaggedAddresses));

			var services = Json.DeSerialize<List<AvailableServices>> (json, knownTypes);

			return services;
		}
		#endregion

		#region Helper Methods

		/// <summary>
		/// Creates an endpoint from the IPAddress and Port number as a colon delimetered string.
		/// Parameter must be in the format IPAddress:PortNumber e.g. 192.168.1.64:11000.
		/// </summary>
		/// <returns>The endpoint.</returns>
		/// <param name="endpointAddress">Address with port.</param>
		public static IPEndPoint CreateEndpoint (string endpointAddress)
		{
			IPEndPoint ipEndPoint = null;

			if (endpointAddress.Contains (":")) {

				var epSegments = endpointAddress.Split (":".ToCharArray ());

				IPAddress addr = null;
				int port = 0;

				if (IPAddress.TryParse (epSegments [0], out addr) &&
					int.TryParse (epSegments [1], out port)) {

					ipEndPoint = new IPEndPoint (addr, port);

				}
			}

			return ipEndPoint;
		}

		#endregion

		#region Http Methods

		private HttpResponseMessage SendRequest (string resourceUrl, HttpMethod method)
		{
			return SendRequest (resourceUrl, method, null);
		}

		private HttpResponseMessage SendRequest (string resourceUrl, HttpMethod method, string jsonDocument)
		{
			var httpRequest = new HttpRequestMessage (method, resourceUrl);

			if (method == HttpMethod.Put || method == HttpMethod.Post) {
				var httpContent = new StringContent (jsonDocument, Encoding.UTF8, "application/json");
				httpRequest.Content = httpContent;
			}

			var httpResponse = _httpClient.SendAsync (httpRequest).Result;

			return httpResponse;
		}

		#endregion
	}
}