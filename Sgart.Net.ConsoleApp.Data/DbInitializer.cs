using Sgart.Net.ConsoleApp.BO;
using Sgart.Net.ConsoleApp.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sgart.Net.ConsoleApp.Data
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

                // inserisco dei valori iniziali
                var todoItems = new List<Todo>
                {
                    new Todo
                    {
                        DataJson = new BO.TodoData
                        {
                            Text = "Configurare la connection string",
                            Completed = true
                        },
                        CreatedUTC = dt,
                        ModifiedUTC = dt
                    },
                    new Todo
                    {
                        DataJson = new BO.TodoData
                        {
                            Text = "Configurare il percorso dei log",
                            Completed = false
                        },
                        CreatedUTC = dt,
                        ModifiedUTC = dt
                    },
                    new Todo
                    {
                        DataJson = new BO.TodoData
                        {
                            Text = "Eseguire la console app",
                            Completed = false
                        },
                        CreatedUTC = dt,
                        ModifiedUTC = dt
                    }

                };

                // li aggiungo al conttesto nella tabella
                context.Todos.AddRange(todoItems);

                // persist/salvo i cambiamenti su DB
                context.SaveChanges();
            }
        }
    }
}
