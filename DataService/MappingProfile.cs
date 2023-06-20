using AutoMapper;
using DatabaseConection.Entities;
using ViewModel.CategoryMapping;
using ViewModel.Chat;

namespace DataService
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryMapping, CategoryMappingViewModel>();
            CreateMap<Chat,ChatViewModel>();
        }
    }
}