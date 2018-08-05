using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using Android.App;
using FFImageLoading;
using AppCreditoff.Model;

namespace AppCreditoff
{
    public class ViewHolder : Java.Lang.Object
    {
        internal FFImageLoading.Views.ImageViewAsync image_coins;
        internal FFImageLoading.Views.ImageViewAsync image_count_star;
        internal TextView text_name;
        internal TextView text_suma;
        internal TextView text_time_check;
        internal TextView text_time_credit;
        internal TextView text_pereplata;
        internal TextView text_information;
        internal CustomButton ButtonCredit;
    }

    class AdapterListCreditoff : BaseAdapter<Model_Creditoff>
    {
        private List<Model_Creditoff> data;
        private int index_layout;
        private Context context;
        private ViewHolder viewHolder;

        public AdapterListCreditoff(Context context, int index_layout, List<Model_Creditoff> data)
        {
            this.data = data;
            this.index_layout = index_layout;
            this.context = context;
        }

        public override int Count
        {
            get
            {
                return data.Count;
            }
        }

        public override Model_Creditoff this[int position]
        {
            get
            {
                return data[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var item = data[position];

            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(index_layout, parent, false);
                viewHolder = new ViewHolder();

                viewHolder.image_coins = view.FindViewById<FFImageLoading.Views.ImageViewAsync>(Resource.Id.ImageCoins);
                viewHolder.image_count_star = view.FindViewById<FFImageLoading.Views.ImageViewAsync>(Resource.Id.logo_count_star);
                viewHolder.text_name = view.FindViewById<TextView>(Resource.Id.name);
                viewHolder.text_suma = view.FindViewById<TextView>(Resource.Id.suma);
                viewHolder.text_time_check = view.FindViewById<TextView>(Resource.Id.time_check);
                viewHolder.text_time_credit = view.FindViewById<TextView>(Resource.Id.time_credit);
                viewHolder.text_pereplata = view.FindViewById<TextView>(Resource.Id.pereplata);
                viewHolder.text_information = view.FindViewById<TextView>(Resource.Id.information);
                viewHolder.ButtonCredit = view.FindViewById<CustomButton>(Resource.Id.ButtonCredit);
                viewHolder.ButtonCredit.Click += ButtonCreditClick;

                view.Tag = viewHolder;
            }
            else viewHolder = (ViewHolder)view.Tag;

            ImageService.Instance.LoadUrl(item.logo).WithCache(FFImageLoading.Cache.CacheType.All).Into(viewHolder.image_coins);
            viewHolder.image_count_star.SetImageResource(2130903041 + item.logo_count_star);

            viewHolder.text_name.Text = item.name;
            viewHolder.text_suma.Text = item.suma + " грн.";

            viewHolder.text_time_check.Text = item.time_check;
            viewHolder.text_time_credit.Text = item.time_credit;

            if (item.pereplata != null)
                viewHolder.text_pereplata.Text = item.pereplata;
            else viewHolder.text_pereplata.Text = "Нет информации";

            if (item.information != null)
                viewHolder.text_information.Text = item.information;
            else viewHolder.text_information.Text = "Нет информации";

            viewHolder.ButtonCredit.Id_Item = item.id;
            return view;
        }

        private void ButtonCreditClick(object sender, EventArgs args)
        {
            var button = (CustomButton)sender;
            var item = data.Where(u => u.id == button.Id_Item).First();
            var _Activity = (Activity)context;

            if (item.url == null) return;
            else _Activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse(item.url)));
        }
    }
}