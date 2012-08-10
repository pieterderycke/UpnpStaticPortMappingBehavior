using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace UpnpStaticPortMappingBehavior
{
    /// <summary>
    /// The WCF <see cref="BehaviorExtensionElement"/> to apply the <see cref="UpnpStaticPortMappingBehavior"/>
    /// to a service using the WCF configuration section in the web.config or the app.config.
    /// </summary>
    public class UpnpStaticPortMappingBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof (UpnpStaticPortMappingBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new UpnpStaticPortMappingBehavior();
        }
    }
}