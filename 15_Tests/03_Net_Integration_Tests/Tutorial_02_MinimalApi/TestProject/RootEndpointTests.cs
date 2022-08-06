using NUnit.Framework;

namespace TestProject
{
    internal class RootEndpointTests
    {
        private readonly ITestOutputHelper output;
        private readonly TestApplication app;

        public RootEndpointTests(ITestOutputHelper output)
        {
            this.output = output;
            app = new TestApplication();
        }

        [Test]
        public async Task Can_get_message()
        {
            app.Message = "test message";

            var client = app.CreateDefaultClient();
            var result = await client.GetStringAsync("/");

            output.WriteLine(result);
            Assert.Equal($"Khalid says \"{app.Message}\"", result);
        }
    }
}
