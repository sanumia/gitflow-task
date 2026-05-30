using AutoMapper;

namespace PermissionAttribute.Models;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, Contact>()
            .ForMember(dest => dest.ContactId, opt => opt.Ignore()) 
            .ForMember(dest => dest.OwnerID, opt => opt.Ignore())  
            .ForMember(dest => dest.Status, opt => opt.Ignore());  
    }
}
