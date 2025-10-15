using System;
using System.Reflection;

public static class DebugHelper
{
    public static void PrintObjectProperties(object obj)
    {
        if (obj == null)
        {
            Console.WriteLine("Objeto es null");
            return;
        }

        Type type = obj.GetType();
        Console.WriteLine("Propiedades de {type.Name}:");

        foreach (PropertyInfo prop in type.GetProperties())
        {
            object value = prop.GetValue(obj, null);
            Console.WriteLine("  {prop.Name} = {value ?? \"null\"}");
        }
    }
}
