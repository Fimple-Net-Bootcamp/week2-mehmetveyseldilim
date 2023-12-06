using AutoMapper;
using Weather.API.DTOs.Request;
using Weather.API.DTOs.Response;
using Weather.API.Models;

namespace Weather.API.AutoMapperProfiles
{
    public class WeatherObjectProfile : Profile
    {
       
        public WeatherObjectProfile()
        {
            // source => target
            CreateMap<WeatherObject, ReadWeatherObject>()
                .ForMember(dest => dest.AirQuality, 
                    opt => opt.MapFrom(src => src.AirQuality.ToString()))
                .ForMember(dest => dest.DateTime, opt => 
                    opt.MapFrom(src => src.DateTime.Date));;

            //* true because we need Enum.Parse is to be case insensitive
            //* Meaning "poor" and "Poor" is the same thing
            CreateMap<CreateWeatherObject, WeatherObject>()
            .ForMember(dest => dest.AirQuality, opt => 
                opt.MapFrom(src => Enum.Parse<AirQuality>(src.AirQuality,true)))
            .ForMember(dest => dest.DateTime, opt => 
                opt.MapFrom(src => src.DateTime.Date));
        }
        
        // CreateMap<CommandCreateDto, Command>();
        // CreateMap<CommandUpdateDto, Command>();
        // CreateMap<Command, CommandUpdateDto>();

    }
}