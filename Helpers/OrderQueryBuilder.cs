using System.Reflection;
using System.Text;
using Weather.API.Exceptions;

namespace Weather.API.Helpers
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                
                var trimmedQueryName = param.Trim();
                var propertyFromQueryName = trimmedQueryName.Split(" ")[0];



                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    throw new PropertyNotFoundException(propertyFromQueryName);
    
                var direction = param.EndsWith(" desc", StringComparison.InvariantCultureIgnoreCase) ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            return orderQuery;
        }
    }
}

