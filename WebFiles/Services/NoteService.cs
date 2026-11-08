using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Services;

public class NoteService : INoteService
{
    private readonly ApplicationDbContext _context;

    public NoteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task
        AddNoteAsync(string name, string title, string text, bool isPublic,
            string userId) //I know, that is bad practice
    {
        var note = new Note(name, title, text, isPublic, userId);
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNoteAsync(int id, string name, string title, string text, bool isPublic, string userId)
    {
        var notesRes = from n in _context.Notes
            where n.UserId.Equals(userId) && n.Id == id
            select n;
        if (!notesRes.Any())
            throw new Exception("This note doesn't exists.");

        var note = notesRes.FirstOrDefault();
        
        note.SetName(name);
        note.SetText(text);
        note.SetTitle(title);
        note.SetAvailability(isPublic);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserNotesAsync(string userId)
    {
        var userNotes = (from n in _context.Notes
            where n.UserId.Equals(userId)
            select n).ToList();

        foreach (var note in userNotes)
        {
            await DeleteNoteAsync(note.Id, userId, true);
        }

        await _context.SaveChangesAsync();
    }

    public Note GetNote(int id)
    {
        var res = from n in _context.Notes
            where n.IsPublic && n.Id == id
            select n;
        if (!res.Any()) throw new ArgumentException("There's no public notes with that id");
        return res.First();
    }

    public Note GetNote(int id, string userId)
    {
        var res = from n in _context.Notes
            where n.Id == id
            select n;

        if (!res.Any()) throw new ArgumentException("There's no notes with that id");

        if (res.First().UserId.Equals(userId) || (!res.First().UserId.Equals(userId) && res.First().IsPublic))
            return res.First();

        throw new Exception("This user can't get that note");
    }

    public async Task DeleteNoteAsync(int id, string userId, bool forced = false)
    {
        var res = from n in _context.Notes
            where n.Id == id
            select n;

        if (!res.Any()) throw new ArgumentException("There's no note with that id");

        if (forced || res.First().UserId.Equals(userId))
        {
            _context.Notes.Remove(res.First());
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("This user can't delete that note");
        }
    }

    public List<Note> GetUserNotes(string userId)
    {
        var res = from n in _context.Notes
            where n.UserId.Equals(userId)
            select n;

        return res.ToList();
    }
}