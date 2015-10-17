using LinqJson.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace LinqJson.Service
{
    public class DataService
    {
        private ObservableCollection<Note> Notes;
        const string fileName = "notes.json";
    
        public DataService()
        {
            Notes = new ObservableCollection<Note>();
        }


        public async Task<ObservableCollection<Note>> GetNotes()
        {
            await getNoteDataAsync();
            return Notes;
        }

       

        private async Task getNoteDataAsync()
        {
            
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Note>));

            try
            {
                 
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    Notes = (ObservableCollection<Note>)jsonSerializer.ReadObject(stream);
                }
            }
            catch
            {
                Notes = new ObservableCollection<Note>();
            }
        }

        public async void AddNote(Note Note)
        {
            Notes.Add(Note);
            await saveNoteDataAsync();
        }

        public async void DeleteNote(Note Note)
        {
            Notes.Remove(Note);
            await saveNoteDataAsync();
        }

        public async void UpdateNote(Note Note)
        {
            var oldNote = from n in Notes
                          where n.ID == Note.ID
                          select n;
            Note note = oldNote.FirstOrDefault();
            Notes.Remove(note);
            Notes.Add(Note);
            await saveNoteDataAsync();
        }
        public Note QueryNoteByID(Guid id)
        {
            Note note;
            var oldNote = from n in Notes
                          where n.ID == id
                          select n;
            return  note = oldNote.FirstOrDefault();
        }

        private async Task saveNoteDataAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Note>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
                CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, Notes);
            }
        }



    }
    
}
