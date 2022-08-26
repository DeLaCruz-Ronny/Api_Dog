using Api_Dog.DTOs;
using Api_Dog.Entidades;
using AutoMapper;

namespace Api_Dog.Utilidades
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Dog, DogDTO>();
            CreateMap<DogsDTO, Dog>();
        }
    }
}
