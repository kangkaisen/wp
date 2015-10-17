using LinqJson.Common;
using LinqJson.Model;
using LinqJson.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace LinqJson.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewOrEditView : Page
    {
      
        public NewOrEditView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           this.DataContext = NewOrEditView.NewOrEditViewModel;
      
        }

        private static NewOrEditViewModel newOrEditViewModel = null;
        public static NewOrEditViewModel NewOrEditViewModel
        {
            get
            {
                if (NavigationHelp.Parameter != null)
                {
                    newOrEditViewModel = new NewOrEditViewModel((Guid)NavigationHelp.Parameter);
                }

                else if (newOrEditViewModel == null || newOrEditViewModel.NoteDemo.ImagePath != null)
                {
                    newOrEditViewModel = new NewOrEditViewModel();
                }
                return newOrEditViewModel;
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.ContinuationData["Operation"] = "Image";
            openPicker.PickSingleFileAndContinue();
        }

        private FileOpenPickerContinuationEventArgs _filePickerEventArgs = null;
        public FileOpenPickerContinuationEventArgs FilePickerEvent
        {
            get { return _filePickerEventArgs; }
            set
            {
                _filePickerEventArgs = value;
                ContinueFileOpenPicker(_filePickerEventArgs);
            }
        }

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {


            if ((args.ContinuationData["Operation"] as string) == "Image" && args.Files != null && args.Files.Count > 0)
            {
                StorageFile inFile = args.Files[0];
                StorageFile imageFile = await App.ImageFolder.CreateFileAsync(((NewOrEditViewModel)DataContext).NoteDemo.ID.ToString() + ".jpg", CreationCollisionOption.ReplaceExisting);
                var inStream = await inFile.OpenAsync(FileAccessMode.Read);
                var outStream = await imageFile.OpenAsync(FileAccessMode.ReadWrite);
                outStream.Size = 0;
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(inStream);
                PixelDataProvider provider = await decoder.GetPixelDataAsync();
                byte[] data = provider.DetachPixelData();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, outStream);
                encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode,
                                                   decoder.PixelWidth, decoder.PixelHeight,
                                                   decoder.DpiX, decoder.DpiY, data
                    );

                try
                {
                    await encoder.FlushAsync();
                   ((NewOrEditViewModel)DataContext).NoteDemo.ImagePath = imageFile.Path;

                }
                catch (Exception err)
                {
                    Debug.WriteLine(err.ToString());
                }
                finally
                {
                    inStream.Dispose();
                    outStream.Dispose();
                }
            }
        }



       
    }
}
