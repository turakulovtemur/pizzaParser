using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.Models;

namespace Task2.service
{
    public interface IPizzaService
    {       
        public Task<IEnumerable<Pizza>> GetPizzaFromHtml();

    }
}
