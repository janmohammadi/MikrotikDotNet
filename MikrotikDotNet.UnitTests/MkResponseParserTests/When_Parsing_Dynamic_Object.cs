using MikrotikDotNet.ReponseParser;
using Xunit;

namespace MikrotikDotNet.UnitTests.MkResponseParserTests
{
    public class When_Parsing_Dynamic_Object : Given_Response_Parser
    {
     
       

        [Fact]
        public void Then_Can_Return_Correct_Response()
        {
            var row =
                "!re=.id=*1FE=name=NormalUser=max-mtu=1480=max-mru=1480=mrru=disabled=interface=ether1=user==password=123=profile=default=keepalive-timeout=60=service-name==ac-name==add-default-route=false=dial-on-demand=false=use-peer-dns=false=allow=pap,chap,mschap1,mschap2=running=false=disabled=true";

            dynamic response = MKResponseParser.GetDynamicObject(row);

            Assert.Equal(response.Id, "*1FE");
            Assert.Equal(response.Name, "NormalUser");
            Assert.Equal(response.Disabled, "true");
            Assert.Equal(response.Running, "false");
            Assert.Equal(response.KeepaliveTimeout, "60");
        }
    }
}