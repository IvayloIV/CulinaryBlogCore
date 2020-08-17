using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;

namespace CulinaryBlogCore.Utils
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<Recipe, CreateViewModel>();
            CreateMap<CreateViewModel, Recipe>();

            CreateMap<Recipe, RecipeViewModel>();
            CreateMap<RecipeViewModel, Recipe>();

            CreateMap<Recipe, MoreViewModel>();
            CreateMap<MoreViewModel, Recipe>();

            CreateMap<Recipe, UpdateViewModel>();
            CreateMap<UpdateViewModel, Recipe>();

            CreateMap<Recipe, DeleteViewModel>();
            CreateMap<DeleteViewModel, Recipe>();
        }
    }
}
