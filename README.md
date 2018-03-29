# MikrotikDotNet
MikrotikDotNet is a lightwaight and easy to use ADO.NET like library for Mikrotik Api with extensibility and performance in mind.

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
    cmd.Parameters.Add("name", "Test");
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

To deserialize response: (ExecuteReader<T>)
```cs
class MyIpAddress
{
    public string MKID { get; set; }  //MKID alwase referce to .id field in response.
    public string Address { get; set; }
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
>Note: Using method ExecuteReader<T> only reads the fields that are 


