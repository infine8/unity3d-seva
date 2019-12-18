using UnityEngine;
using System.Collections;

namespace UniKid.Core.Service
{
    public sealed class NetworkingCore
    {
        public static float PING_DELTA_TIME = 5.0f;

        public static bool IsThereConnection { get; private set; }

        private static readonly string VK_APP_ID = "3743223";

        public NetworkingCore()
        {
            CoreContext.StartCoroutine(TestConnection());

            InitSocialNetwork();
        }

        private void InitDbService()
        {
            //gameObject.AddComponent<Pa>()
        }

        private void InitSocialNetwork()
        {
            //VKontakteManager.Init(this, VK_APP_ID);
        }


        private IEnumerator TestConnection()
        {
            while (true)
            {
                var testPing = new WWW("http://www.google.com");

                yield return testPing;

                IsThereConnection = string.IsNullOrEmpty(testPing.error);

                if (IsThereConnection)
                {
                    var loc = testPing.responseHeaders["LOCATION"];
                    IsThereConnection = !string.IsNullOrEmpty(loc);
                    if (IsThereConnection) IsThereConnection = loc.StartsWith("http://www.google");
                }

                testPing.Dispose();

                yield return new WaitForSeconds(PING_DELTA_TIME);
            }
        }
    }


}
