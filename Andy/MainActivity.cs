using Android.App;
using System;
using Android.Widget;
using Android.OS;
using System.Json;
using System.Net;
using System.Collections.Generic;

namespace Andy
{
    [Activity(Label = "Andy", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.button1).Click += delegate
            {
                StartActivity(typeof(NewItemActivity));
            };

            var people = new List<Models.Person>();
            var client = new WebClient();
            var response =
                client.DownloadString(ConnectionInfo.PeopleController);
            int count = JsonValue.Parse(response).Count;
            for (int i = 0; i < count; i++)
            {
                var p = new Models.Person();
                var objj = JsonValue.Parse(response)[i];
                p.Name = (objj["Name"] ?? "").ToString();
                p.Description = (objj["Description"] ?? "").ToString();
                p.Id = Convert.ToInt32(objj["Id"].ToString());
                var bas = objj["Image"];
                if (bas != null)
                {
                    p.Image = Convert.FromBase64String(objj["Image"]);
                    p.ContentType = objj["ContentType"].ToString();
                }
                people.Add(p);
            }
            ListView lv = FindViewById<ListView>(Resource.Id.listView1);
            var adapter = new MainAdapter(this, people);
            lv.Adapter = adapter;
        }
    }
}