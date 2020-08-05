using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Creating a map from source to destination
            CreateMap<User, UserForListDto>()
            //Maping the photo Url prop to our Main photo(boolean true)
                .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.Main).Url))
                .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.Main).Url))
                  .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetailed>();

            CreateMap<UserForEditDto, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<UserForRegisterDto, User>();
            // ReverseMap for mapping to go both ways
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt =>
                opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.Main == true).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt =>
                opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.Main == true).Url));
        
        }
    }
}