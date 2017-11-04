using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using Xunit;

namespace AspectCore.Tests.DynamicProxy.Addons
{
    public class InterfaceProxyGenerator: DynamicProxyTestBase
    {
        [Fact]
        public void CreateInterfaceProxy_Without_Impl()
        {
            var serviceProxy = ProxyGenerator.CreateInterfaceProxy<IMethodTestService>();
            Assert.NotNull(serviceProxy);
            var name = serviceProxy.GetName();
            Assert.Equal("Your Name", name);
        }


        protected override void Configure(IAspectConfiguration configuration)
        {
            configuration.Interceptors.AddDelegate((ctx, next) => next(ctx), Predicates.ForService("IService"));

            configuration.Interceptors.AddDelegate(async (ctx, next) =>
            {
                await next(ctx);
                ctx.ReturnValue = "Your Name";
            }
            , Predicates.ForMethod("*Name"));
        }


    }

    public interface IMethodTestService
    {
        string GetName();
    }
}
