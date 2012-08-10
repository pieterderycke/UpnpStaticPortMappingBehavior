#UpnpStaticPortMappingBehavior for WCF

This is a WCF service behavior that allows to map an incomming port on the router to the port used by the WCF service. It can be used by by WCF services hosted behind a NAT router supporting UPNP.

It can be applied to a WCF service in two ways:
*   In Code
*   In Configuration

##In Code
By annotating the WCF service with the behavior, the behavior will be added into the WCF pipeline:

```csharp
[UpnpStaticPortMappingBehavior]
public class YourService : IYourService
{
	// Your service implementation ...
}
```

##In Configuration
If you prefer configuration based config of WCF:

```xml
<system.serviceModel>
  <!-- Register the upnpPortMapping extension -->
  <extensions>
    <behaviorExtensions>
      <add name="upnpPortMapping" 
           type="UpnpStaticPortMappingBehavior.UpnpStaticPortMappingBehavior, UpnpStaticPortMappingBehavior, Version=1.0.0.0, Culture=neutral"/>
    </behaviorExtensions>
  </extensions>
  
  <behaviors>
    <serviceBehaviors>
      <behavior name="yourServiceBehavior">
        <upnpPortMapping/>
		<!-- Possible other behaviors ... -->
      </behavior>
    </serviceBehaviors>
  </behaviors>
  
  <!-- The configuration of your services and client endpoints ... -->
</system.serviceModel>
```

##Remarks
Don't forget to configure the Windows firewall or any other firewall installed on your computer to allow the incomming traffic.