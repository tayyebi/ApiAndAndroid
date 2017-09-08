using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Andy
{
    class MainAdapter : BaseAdapter
    {

        Activity context;
        List<Models.Person> data;

        public MainAdapter(Activity context, List<Models.Person> data)
        {
            this.context = context;
            this.data = data;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
                convertView = context.LayoutInflater.Inflate(Resource.Layout.MainItem, null);

            var tv1 = convertView.FindViewById<TextView>(Resource.Id.textView1);
            var tv2 = convertView.FindViewById<TextView>(Resource.Id.textView2);
            var img = convertView.FindViewById<ImageView>(Resource.Id.imageView1);
            var btn1 = convertView.FindViewById<Button>(Resource.Id.button1);
            var btn2 = convertView.FindViewById<Button>(Resource.Id.button2);

            tv1.Text = data[position].Name;
            tv2.Text = data[position].Description;
            if (data[position].Image != null)
            {
                var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data[position].Image, 0, data[position].Image.Length);
                img.SetImageBitmap(bitmap);
            }
            else
            {
                img.Visibility = ViewStates.Gone;
            }
            // Update
            btn1.Click += delegate
            {
                var i = new Intent(context, typeof(UpdateItemActivity));

                i.PutExtra("Id", data[position].Id);
                i.PutExtra("Name", data[position].Name);
                i.PutExtra("Description", data[position].Description);

                context.StartActivity(i);

            };
            // Delete
            btn2.Click += delegate
            {
                var builder = new AlertDialog.Builder(context);
                builder.SetMessage("Are you sure to delete?");
                builder.SetPositiveButton("OK", (sender, args) =>
                {
                    // String a =  System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes("password here")).Aggregate("", (c, n) => c + n);
                    using (var client = new System.Net.WebClient())
                    {
                        client.UploadString(ConnectionInfo.PeopleController + "/" + data[position].Id.ToString(), "DELETE", string.Empty);
                    }

                });
                builder.SetNegativeButton("Cancel", (sender, args) =>
                {

                });
                builder.Create().Show();
            };

            return convertView;

        }
        public override int Count
        {
            get
            {
                return data.Count;
            }
        }

    }

    class MainAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}