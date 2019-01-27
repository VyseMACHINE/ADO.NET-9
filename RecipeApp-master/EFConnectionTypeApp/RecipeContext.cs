using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace EFConnectionTypeApp
{
    class RecipeContext:DbContext
    {
        public RecipeContext() : base("RecipeConnection")
        {

        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}