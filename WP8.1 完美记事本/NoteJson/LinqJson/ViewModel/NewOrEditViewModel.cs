using LinqJson.Command;
using LinqJson.Common;
using LinqJson.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Activation;

namespace LinqJson.ViewModel
{
    public class NewOrEditViewModel : ModelBase
    {
        private Note notedemo;

        public Note NoteDemo
        {
            get { return notedemo; }
            set
            { this.SetProperty(ref this.notedemo, value); }
        }

      

        public string  Title { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

    


        public NewOrEditViewModel(Guid id)
        {

            NoteDemo=App.DataModel.QueryNoteByID(id);
            Title = "编辑记事";
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Modify);
        
              
        }

      

        public NewOrEditViewModel()
        {
            NoteDemo = new Note();
            Title = "新建记事";
            SaveCommand = new DelegateCommand(Add);
            CancelCommand = new DelegateCommand(Cancel);
         
         }


  

        private void Modify()
     {
         
         App.DataModel.UpdateNote(NoteDemo);
         NavigationHelp.NavigateTo( typeof(MainPage));
     }
        private void Add()
        {
           
           App.DataModel.AddNote(NoteDemo);
           NavigationHelp.NavigateTo(typeof(MainPage));
        }

        private void Cancel()
        {
            NavigationHelp.NavigateTo(typeof(MainPage));
        }

        //private void TapChanged()
        //{
        //    FileOpenPicker openPicker = new FileOpenPicker();
        //    openPicker.FileTypeFilter.Add(".jpg");
        //    openPicker.ContinuationData["Operation"] = "Image";
        //    openPicker.PickSingleFileAndContinue();
        //}

        //private FileOpenPickerContinuationEventArgs _filePickerEventArgs = null;
        //public FileOpenPickerContinuationEventArgs FilePickerEvent
        //{
        //    get { return _filePickerEventArgs; }
        //    set
        //    {
        //        _filePickerEventArgs = value;
        //        ContinueFileOpenPicker(_filePickerEventArgs);
        //    }
        //}
        //public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        //{

        //    if ((args.ContinuationData["Operation"] as string) == "Image" && args.Files != null && args.Files.Count > 0)
        //    {
        //        StorageFile inFile = args.Files[0];
        //        StorageFile imageFile = await App.ImageFolder.CreateFileAsync(NoteDemo.ID.ToString() + ".jpg", CreationCollisionOption.ReplaceExisting);
        //        var inStream = await inFile.OpenAsync(FileAccessMode.Read);
        //        var outStream = await imageFile.OpenAsync(FileAccessMode.ReadWrite);
        //        outStream.Size = 0;
        //        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(inStream);
        //        PixelDataProvider provider = await decoder.GetPixelDataAsync();
        //        byte[] data = provider.DetachPixelData();
        //        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, outStream);
        //        encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode,
        //                                           decoder.PixelWidth, decoder.PixelHeight,
        //                                           decoder.DpiX, decoder.DpiY, data
        //            );

        //        try
        //        {
        //            await encoder.FlushAsync();
        //            NoteDemo.ImagePath = imageFile.Path;

        //        }
        //        catch (Exception err)
        //        {
        //            Debug.WriteLine(err.ToString());
        //        }
        //        finally
        //        {
        //            inStream.Dispose();
        //            outStream.Dispose();
        //        }
        //    }
        //}

        
       
    }
}
