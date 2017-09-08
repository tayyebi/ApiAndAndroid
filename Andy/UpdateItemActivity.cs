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
using System.Net;
using System.Collections.Specialized;
using System.IO;

namespace Andy
{
    [Activity(Label = "UpdateItemActivity")]
    public class UpdateItemActivity : Activity
    {

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                imgBtn.SetImageURI(data.Data);
                var bmp = Android.Provider.MediaStore.Images.Media.GetBitmap(ContentResolver, data.Data);
                using (var stream = new MemoryStream())
                {
                    bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 10, stream);
                    SelectedImage = stream.ToArray();
                }
            }
        }

        EditText et1, et2;
        ImageButton imgBtn;
        byte[] SelectedImage = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UpdateItem);

            var id = Intent.GetIntExtra("Id", 0);

            et1 = FindViewById<EditText>(Resource.Id.editText1);
            et2 = FindViewById<EditText>(Resource.Id.editText2);
            imgBtn = FindViewById<ImageButton>(Resource.Id.imageButton1);

            et1.Text = Intent.GetStringExtra("Name");
            et2.Text = Intent.GetStringExtra("Description");

            imgBtn.Click += delegate
            {
                var ofd = new Intent(Intent.ActionGetContent);
                ofd.SetType("image/*");
                StartActivityForResult(ofd, 1);
            };

            FindViewById<Button>(Resource.Id.button1).Click += delegate
            {
                using (var client = new WebClient())
                {
                    var nvc = new NameValueCollection();
                    nvc.Add("Name", et1.Text);
                    nvc.Add("Description", et2.Text);

                    if (SelectedImage != null)
                    {
                        nvc.Add("ContentType", "image/jpeg");
                        nvc.Add("Base64Image", Convert.ToBase64String(SelectedImage));
                    }
                    client.UploadValues(ConnectionInfo.PeopleController + "/" + id.ToString(), "PUT", nvc);
                    Finish();
                }
            };
        }
    }
}