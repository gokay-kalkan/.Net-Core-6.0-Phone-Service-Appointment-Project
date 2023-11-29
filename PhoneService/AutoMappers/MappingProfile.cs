using AutoMapper;
using PhoneService.Dtos.AppointmentDtos;
using PhoneService.Entities;

namespace PhoneService.AutoMappers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentCreateDto>().ReverseMap();
        }
    }
}
