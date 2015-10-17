using LinqJson.Common;
using LinqJson.Model;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;




// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LinqJson.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OneDriveView : Page
    {

        public const string fileName = "notes.json";
        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Note>));
        private ObservableCollection<Note> Notes;
        public OneDriveView()
        {
            this.InitializeComponent();

        }
        private async void loginButton_Click(object sender, RoutedEventArgs e)//登录
        {
            try
            {
                msgText.Text = "亲：正在登录";
                var authClient = new LiveAuthClient();

                LiveLoginResult result = await authClient.LoginAsync(new string[] { "wl.signin", "wl.skydrive" });

                if (result.Status == LiveConnectSessionStatus.Connected)
                {

                    var connectClient = new LiveConnectClient(result.Session);
                    var meResult = await connectClient.GetAsync("me");
                    msgText.Text = "亲爱的：" + meResult.Result["name"].ToString() + "  您已成功登录OneDrive!";
                }
                updateButton.Visibility = Visibility.Visible;
                uploadButton.Visibility = Visibility.Visible;
                downButton.Visibility = Visibility.Visible;


            }
            catch (LiveAuthException ex)
            {
                msgText.Text = ex.Message;
            }
            catch (LiveConnectException ex)
            {
                msgText.Text = ex.Message;
            }
        }

        private async void uploadButton_Click(object sender, RoutedEventArgs e)//上传
        {
            try
            {
                msgText.Text = "亲：正在上传";
                var authClient = new LiveAuthClient();
                var authResult = await authClient.LoginAsync(new string[] { "wl.skydrive", "wl.skydrive_update" });
                if (authResult.Session != null)
                {
                    var liveConnectClient = new LiveConnectClient(authResult.Session);

                    StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                    //String fileText = await FileIO.ReadTextAsync(file);

                    LiveUploadOperation uploadOperation = await liveConnectClient.CreateBackgroundUploadAsync(
                        "me/skydrive", file.Name, file, OverwriteOption.Overwrite);
                    LiveOperationResult uploadResult = await uploadOperation.StartAsync();

                    if (uploadResult.Result != null)
                    {
                        msgText.Text = "恭喜：您已成功同步记事至OneDrive!";
                    }

                }
            }
            catch (LiveAuthException ex)
            {
                msgText.Text = ex.Message;
            }
            catch (LiveConnectException ex)
            {
                msgText.Text = ex.Message;
            }

        }

        private async void downButton_Click_1(object sender, RoutedEventArgs e)//下载
        {
            try
            {
                msgText.Text = "亲：正在下载";
                string id = string.Empty;
                var authClient = new LiveAuthClient();
                var authResult = await authClient.LoginAsync(new string[] { "wl.skydrive" });
                if (authResult.Session != null)
                {
                    var connectClient = new LiveConnectClient(authResult.Session);
                    LiveOperationResult operationResult = await connectClient.GetAsync("me/skydrive/search?q=notes.json");
                    List<object> items = operationResult.Result["data"] as List<object>;
                    IDictionary<string, object> file = items.First() as IDictionary<string, object>;
                    id = file["id"].ToString();
                    LiveDownloadOperation operation = await connectClient.CreateBackgroundDownloadAsync(string.Format("{0}/content", id));
                    var result = await operation.StartAsync();
                    Stream stream = result.Stream.AsStreamForRead();
                    Notes = (ObservableCollection<Note>)jsonSerializer.ReadObject(stream);
                    StorageFile files = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                    using (var streamJson = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
                        CreationCollisionOption.ReplaceExisting))
                    {
                        jsonSerializer.WriteObject(streamJson, Notes);
                    }
                    msgText.Text = "恭喜：您已成功从OneDrive下载记事!";
                    
                    Frame.Navigate(typeof(MainPage));

                }
            }
            catch (Exception ex)
            {
                msgText.Text = ex.Message;
            }
        }
       
       
    }
}
