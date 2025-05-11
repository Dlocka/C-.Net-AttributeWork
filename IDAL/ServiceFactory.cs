using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
namespace IDAL
{
    public class ServiceFactory
    {
        
        public static IServiceDAL CreateUserService()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appSettings.json");
            IConfiguration configuration = builder.Build();
            string ProjectName = configuration["AppSettings:DALProfile"];
            
            //Console.WriteLine(ProjectName);
            Assembly assembly = Assembly.LoadFrom(ProjectName + ".dll");
            if (assembly == null)
            {
                Console.WriteLine("null");
            }
            try
            {
                IServiceDAL serviceDAL = (IServiceDAL)assembly.CreateInstance(ProjectName + ".DALService");
                return serviceDAL;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
            
        }
    }
}
