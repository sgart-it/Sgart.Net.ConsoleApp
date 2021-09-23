using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sgart.Net5.ConsoleApp.Data
{
    public static class DbUpdater
    {
        /// <summary>
        /// applica le migration al db 
        /// e se vuoto inserisce i valori iniziali nel db
        /// </summary>
        /// <param name="serviceScope">IServiceScope</param>
        public static void Upgrade(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<SgartDbContext>();
            if (context != null && context.Database != null)
            {
                context.Database.Migrate();

                DbInitializer.Initialize(context);
            }
        }

    }
}
