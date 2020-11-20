using Moq;
using NUnit.Framework;
using System.Security.Principal;

namespace Nop.Tests
{
    public abstract class TestsBase
    {
        protected MockRepository mocks;

        [SetUp]
        public virtual void SetUp()
        {
            mocks = new MockRepository(MockBehavior.Loose);
        }

        [TearDown]
        public virtual void TearDown()
        {
            mocks?.VerifyAll();
        }

        protected static IPrincipal CreatePrincipal(string name, params string[] roles)
        {
            return new GenericPrincipal(new GenericIdentity(name, "TestIdentity"), roles);
        }
    }
}
