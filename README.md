# MikrotikDotNet
MikrotikDotNet is a lightweight and easy to use ADO.NET like library for Mikrotik Api with extensibility and performance in mind.

[NuGet package:](https://www.nuget.org/packages/Mikrotik.Net)
```
Install-Package Mikrotik.Net
```

## How to use:
Simple command with parameters (ExecuteNonQuery):
```cs
using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
    conn.Open();
    var cmd = conn.CreateCommand("interface pppoe-client add");
    cmd.Parameters.Add("interface", "ether1");
    cmd.Parameters.Add("Name", "Test"); // You can use PascalCase or kebab-case in parameter name.
    cmd.Parameters.Add("user","Test");
    cmd.Parameters.Add("password", "Test");
    cmd.ExecuteNonQuery();
}
```

Read response: (ExecuteReader):
```cs
using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
  conn.Open();
  var cmd = conn.CreateCommand("ip address print");
  var result = cmd.ExecuteReader();
  foreach (var line in result)
    Console.WriteLine(line);
  
}
```
Result (Raw api response):
```
!re=.id=*4=address=10.20.1.19/16=network=10.20.0.0=interface=bridge1=actual-interface=bridge1=invalid=false=dynamic=false=disabled=false
!re=.id=*5=address=172.16.0.1/30=network=172.16.0.0=interface=bridge1=actual-interface=bridge1=invalid=false=dynamic=false=disabled=false
!re=.id=*6=address=172.19.1.19/19=network=172.19.0.0=interface=bridge1=actual-interface=bridge1=invalid=false=dynamic=false=disabled=false
```

To query data at API level:

Filtering at API level decreases network payload and improves the performance.

```cs
using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
     conn.Open();
     var cmd = conn.CreateCommand("ip address print where address=172.16.0.1/30");
     var result = cmd.ExecuteReader();
     foreach (var line in result)
          Console.WriteLine(line);

}
```

Result:

```
!re=.id=*5=address=172.16.0.1/30=network=172.16.0.0=interface=bridge1=actual-nterface=bridge1=invalid=false=dynamic=false=disabled=false
```

Or:

```cs
using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
conn.Open();
var cmd = conn.CreateCommand("ip address print");
var condition = new MKCommandParameterCollection()
{
     new MKCommandParameter("address","172.16.0.1/3"),
     new MKCommandParameter("address","172.19.1.19/19")
};

var result = cmd.ExecuteReader(queryConditions: condition, logic: MKQueryLogicOperators.Or);
foreach (var line in result)
     Console.WriteLine(line);

}


```

Result:
```
!re=.id=*5=address=172.16.0.1/30=network=172.16.0.0=interface=bridge1=actual-nterface=bridge1=invalid=false=dynamic=false=disabled=false
!re=.id=*6=address=172.19.1.19/19=network=172.19.0.0=interface=bridge1=actual-interface=bridge1=invalid=false=dynamic=false=disabled=false
```

To deserialize response: (ExecuteReader<T>)
```cs
class MyIpAddress
{
    public string MKID { get; set; }  //MKID alwase referce to .id field in response.
    public string Address { get; set; } // Use PascalCase naming style for properties. it will convert from/to kebab-case naming.
    public string Interface { get; set; }
}
//------------------------------------------------
using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
    conn.Open();
    var cmd = conn.CreateCommand("ip address print");
    var result = cmd.ExecuteReader<MyIpAddress>();

    foreach (var ip in result)
        Console.WriteLine($"{ip.MKID} - {ip.Address} - {ip.Interface}");

}

```
Result:
```
*4 - 10.20.1.19/16 - bridge1
*5 - 172.16.0.1/30 - bridge1
*6 - 172.19.1.19/19 - bridge1
```
>Note: Using method ExecuteReader<T> only reads the fields that are present is the given type, using the .proplist field in query.


To get dynamic object response: (ExecuteReaderDynamic)
you can get response object without defining any model class.

```cs

using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
    conn.Open();
    var cmd = conn.CreateCommand("ip address print");
    var result = cmd.ExecuteReaderDynamic();

    foreach (var ip in result)
        Console.WriteLine($"{ip.Id} - {ip.Address} - {ip.Interface}"); //MKID switched to Id
}

```
Result:
```
*4 - 10.20.1.19/16 - bridge1
*5 - 172.16.0.1/30 - bridge1
*6 - 172.19.1.19/19 - bridge1
```
>Note: Using method ExecuteReaderDynamic reads all fields from the router it will increase response payload.

Read data from background commands: (ExecuteBackground)
Some commands works in background ( like ping,bandwith test, discovert,...)

```cs
using (var conn = new MKConnection(IPADDRESS, USERNAME, PASSWORD))
{
    conn.Open();
    var cmd = conn.CreateCommand("ping");
    cmd.Parameters.Add("address", "10.20.0.4");
    cmd.Parameters.Add("count", "3");
    cmd.Parameters.Add("interval", "1");
    cmd.ExecuteBackground();

    Thread.Sleep(5000); 
    
    var result = cmd.ExecuteReaderDynamic();
    foreach (var ip in result)
        Console.WriteLine($"{ip.Host} - {ip.Time} - {ip.Ttl}");

}
```


Result:
```
10.20.0.4 - 13ms - 128
10.20.0.4 - 6ms - 128
10.20.0.4 - 4ms - 128
```

