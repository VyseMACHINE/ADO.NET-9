using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EFConnectionTypeApp
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }

        virtual public ICollection<Ingredient> Ingredients { get; set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
        }
    }
}