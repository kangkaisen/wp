using LinqJson.Command;
using LinqJson.Common;
using LinqJson.Model;
using LinqJson.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace LinqJson.ViewModel
{
    public class MainPageViewModel:ModelBase
    {
        private ObservableCollection<Note> notelist;
        public ObservableCollection<Note> Notelist
        {
            get
            {
                return notelist;
            }
            set
            { this.SetProperty(ref this.notelist, value); }
        }

        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }


        public MainPageViewModel()
        {
            GetList();
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand(Delete);
            EditCommand = new DelegateCommand(Edit);
        }

        private async void GetList()
        {
               Notelist = await App.DataModel.GetNotes();
        }
        private void Edit(Object paramater)
        {
            Guid id = (Guid)paramater;
            NavigationHelp.NavigateTo(typeof(NewOrEditView), id);
        }


        private void Delete(Object paramater)
        {
            Guid id = (Guid)paramater;
            Note note = App.DataModel.QueryNoteByID(id);
            App.DataModel.DeleteNote(note);
            Notelist.Remove(note);
           
          
        }

        private void Add(Object paramater)
        {
            NavigationHelp.NavigateTo(typeof(NewOrEditView));
        }
    }
}
