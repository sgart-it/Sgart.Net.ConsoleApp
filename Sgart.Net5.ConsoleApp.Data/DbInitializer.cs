using Sgart.Net5.ConsoleApp.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sgart.Net5.ConsoleApp.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// inserisce i valori iniziali nel DB
        /// </summary>
        /// <param name="context">SgartDbContext</param>
        public static void Initialize(SgartDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Todos.Any() == false)
            {
                // popolo la tabella, se vuota, con dei valori iniziali

                DateTime dt = DateTime.UtcNow;

                var todoItems = new List<Todo>
                {
                    new Todo
                    {
                        DataJson = new BO.TodoData
                        {
                            Text = "Configurare la connection string",
                            Completed = true
                        },
                        Created = dt
                    },
                    new Todo
                    {
                        DataJson = new BO.TodoData
                        {
                            Text = "Configurare il percorso dei log",
                            Completed = false
                        },
                        Created = dt
                    },
                    new Todo
                    {
                        DataJson = new BO.TodoData
                        {
                            Text = "Eseguire la console app",
                            Completed = false
                        },
                        Created = dt
                    }

                };

                context.Todos.AddRange(todoItems);
                context.SaveChanges();
            }
        }
    }
}
