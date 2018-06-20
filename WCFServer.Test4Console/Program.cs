using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WCFServer.Service;

namespace WCFServer.Test4Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var subTypeQuery = from t in Assembly.GetExecutingAssembly().GetTypes()
                               where IsSubClassOf(t, typeof(IBaseContract))
                               select t;

            foreach (var type in subTypeQuery)
            {
                Console.WriteLine(type);
            }

            //var types = AppDomain.CurrentDomain.GetAssemblies()
            //        .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IBaseContract))))
            //        .ToArray();
            var types = GetType(typeof(IBaseContract));
            Console.WriteLine(types.Count());   // 用的时候要求是偶数
            Dictionary<Type, Type> pairs = new Dictionary<Type, Type>();
            bool isPair = true;
            Type type1 = null, type2 = null;
            foreach (var type in types)
            {
                Console.WriteLine(type);
                if (isPair)
                { 
                    type1 = type;
                    isPair = false;
                }
                else
                {
                    type2 = type;
                    pairs.Add(type1, type2);
                    isPair = true;
                    Console.WriteLine(type1.Name + "==>" + type2.Name);
                }
            }
            Console.ReadKey();
        }

        static IEnumerable<Type> GetType(Type interfaceType)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == interfaceType)
                        {
                            yield return type;
                            break;
                        }
                    }
                }
            }
        }

        static bool IsSubClassOf(Type type, Type baseType)
        {
            var b = type.BaseType;
            while (b != null)
            {
                if (b.Equals(baseType))
                {
                    return true;
                }
                b = b.BaseType;
            }
            return false;
        }



    }

    public class Base { }
    public class Sub1 : Base { }
    public class Sub2 : Base { }
    public class Sub3 : Sub1 { }
}
