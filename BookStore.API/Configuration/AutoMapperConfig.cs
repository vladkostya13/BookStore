using AutoMapper;
using BookStore.API.Dtos.Book;
using BookStore.API.Dtos.Category;
using BookStore.Domain.Models;

namespace BookStore.API.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryAddDto>().ReverseMap();
            CreateMap<Category, CategoryEditDto>().ReverseMap();
            CreateMap<Category, CategoryResultDto>().ReverseMap();
            CreateMap<Book, BookAddDto>().ReverseMap();
            CreateMap<Book, BookEditDto>().ReverseMap();
            CreateMap<Book, BookResultDto>().ReverseMap();
        }
    }
}