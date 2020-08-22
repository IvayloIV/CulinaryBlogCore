﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CulinaryBlogCore.Models;
using CulinaryBlogCore.Services.Contracts;
using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CulinaryBlogCore.Controllers
{
    public class HomeController : Controller
    {
        private IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public HomeController(
            IRecipeService recipeService, 
            IMapper mapper)
        {
            this._recipeService = recipeService;
            this._mapper = mapper;
        }

        public ActionResult Index()
        {
            IList<Recipe> recipesByRating = this._recipeService.GetByRatingWeek();
            List<RecipeViewModel> recipesByRatingModel = this._mapper.Map<List<RecipeViewModel>>(recipesByRating);

            IList<Recipe> lastAddedRecipes = this._recipeService.GetLastAdded();
            List<RecipeViewModel> lastAddedRecipesModel = this._mapper.Map<List<RecipeViewModel>>(lastAddedRecipes);

            RecipeHomeViewModel recipeHomeViewModel = new RecipeHomeViewModel() { 
                recipesByRating = recipesByRatingModel,
                lastAddedRecipes = lastAddedRecipesModel
            };
            return View(recipeHomeViewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
