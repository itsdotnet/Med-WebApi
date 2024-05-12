using AutoMapper;
using Medic.DataAccess.UnitOfWorks;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Attachments;
using Medic.Service.DTOs.Users;
using Medic.Service.Exceptions;
using Medic.Service.Helpers;
using Medic.Service.Interfaces;

namespace Medic.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }
    
    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");

        await _unitOfWork.UserRepository.DeleteAsync(x => x == user);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id, new []{"Attachment"});

        if (user is null)
            throw new NotFoundException("User not found");
        
        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = (IEnumerable<User>)_unitOfWork.UserRepository.SelectAll(includes: new []{"Attachment"});

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> GetByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Email == email, includes: new []{"Attachment"});

        if (user is null)
            throw new NotFoundException("User not found");

        return _mapper.Map<UserResultDto>(user);
    }


    public async Task<UserResultDto> ModifyAsync(UserUpdateDto dto)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(d => d.Id == dto.Id);
    
        if (exist is null)
            throw new NotFoundException("User not found");
        if (dto.Firstname == exist.Firstname && dto.Lastname == exist.Lastname && dto.DateOfBirth == exist.DateOfBirth)
        {
            if(dto.ImagePath is null)
                throw new CustomException(400, "You changed nothing");
        }
        Attachment image = await _attachmentService.UploadAsync(new AttachmentCreationDto(){
            File = dto.ImagePath
        });
        
        _mapper.Map(dto, exist);
        exist.AttachmentId = image.Id;

        await _unitOfWork.UserRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }

    public async Task<UserResultDto> CreateAsync(UserCreationDto dto)
    {
        dto.Email = dto.Email.Trim().ToLower();
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Email == dto.Email);

        if (exist is not null)
            throw new AlreadyExistException("User already exist with this Email");
        
        var newUser = _mapper.Map<User>(dto);
        newUser.Password = PasswordHasher.Hash(newUser.Password);
        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(newUser);
    }

    public async Task<IEnumerable<UserResultDto>> GetByName(string name)
    {
        var users = _unitOfWork.UserRepository.SelectAll(u =>
            u.Firstname.Contains(name) || u.Lastname.Contains(name), includes: new []{"Attachment"});

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> ModifyPasswordAsync(long id, string oldPass, string newPass)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (exist is null)
            throw new NotFoundException("User not found");

        if (oldPass.Verify(exist.Password))
            throw new CustomException(403, "Passwor is invalid");

        exist.Password = newPass.Hash();
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }
}