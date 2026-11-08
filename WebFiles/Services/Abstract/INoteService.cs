using WebFiles.Domain.Entities;

namespace WebFiles.Services.Abstract;

public interface INoteService
{
    public Task AddNoteAsync(string name, string title, string text, bool isPublic, string userId);
    public Note GetNote(int id); //Get public note(if user isn't authenticated)
    public Note GetNote(int id, string userId); //Get all types of notes(if user is authenticated)
    public Task DeleteNoteAsync(int id, string userId, bool forced = false); //Forced s for removing notes from admin panel
    public List<Note> GetUserNotes(string userId); //For account page
    public Task UpdateNoteAsync(int id, string name, string title, string text, bool isPublic, string userId);
    public Task DeleteUserNotesAsync(string userId);
}