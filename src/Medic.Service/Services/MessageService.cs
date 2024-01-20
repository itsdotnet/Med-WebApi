using AutoMapper;
using Medic.DataAccess.UnitOfWorks;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Messages;
using Medic.Service.Exceptions;
using Medic.Service.Interfaces;

namespace Medic.Service.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var message = await _unitOfWork.MessageRepository.SelectAsync(q => q.Id == id);

        if (message is null)
            throw new NotFoundException("Message not found");

        await _unitOfWork.MessageRepository.DeleteAsync(x => x == message);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<MessageResultDto> GetByIdAsync(long id)
    {
        var message = await _unitOfWork.MessageRepository.SelectAsync(q => q.Id == id);

        if (message is null)
            throw new NotFoundException("Message not found");

        return _mapper.Map<MessageResultDto>(message);
    }

    public async Task<IEnumerable<MessageResultDto>> GetAllAsync()
    {
        var messages = _unitOfWork.MessageRepository.SelectAll();

        return _mapper.Map<IEnumerable<MessageResultDto>>(messages);
    }

    public async Task<IEnumerable<MessageResultDto>> GetByUserAsync(long userId)
    {
        var existUser = await _unitOfWork.DoctorRepository
            .SelectAsync(x => userId == x.Id);
        
        if (existUser is null)
            throw new NotFoundException("User not found with this ID");
        
        var messages = _unitOfWork.MessageRepository.SelectAll(q => q.UserId == userId);

        return _mapper.Map<IEnumerable<MessageResultDto>>(messages);
    }

    public async Task<IEnumerable<MessageResultDto>> GetByDoctorAsync(long doctorId)
    {
        var existDoctor = await _unitOfWork.DoctorRepository
            .SelectAsync(x => doctorId == x.Id);
        
        if (existDoctor is null)
            throw new NotFoundException("Doctor not found with this ID");
        
        var messages = _unitOfWork.MessageRepository.SelectAll(q => q.DoctorId == doctorId);

        return _mapper.Map<IEnumerable<MessageResultDto>>(messages);
    }

    public async Task<MessageResultDto> CreateAsync(MessageCreationDto messageCreationDto)
    {
        var existDoctor = await _unitOfWork.DoctorRepository
            .SelectAsync(x => messageCreationDto.DoctorId == x.Id);
        var existUser = await _unitOfWork.UserRepository
            .SelectAsync(u => messageCreationDto.UserId == u.Id);

        if (existDoctor is null)
            throw new NotFoundException("Doctor not found with this ID");
        
        if (existUser is null)
            throw new NotFoundException("User not found with this ID");
            
        var newMessage = _mapper.Map<Message>(messageCreationDto);

        await _unitOfWork.MessageRepository.AddAsync(newMessage);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<MessageResultDto>(newMessage);
    }
}