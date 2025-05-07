using System;
using UnityEngine;

namespace UnityCommunity.UnitySingleton
{
    public class NativeServiceManager : Singleton<NativeServiceManager>
    {
        public AdvertisementSystem AdvertisementSystem
        {
            get;
            private set;
        }

        protected override void OnInitializing()
        {
            base.OnInitializing();

            if (AdvertisementSystem == null)
            {
                AdvertisementSystem = AdvertisementSystem.Instance;
                AdvertisementSystem.InitializeSingleton();
            }
        }
    }
}