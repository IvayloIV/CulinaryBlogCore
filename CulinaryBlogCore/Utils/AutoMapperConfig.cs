using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.ProductViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;

using AutoMapper;

namespace CulinaryBlogCore.Utils
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<Recipe, CreateRecipeViewModel>();
            CreateMap<CreateRecipeViewModel, Recipe>();

            CreateMap<Recipe, RecipeViewModel>();
            CreateMap<RecipeViewModel, Recipe>();

            CreateMap<Recipe, MoreRecipeViewModel>();
            CreateMap<MoreRecipeViewModel, Recipe>();

            CreateMap<Recipe, UpdateRecipeViewModel>();
            CreateMap<UpdateRecipeViewModel, Recipe>();

            CreateMap<Recipe, DeleteRecipeViewModel>();
            CreateMap<DeleteRecipeViewModel, Recipe>();

            CreateMap<Recipe, RecipeDetailsViewModel>();
            CreateMap<RecipeDetailsViewModel, Recipe>();

            CreateMap<Product, CreateProductViewModel>();
            CreateMap<CreateProductViewModel, Product>();

            CreateMap<UserRecipeRating, RecipeRatingViewModel>();
            CreateMap<RecipeRatingViewModel, UserRecipeRating>();

            CreateMap<RecipeSubscription, RecipeSubscriptionViewModel>();
            CreateMap<RecipeSubscriptionViewModel, RecipeSubscription>();
        }
    }
}
