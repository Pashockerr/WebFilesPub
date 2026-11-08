using Microsoft.AspNetCore.Identity;

namespace WebFiles.Domain.Entities;

public class User : IdentityUser
{
    public bool IsExtendedAccount { get; set; }
    private long availableSpace;
    public long AvailableSpace
    {
        get
        {
            return availableSpace;
        }
        set{
        if(value < 0){
            throw new System.Exception("Can't set available space less than zero.");
        }
        else{
            availableSpace = value;
        }
    } }
}