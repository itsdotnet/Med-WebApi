using AutoMapper;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Attachments;
using Medic.Service.DTOs.Users;
using Medic.Service.DTOs.Reports;
using Medic.Service.DTOs.Doctors;
using Medic.Service.DTOs.Messages;
using Medic.Service.DTOs.Bookings;
using Medic.Service.DTOs.Hospitals;
using Medic.Service.DTOs.Feedbacks;

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
        
        //Attachment
        CreateMap<Attachment, AttachmentResultDto>().ReverseMap();
        
        //Message
        CreateMap<Message, MessageResultDto>().ReverseMap();
        CreateMap<MessageCreationDto, Message>().ReverseMap();
        
        // Feedback
        CreateMap<Feedback, FeedbackResultDto>().ReverseMap();
        CreateMap<FeedbackCreationDto, Feedback>().ReverseMap();
        
        //Hospital
        CreateMap<Hospital,HospitalCreationDto>().ReverseMap();
        CreateMap<Hospital, HospitalUpdateDto>().ReverseMap();
        CreateMap<HospitalResultDto, Hospital>().ReverseMap();
        
        //Book
        CreateMap<Book, BookCreationDto>().ReverseMap();
        CreateMap<Book, BookUpdateDto>().ReverseMap();
        CreateMap<BookResultDto, Book>().ReverseMap();
        
        //Report
        CreateMap<Report, ReportCreationDto>().ReverseMap();
        CreateMap<Report, ReportUpdateDto>().ReverseMap();
        CreateMap<ReportResultDto, Report>().ReverseMap();
    }
}