using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EFConnectionTypeApp
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Measure { get; set; }

        public int? RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}