using Microsoft.AspNetCore.Mvc.Testing;

namespace Test_eJobV30
{
    [TestClass]
    public class eJob_homepages
    {
        private HttpClient _httpClient;

        public eJob_homepages()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();

        }
        [TestMethod]
        public async Task open_HomeIndex_returnsOK()
        {
            var response = await _httpClient.GetAsync("");
            var stringResult = await response.Content.ReadAsStringAsync();
            var isHelloFound = stringResult.Contains("eJobv30") 
                && stringResult.Contains("Modules")
                && stringResult.Contains("Privacy");
            Assert.AreEqual(true, isHelloFound);
        }

        [TestMethod]
        public async Task ReportIndex_returnsOK()
        {
            var response = await _httpClient.GetAsync("Reports");
            var stringResult = await response.Content.ReadAsStringAsync();
            var isHelloFound = stringResult.Contains("Report List")
                && stringResult.Contains("TestReport")
                //&& stringResult.Contains("Privacy")
                ;
            Assert.AreEqual(true, isHelloFound);
        }

        


    }
}