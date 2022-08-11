using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Task2.Models;

namespace Task2.service
{
    public class PizzaService:IPizzaService
    {
        public async Task<IEnumerable<Pizza>> GetPizzaFromHtml()
        {
            string URL = "https://cagliari-pizza.ru/pizza.html";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);


            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //ответ от сервера
                var result = streamReader.ReadToEnd();

                IConfiguration config = Configuration.Default;
                IBrowsingContext context = BrowsingContext.New(config);
                IDocument document = await context.OpenAsync(req => req.Content(result.ToString()));

                var pizzaBlocks = document.QuerySelectorAll(".product_preview");

                List<Pizza> pizzas = new List<Pizza>();
                int i = 0;
                foreach (var item in pizzaBlocks)
                {
                    var photoUrl = item.QuerySelector(".product_preview_image")
                        .GetAttribute("style")
                        .Split(" ")[1];
                    var startIndex = 4;
                    var endLength = 2; // сколько отсекаем смволов с конца
                    var photo = photoUrl.Substring(startIndex, photoUrl.Length - endLength - startIndex);
                    item.QuerySelector(".product_preview_price").QuerySelector(".ruble").Remove();//удаляем знак рубля
                    var ingredient = item.QuerySelector(".product_preview_desc").QuerySelectorAll(".del_ingredient");//удаляем некоторый состав пиццы
                    foreach (var it in ingredient)
                    {
                        it.Remove();
                    }

                    pizzas.Add(new Pizza
                    {
                        Id = i,
                        Name = item.QuerySelector(".product_preview_name").TextContent,
                        Photo = $"https://cagliari-pizza.ru/{photo}",
                        Desc = item.QuerySelector(".product_preview_desc").TextContent.TrimEnd(new char[] { ',', ' ' }),
                        Size1 = item.QuerySelector(".size_1").QuerySelector(".name").TextContent.Split("/")[0],
                        Size2 = item.QuerySelector(".size_2").QuerySelector(".name").TextContent.Split("/")[0],
                        Size3 = item.QuerySelector(".size_3").QuerySelector(".name").TextContent.Split("/")[0],
                        Price = item.QuerySelector(".product_preview_price").TextContent,
                        Weight = item.QuerySelector(".product_preview_weight").TextContent
                    });
                    i++;
                }

                return pizzas;
            }
        }
    }
}
