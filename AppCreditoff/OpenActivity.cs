using System.IO;
using Android.OS;
using Android.App;
using Android.Content.PM;
using System.Threading.Tasks;
using AppCreditoff.MainFunction;

namespace AppCreditoff
{
    [Activity(Theme = "@style/Theme.Splash",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        NoHistory = false)]
    public class OpenActivity : Activity
    {
        static public string file_name = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Creditoff_10.json");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Task.Run(() =>
            {
                if (new HelpFunction().CheckConnection() && !File.Exists(file_name))
                    new MainOperation().Updated();
            }).Wait();
            StartActivity(typeof(MainActivity));
        }
    }
}