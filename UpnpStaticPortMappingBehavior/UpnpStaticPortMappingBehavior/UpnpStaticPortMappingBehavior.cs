using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web;
using NATUPNPLib;

namespace UpnpStaticPortMappingBehavior
{
    /// <summary>
    /// A WCF service port that allows to map a router incomming port to the WCF service port.
    /// This service behavior can be used for WCF services hosted behind a NAT router supporting UPNP.
    /// 
    /// This behavior can be applied to a WCF service as attribute or by using web.config or app.config WCF
    /// configuration.
    /// </summary>
    public class UpnpStaticPortMappingBehavior : Attribute, IServiceBehavior
    {
        private readonly UPnPNAT upnp;

        public UpnpStaticPortMappingBehavior()
        {
            this.upnp = new UPnPNAT();
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            AddPortMapping(serviceHostBase);
            serviceHostBase.Closing += new EventHandler((sender, e) => RemovePortMapping((ServiceHostBase)sender));
        }

        /// <summary>
        /// Add a UPNP port mapping in the router.
        /// </summary>
        /// <param name="serviceHostBase">The WCF <see cref="ServiceHostBase"/> hosting the service.</param>
        private void AddPortMapping(ServiceHostBase serviceHostBase)
        {
            ServiceEndpointCollection serviceEndpoints = serviceHostBase.Description.Endpoints;

            IEnumerable<int> endpointPorts = serviceEndpoints.Select(endpoint => endpoint.Address.Uri.Port).Distinct();
            string internalAddress = GetIpAddress();

            foreach (int port in endpointPorts)
            {
                upnp.StaticPortMappingCollection.Add(port, "TCP", port, internalAddress, true, "WCF Web Service");
            }
        }

        /// <summary>
        /// Remove a UPNP port mappping in the router.
        /// </summary>
        /// <param name="serviceHostBase">The WCF <see cref="ServiceHostBase"/> hosting the service.</param>
        private void RemovePortMapping(ServiceHostBase serviceHostBase)
        {
            ServiceEndpointCollection serviceEndpoints = serviceHostBase.Description.Endpoints;

            IEnumerable<int> endpointPorts = serviceEndpoints.Select(endpoint => endpoint.Address.Uri.Port).Distinct();

            foreach (int port in endpointPorts)
            {
                upnp.StaticPortMappingCollection.Remove(port, "TCP");
            }
        }

        /// <summary>
        /// Get the internet IP Address of the machine on the local network.
        /// </summary>
        /// <returns>The internet IP Address of the machine on the local network.</returns>
        private string GetIpAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            return (ipAddress != null) ? ipAddress.ToString() : null;
        }
    }
}