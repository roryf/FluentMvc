namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;

    public class ActionFilterRegistry : AbstractRegistry<ActionFilterRegistryItem, ActionFilterSelector>, IActionFilterRegistry
    {
        private IFluentMvcObjectFactory objectFactory;

        public ActionFilterRegistry(IFluentMvcObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public void AddFiltersTo(FilterInfo baseFilters, ActionFilterSelector selector)
        {
            var applicableFilters = FindForSelector(selector);
            FilterAndAddFilters(baseFilters.ActionFilters, applicableFilters);
            FilterAndAddFilters(baseFilters.AuthorizationFilters, applicableFilters);
            FilterAndAddFilters(baseFilters.ExceptionFilters, applicableFilters);
            FilterAndAddFilters(baseFilters.ResultFilters, applicableFilters);
        }

        public void SetObjectFactory(IFluentMvcObjectFactory factory)
        {
            objectFactory = factory;
        }

        private void FilterAndAddFilters<T>(ICollection<T> baseFiltersList, IEnumerable<ActionFilterRegistryItem> applicableFilters)
        {
            foreach (var item in FilterActionFilters<T>(applicableFilters))
            {
                var filter = CreateFilter<T>(item);
                AddFilter(baseFiltersList, filter);
            }
        }

        private void AddFilter<T>(ICollection<T> baseFiltersList, T filter)
        {
            baseFiltersList.Add(filter);
        }

        private T CreateFilter<T>(RegistryItem item)
        {
            return objectFactory.Resolve<T>(item.Type);
        }

        private IEnumerable<ActionFilterRegistryItem> FilterActionFilters<T>(IEnumerable<ActionFilterRegistryItem> applicableFilters)
        {
            return applicableFilters.Where(i => (typeof(T)).IsAssignableFrom(i.Type));
        }
    }
}