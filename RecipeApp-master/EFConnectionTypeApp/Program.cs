using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EFConnectionTypeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (RecipeContext db = new RecipeContext())
            {
                while (true)
                {
                    Console.WriteLine("1 - Создание рецепта");
                    Console.WriteLine("2 - Редактирование рецепта");
                    Console.WriteLine("3 - Список рецептов");

                    int key;

                    if (int.TryParse(Console.ReadLine(), out key))
                    {
                        switch (key)
                        {
                            case 1:
                                Console.Clear();
                                CreatNewRecipe(db);
                                break;
                            case 2: EditRecipe(db); break;
                            case 3: ShowRecipeList(db.Recipes); break;
                            default:break;
                        }
                    }

                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static void EditRecipe(RecipeContext db)
        {
            if(db.Recipes.Count() == 0)
            {
                Console.WriteLine("Список пуст!");
                return;
            }

            ShowRecipeList(db.Recipes);
            Console.WriteLine("Введите ID рецепта: ");
            int id;

            if (int.TryParse(Console.ReadLine(),out id))
            {
                Console.Clear();
                Recipe recipe = db.Recipes.Find(id);
                if(recipe !=null)
                    MenuToEditRecipe(db, recipe);
                else
                    Console.WriteLine("Введен не верный ID!!!");
            }
        }

        static void MenuToEditRecipe(RecipeContext db, Recipe recipe)
        {            
            while (true)
            {
                ShowIngredientList(recipe);
                Console.WriteLine("\n1 - Сменить название рецепта");
                Console.WriteLine("2 - Редактировать ингредиент");
                Console.WriteLine("3 - Удалить ингредиент");
                Console.WriteLine("4 - Добавить ингредиент");
                Console.WriteLine("5 - Сохранить и выйти");

                int key;
                int id;

                if (int.TryParse(Console.ReadLine(), out key))
                {
                    switch (key)
                    {
                        case 1:
                            Console.Write("Введите новое название:");
                            string name = Console.ReadLine();
                            recipe.Name = name; break;
                        case 2:
                            Console.WriteLine("Введите ID ингредиента: ");                            

                            if (int.TryParse(Console.ReadLine(), out id))
                            {
                                Console.Clear();
                                Ingredient ing = recipe.Ingredients.Where(item => item.Id == id).FirstOrDefault();
                                if (ing != null)
                                    EditIngredient(ing);
                                else
                                    Console.WriteLine("Введен не верный ID!!!");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Введите ID ингредиента: ");
                                                 
                            if (int.TryParse(Console.ReadLine(), out id))
                            {                                
                                Ingredient ing = recipe.Ingredients.Where(item => item.Id == id).FirstOrDefault();
                                if (ing != null)
                                    recipe.Ingredients.Remove(ing);
                                else
                                    Console.WriteLine("Введен не верный ID!!!");
                            }
                            break;
                        case 4:
                            Ingredient newIng = new Ingredient();
                            AddIngredient(newIng);
                            recipe.Ingredients.Add(newIng);
                            break;
                        case 5: db.SaveChanges(); Console.WriteLine("Сохранено!!!"); return;                        
                        default: break;
                    }
                }

                Console.ReadLine();
                Console.Clear();
            }
        }

        static void EditIngredient(Ingredient ing)
        {
            Console.WriteLine("Текущее количество: {0} {1}", ing.Count, ing.Measure);
            while (true)
            {
                Console.Write("Введите новое количество ингредиента: ");
                int count;
                if (int.TryParse(Console.ReadLine(), out count))
                {
                    ing.Count = count;
                    break;
                }
                Console.WriteLine("Ошибка!Введите число");
            }
        }

        static void ShowIngredientList(Recipe recipe)
        {
            Console.WriteLine("Рецепт: {0}", recipe.Name);
            Console.WriteLine("Ингредиенты:");
            foreach (var ing in recipe.Ingredients)
            {
                Console.WriteLine("ID: {3} | Название: {0} | Количество: {1} {2}", ing.Name, ing.Count, ing.Measure, ing.Id);
            }
        }

        static void ShowRecipeList(DbSet<Recipe> recipeList)
        {

            if(recipeList.Count() == 0)
            {
                Console.WriteLine("Список пуст!");
                return;
            }

            Console.WriteLine("Список рецептов: ");
            
            foreach (var recipe in recipeList.ToList())
            {
                Console.WriteLine("ID: {0} | Name: {1}", recipe.Id, recipe.Name);
            }
        }

        static void CreatNewRecipe(RecipeContext db)
        {
            Recipe recipe = new Recipe();
            Ingredient ingredient = new Ingredient();

            bool flag = true;

            while (flag)
            {
                Console.WriteLine("1 - Добавить название рецепта");
                Console.WriteLine("2 - Добавить ингредиент");
                Console.WriteLine("3 - Сохранить и выйти");

                int key;

                if (int.TryParse(Console.ReadLine(), out key))
                {
                    switch (key)
                    {
                        case 1:
                            Console.WriteLine("Текущее название рецепта: {0}", recipe.Name == null?"не установлено":recipe.Name);
                            Console.Write("Введите новое название:");
                            string name = Console.ReadLine();
                            recipe.Name = name;
                            Console.WriteLine("Сохранено!!!");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 2:
                            AddIngredient(ingredient);                            
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 3:
                            if(recipe.Name == null)
                            {
                                Console.WriteLine("Не сохранено! Вы не указали название рецепта!");
                            }
                            else
                            {
                                SaveRecipe(db, recipe, ingredient);
                                Console.WriteLine("Рецепт сохранен!!!");
                            }
                            flag = false;
                            break;
                        default: break;
                    }
                }                               
            }
        }

        static void AddIngredient(Ingredient ingredient)
        {
            Console.Write("Введите название ингредиента: ");
            ingredient.Name = Console.ReadLine();
            while (true)
            {
                Console.Write("Введите количество ингредиента: ");
                int count;
                if (int.TryParse(Console.ReadLine(), out count))
                {
                    ingredient.Count = count;
                    break;
                }
                Console.WriteLine("Ошибка!Введите число");
            }
            Console.Write("Введите ед.измерения(Например: кг.,шт. и.т.п):");
            ingredient.Measure = Console.ReadLine();
            Console.WriteLine("Ингредиент добавлен!!!");
        }

        static void SaveRecipe(RecipeContext db, Recipe recipe, Ingredient ingredient)
        {
            db.Recipes.Add(recipe);
            ingredient.Recipe = recipe;
            db.Ingredients.Add(ingredient);

            db.SaveChanges();
        }
    }
}
