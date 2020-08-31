using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.ProductViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;

using AutoMapper;
using CulinaryBlogCore.Models.ChefViewModels;
using CulinaryBlogCore.Models.TrickViewModels;

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

            CreateMap<Chef, CreateChefViewModel>();
            CreateMap<CreateChefViewModel, Chef>();

            CreateMap<Chef, ChefViewModel>();
            CreateMap<ChefViewModel, Chef>();

            CreateMap<Chef, DeleteChefViewModel>();
            CreateMap<DeleteChefViewModel, Chef>();

            CreateMap<Chef, UpdateChefViewModel>();
            CreateMap<UpdateChefViewModel, Chef>();

            CreateMap<Trick, CreateTrickViewModel>();
            CreateMap<CreateTrickViewModel, Trick>();

            CreateMap<Trick, TrickViewModel>();
            CreateMap<TrickViewModel, Trick>();
        }
    }
}
