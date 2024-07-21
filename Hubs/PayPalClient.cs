using EvoltingStore.Config;
using PayPalCheckoutSdk.Core;


namespace EvoltingStore.Hubs
{

    public class PayPalClient
    {
        public static PayPalEnvironment Environment()
        {
            return new SandboxEnvironment(PayPalConfig.ClientId, PayPalConfig.Secret);
        }

        public static PayPalHttpClient Client()
        {
            return new PayPalHttpClient(Environment());
        }
    }

}
