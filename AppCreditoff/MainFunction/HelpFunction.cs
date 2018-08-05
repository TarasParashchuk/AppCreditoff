using Android.App;
using Android.Content;
using AppCreditoff.Model;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.IO;

namespace AppCreditoff.MainFunction
{
    class HelpFunction
    {
        public bool CheckConnection()
        {
            if (CrossConnectivity.Current != null && CrossConnectivity.Current.ConnectionTypes != null && CrossConnectivity.Current.IsConnected == true)
                return true;
            else return false;
        }

        public List<Model_Creditoff> Open_Favorites()
        {
            var file_name = OpenActivity.file_name;

            if (File.Exists(file_name))
            {
                var response = File.ReadAllText(file_name);
                return JsonConvert.DeserializeObject<List<Model_Creditoff>>(response);
            }
            else return null;
        }

        public void ShowDialog(Context context, string title, string message, string PositiveButton, string NegativeButton)
        {
            var alert = new Android.Support.V7.App.AlertDialog.Builder(context);
            alert.SetTitle(title);
            alert.SetMessage(message);

            if (PositiveButton != string.Empty)
                alert.SetPositiveButton(PositiveButton, (s, e) => { });

            if (NegativeButton != string.Empty)
                alert.SetNegativeButton(NegativeButton, (s, e) => { });
            Dialog dialog = alert.Create();
            var _Activity = (Activity)context;
            _Activity.RunOnUiThread(() => { dialog.Show(); });
        }
    }
}