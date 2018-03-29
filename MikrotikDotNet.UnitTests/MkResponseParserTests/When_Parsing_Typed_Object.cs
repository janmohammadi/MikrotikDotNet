using MikrotikDotNet.ReponseParser;
using Xunit;

namespace MikrotikDotNet.UnitTests.MkResponseParserTests
{
    public class When_Parsing_Typed_Object : Given_Response_Parser
    {

        class TestModel
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }

        }


        [Fact]
        public void Then_Ignores_Fields_That_Are_Not_Avaliable_In_Object_Type()
        {
            var responseText = "!re=a=a=b=b=x=x";
            var res = MKResponseParser.GetObject<TestModel>(responseText);

            Assert.Equal("a", res.A);
            Assert.Equal("b", res.B);
            Assert.Equal(null, res.C);
        }

        [Fact]
        public void Then_Ignores_Properties_That_Are_Not_Avaliable_In_Input_String()
        {
            var responseText = "!re=a=a=b=b";
            var res = MKResponseParser.GetObject<TestModel>(responseText);

            Assert.Equal("a", res.A);
            Assert.Equal("b", res.B);
            Assert.Equal(null, res.C);
        }


        [Fact]
        public void Then_Maps_Properites_That_Are_Avaliable()
        {
            var responseText = "!re=a=a=b=b=c=c";
            var res = MKResponseParser.GetObject<TestModel>(responseText);

            Assert.Equal("a",res.A);
            Assert.Equal("b",res.B);
            Assert.Equal("c",res.C);

        }

    }
}