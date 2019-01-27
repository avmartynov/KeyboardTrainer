using NUnit.Framework;
using Twidlle.Infrastructure.Diagnostics;
using Twidlle.Infrastructure.Testing;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Tests
{
    public class Smoke_TestFixture
    {
        [Test]
        [Category("Long")]
        public void Test_Smoke()
            => _facade.Execute(() => WinAppControl.StartWaitFinishConfigured(waitSeconds: 5));

        private static readonly TestFacade _facade = TestFacade.GetCurrentClassFacade();
    }
}
