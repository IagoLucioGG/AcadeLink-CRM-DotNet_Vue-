using System.Reflection;

namespace CRM.Maps
{

    public static class MapeadorModels
    {
        public static TDestino Montar<TDestino, TOrigem>(TOrigem origem)
            where TDestino : new()
        {
            var destino = new TDestino();
            CopiarPropriedades(origem, destino);
            return destino;
        }

        public static void CopiarPropriedades<TOrigem, TDestino>(TOrigem origem, TDestino destino)
        {
            var propsDestino = typeof(TDestino).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propsOrigem = typeof(TOrigem).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propDestino in propsDestino)
            {
                if (!propDestino.CanWrite) continue;

                var propOrigem = propsOrigem.FirstOrDefault(p => p.Name == propDestino.Name);
                if (propOrigem == null) continue;

                var valor = propOrigem.GetValue(origem);

                // Se os tipos batem, copia direto
                if (propOrigem.PropertyType == propDestino.PropertyType)
                {
                    propDestino.SetValue(destino, valor);
                }
                // Caso especial: origem string/char e destino enum
                else if (propDestino.PropertyType.IsEnum &&
                        (propOrigem.PropertyType == typeof(string) || propOrigem.PropertyType == typeof(char)))
                {
                    if (valor != null)
                    {
                        var strValor = valor.ToString();
                        if (!string.IsNullOrEmpty(strValor))
                        {
                            var enumValor = Enum.Parse(propDestino.PropertyType, strValor);
                            propDestino.SetValue(destino, enumValor);
                        }
                    }
                }
                // Caso especial: origem enum e destino string
                else if (propOrigem.PropertyType.IsEnum && propDestino.PropertyType == typeof(string))
                {
                    propDestino.SetValue(destino, valor?.ToString());
                }
            }
        }

    }
}
