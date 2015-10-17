using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw
{
    public class DrawListViewModel:ModelBase
    {
        //  private ObservableCollection<Note> notelist;
        //public ObservableCollection<Note> Notelist
        //{
        //    get
        //    {
        //        return notelist;
        //    }
        //    set
        //    { this.SetProperty(ref this.notelist, value); }
        //}

        //public DelegateCommand DeleteCommand { get; set; }
        //public DelegateCommand EditCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }


        public DrawListViewModel()
        {
            //GetList();
            AddCommand = new DelegateCommand(Add);
            //    DeleteCommand = new DelegateCommand(Delete);
            //    EditCommand = new DelegateCommand(Edit);
            //}
        }
        //private async void GetList()
        //{
        //    Notelist = await DataService.Current.GetNotes();
        //}


   
        //private void Edit(Object paramater)
        //{
        //    Guid id = (Guid)paramater;
        //    NavigationHelp.NavigateTo(typeof(MainPage), id);
        //}


        //private void Delete(Object paramater)
        //{
        //    Guid id = (Guid)paramater;
        //    Note note = DataService.Current.QueryNoteByID(id);
        //    DataService.Current.DeleteNote(note);
        //    Notelist.Remove(note);


        //}

        private void Add(Object paramater)
        {
            NavigationHelp.NavigateTo(typeof(MainPage));
        }
    }
}
