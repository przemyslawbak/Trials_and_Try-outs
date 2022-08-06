using NSubstitute;
using NUnit.Framework;
using System;
using Unit_Test_13___PresenterMock;

namespace Tests
{
    [TestFixture]
    public class EventRelatedTests
    {
        [Test]
        public void Ctor_WhenViewIsLoaded_CallsViewRender()
        {
            //konfiguracja
            var mockView = Substitute.For<IView>();
            Presenter p = new Presenter(mockView);
            //dzia�anie
            //Wyzwolenie zdarzenia za pomoc� frameworka NSubstitute
            mockView.Loaded += Raise.Event<Action>();
            //asercja
            //Sprawdzenie, czy wywo�ano widok
            mockView.Received().Render(Arg.Is<string>(s => s.Contains("Witaj, �wiecie")));
        }
    }
}