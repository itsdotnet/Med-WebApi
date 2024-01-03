using AutoMapper;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Doctors;
using Medic.Service.DTOs.Messages;
using Medic.Service.DTOs.Users;

namespace Medic.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //User
        CreateMap<User, UserResultDto>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<UserCreationDto, User>().ReverseMap();
        
        
        //Doctor
        CreateMap<Doctor, DoctorResultDto>().ReverseMap();
        CreateMap<DoctorUpdateDto, Doctor>().ReverseMap();
        CreateMap<DoctorCreationDto, Doctor>().ReverseMap();
        
        
        //Message
        CreateMap<Message, MessageResultDto>().ReverseMap();
        CreateMap<MessageCreationDto, Message>().ReverseMap();
    }
}