using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace Dictionary
{
  
    public sealed partial class OneDrive : Page
    {
        public OneDrive()
        {
            this.InitializeComponent();
        }

       
       

        private  async void loginButton_Click(object sender, RoutedEventArgs e)//登录
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

        private  async void uploadButton_Click(object sender, RoutedEventArgs e)//上传
        {
            try
            {
                msgText.Text = "亲：正在上传";
                var authClient = new LiveAuthClient();
                var authResult = await authClient.LoginAsync(new string[] { "wl.skydrive", "wl.skydrive_update" });
                if (authResult.Session != null)
                {
                    var liveConnectClient = new LiveConnectClient(authResult.Session);
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile file = await localFolder.GetFileAsync(App.WordBookFileName);
                    String fileText = await FileIO.ReadTextAsync(file);

                    LiveUploadOperation uploadOperation = await liveConnectClient.CreateBackgroundUploadAsync(
                        "me/skydrive", file.Name, file, OverwriteOption.Overwrite);
                    LiveOperationResult uploadResult = await uploadOperation.StartAsync();

                    if (uploadResult.Result != null)
                    {
                        msgText.Text = "恭喜：您已成功同步生词至OneDrive!";
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

        private async  void downButton_Click_1(object sender, RoutedEventArgs e)//下载
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




                    LiveOperationResult operationResult = await connectClient.GetAsync("me/skydrive/search?q=WorkBook.xml");
                    List<object> items = operationResult.Result["data"] as List<object>;
                    IDictionary<string, object> file = items.First() as IDictionary<string, object>;
                    id = file["id"].ToString();

                    LiveDownloadOperation operation = await connectClient.CreateBackgroundDownloadAsync(string.Format("{0}/content", id));
                    var result = await operation.StartAsync();
                    Stream stream = result.Stream.AsStreamForRead();

                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile files = await localFolder.GetFileAsync(App.WordBookFileName);

                    XDocument xBook = XDocument.Load(stream);
                    await FileIO.WriteTextAsync(files, xBook.ToString());

                    msgText.Text = "恭喜：您已成功从OneDrive下载生词!";


                }
            }
            catch (Exception ex)
            {
                msgText.Text = ex.Message;
            }
        }

       
       
    }
}
