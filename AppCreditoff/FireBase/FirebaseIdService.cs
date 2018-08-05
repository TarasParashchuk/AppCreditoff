using Android.App;
using Android.Content;
using Firebase.Iid;

namespace AppCreditoff
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class FirebaseIdService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            base.OnTokenRefresh();
            Android.Util.Log.Debug("MainActivity", "Refreshed Token:", FirebaseInstanceId.Instance.Token);
        }
    }
}