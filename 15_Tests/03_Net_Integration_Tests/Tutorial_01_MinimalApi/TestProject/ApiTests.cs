using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    internal class ApiTests
    {
        private HttpClient _httpClient;

        public ApiTests()
        {
            /*Now let’s start with the implementation of the test method.We create an instance of the WebApplicationFactory type. 
             * It’s a generic type, and we provide the Program class from the API project as its type argument.*/
            var webAppFactory = new WebApplicationFactory<Program>();

            /*Next, we create an httpClient variable of type HttpClient using the CreateDefaultClient method of the webAppFactory variable.
             * This call provides us with an HTTP client that we can use to send HTTP requests to the server running in the test execution.*/
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [Test]
        public async Task DefaultRoute_ReturnsHelloWorld()
        {
            //Next, we use the GetAsync method on the httpClient type and provide an empty string to specify that we want to call the default route.
            var response = await _httpClient.GetAsync("");

            //Next, we access the response and use the ReadAsStringAsync method on the Content property of the response object to retrieve the data returned from the API.
            var stringResult = await response.Content.ReadAsStringAsync();
            //Check the content of the stringResult variable against our expected “Hello World!” string.
            Assert.That(stringResult, Is.EqualTo("Hello World!"));
        }
    }
}
