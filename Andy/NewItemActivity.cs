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
using Java.IO;
using System.IO;
using Android.Graphics;
using Android.Provider;
using System.Net.Http;
using System.Threading.Tasks;


namespace Andy
{
    [Activity(Label = "NewItemActivity")]
    public class NewItemActivity : Activity
    {
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                imgButton.SetImageURI(data.Data);

                Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
                var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                SelectedImage = stream.ToArray();
            }
        }
        ImageButton imgButton;
        byte[] SelectedImage;

        /*
        private async Task Post()
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var values = new[]
                    {
                            new KeyValuePair<string, string>("Name", tv1.Text),
                            new KeyValuePair<string, string>("Description", tv3.Text),
                        };
                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }
                    if (SelectedImage != null)
                    {
                        var filecontent = new ByteArrayContent(SelectedImage);
                        filecontent.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachement")
                        {
                            FileName = "img.jpeg"
                        };
                        content.Add(filecontent, "Image", "img.jpeg");
                    }

                    await client.PostAsync(ConnectionInfo.PeopleController, content);

                };
            }
        }
        */

        private EditText tv1;
        private EditText tv3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewItem);

            imgButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            imgButton.Click += delegate
            {
                var ofd = new Intent(Intent.ActionGetContent);
                ofd.SetType("image/*");
                StartActivityForResult(ofd, 1);
            };
            FindViewById<Button>(Resource.Id.button1).Click += delegate
            {
                tv1 = FindViewById<EditText>(Resource.Id.editText1);
                tv3 = FindViewById<EditText>(Resource.Id.editText3);

                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    /*
                    var client = new HttpClient();
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(tv1.Text), "Name", "Name");
                    formData.Add(new StringContent(tv1.Text), "Description", "Description");
                    if (SelectedImage != null)
                    {
                        formData.Add(new ByteArrayContent(SelectedImage), "Image", "img.jpg");
                    }
                    var response = client.PostAsync(ConnectionInfo.PeopleController, formData).Result;
                    */
                    using (var client = new WebClient())
                    {
                        var nvc = new NameValueCollection();
                        nvc.Add("Name1", tv1.Text);
                        nvc.Add("Description", tv3.Text);
                        nvc.Add("ContentType", "image/jpg");
                        if (SelectedImage != null)
                            nvc.Add("Base64Image", Convert.ToBase64String(SelectedImage));
                        client.UploadValues(ConnectionInfo.PeopleController, nvc);
                    }
                    Finish();
                })).Start();





                /*
                var client = new WebClient();

                //string information = "Name=Test&Description=TEETTEEETEST";
                //byte[] data = Encoding.UTF8.GetBytes(information);
                //client.UploadData(ConnectionInfo.PeopleController, "POST", data);

                var nvc = new NameValueCollection();
                nvc.Add("Name1", tv1.Text);
                nvc.Add("Description", tv3.Text);
                nvc.Add("ContentType", "image/jpeg");
                nvc.Add("Base64Image", Convert.ToBase64String(SelectedImage));
                //if (SelectedImage != null)
                //    nvc.Add("Image", Convert.ToBase64String(SelectedImage));
                client.UploadValues(ConnectionInfo.PeopleController, nvc);

                //client.QueryString = nvc;
                //client.UploadData(ConnectionInfo.PeopleController, SelectedImage);

                Toast.MakeText(this, "Done!", ToastLength.Long).Show();
                Finish();
                */

                /*
                var client = new HttpClient();
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(new MemoryStream(SelectedImage)), "Image", "img.jpeg");
                content.Add(new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(tv1.Text))), "Name");
                content.Add(new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(tv3.Text))), "Description");
                var result = client.PostAsync(ConnectionInfo.PeopleController, content);
                */
            };
        }
    }
}
