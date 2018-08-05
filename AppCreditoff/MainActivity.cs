using System.IO;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Threading;
using AppCreditoff.Model;
using Android.Support.V7.App;
using AppCreditoff.MainFunction;
using System.Collections.Generic;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Threading.Tasks;
using Firebase.Iid;
using Android.Util;

namespace AppCreditoff
{
    [Activity(Label = "AppCreditoff", MainLauncher = false, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        private MainOperation _MainOperation = new MainOperation();
        private HelpFunction helpFunction = new HelpFunction();
        private AdapterListCreditoff _AdapterListCreditoff;
        private ListView listview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!GetString(Resource.String.google_app_id).Equals("1:504757005184:android:9b96aa6679eeff18"))
                throw new System.Exception("Invalid Json file");

            Task.Run(() =>
            {
                var instanceId = FirebaseInstanceId.Instance;
                var r = instanceId.Token;
                instanceId.DeleteInstanceId();
                Log.Debug("MainActivity", "{0} {1}", instanceId.Token, instanceId.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope));
            });
            
            SetContentView(Resource.Layout.Main);

            listview = FindViewById<ListView>(Resource.Id.ListCreditoff);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = string.Empty;
 
            try
            {
                if (File.Exists(OpenActivity.file_name))
                    _AdapterListCreditoff = new AdapterListCreditoff(this, Resource.Layout.ItemList, helpFunction.Open_Favorites());
                else _AdapterListCreditoff = null;
                listview.Adapter = _AdapterListCreditoff;
            }
            catch
            {
                helpFunction.ShowDialog(this, "Нет доступа к интернету", "Для включения мобильного интернета зайдите в настройки и подключите мобильною сеть или Wi-fi.", "OK", string.Empty);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        private List<Model_Creditoff> GetDataThread()
        {
            List<Model_Creditoff> list;
            try
            {
                File.Delete(OpenActivity.file_name);
                _MainOperation.Updated();
                list = helpFunction.Open_Favorites();
            }
            catch
            {
                list = null;
            }
            return list;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            var str_message = string.Empty;

            if (id == Resource.Id.menu_update)
            {
                if (helpFunction.CheckConnection())
                {
                    var progressDialog = ProgressDialog.Show(this, null, "Ожидайте обновление списка компаний...", true);

                    new Thread(new ThreadStart(delegate
                    {
                        var list = GetDataThread();
                        RunOnUiThread(() => Toast.MakeText(this, "Список компаний обновлен", ToastLength.Long).Show());
                        RunOnUiThread(() => progressDialog.Hide());
                        RunOnUiThread(() => listview.Adapter = new AdapterListCreditoff(this, Resource.Layout.ItemList, list));
                    })).Start();
                }
                else helpFunction.ShowDialog(this, "Нет доступа к интернету", "Для включения мобильного интернета зайдите в настройки и подключите мобильною сеть или Wi-fi.", "OK", string.Empty);
            }
            else
            {
                if (helpFunction.Open_Favorites() != null)
                {
                    if (id == Resource.Id.menu_summ)
                        _AdapterListCreditoff = new AdapterListCreditoff(this, Resource.Layout.ItemList, _MainOperation.Sort_Sum());
                    else if (id == Resource.Id.menu_time)
                        _AdapterListCreditoff = new AdapterListCreditoff(this, Resource.Layout.ItemList, _MainOperation.Sort_Time());
                    else if (id == Resource.Id.menu_mark)
                        _AdapterListCreditoff = new AdapterListCreditoff(this, Resource.Layout.ItemList, _MainOperation.Sort_Rating());
                    Toast.MakeText(this, "Фильтрация по: " + item.TitleFormatted, ToastLength.Short).Show();
                    listview.Adapter = _AdapterListCreditoff;
                }
                else Toast.MakeText(this, "Обновите список компаний", ToastLength.Short).Show();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
