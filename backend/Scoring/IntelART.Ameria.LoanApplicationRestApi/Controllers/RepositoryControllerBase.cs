using System;
using IntelART.Ameria.Repositories;
using Microsoft.Extensions.Configuration;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    public abstract class RepositoryControllerBase<T> : ControllerBase
        where T : BaseRepository
    {
        private Lazy<T> repositoryFactory;
        private string connectionString;
        protected string languageCode;

        protected T Repository
        {
            get
            {
                return this.repositoryFactory.Value;
            }
        }

        public RepositoryControllerBase(IConfigurationRoot Configuration, Func<string, T> repositoryInitializer)
        {
            this.connectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
            this.repositoryFactory = new Lazy<T>(() => repositoryInitializer(this.connectionString), true);
            this.languageCode = this.GetLanguageCode();
        }
    }
}
