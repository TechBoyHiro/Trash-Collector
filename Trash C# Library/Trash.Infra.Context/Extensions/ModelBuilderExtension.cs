using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trash.Infra.Context.Extensions
{
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Dynamicaly register all Entities that inherit from specific BaseType T
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="baseType">Base type that Entities inherit from this</param>
        /// <param name="assemblies">Assemblies contains Entities</param>
        public static void RegisterEntities<T>(this ModelBuilder modelbuilder,params Assembly[] assemblies)
        {
            IEnumerable<Type> types = assemblies.SelectMany(x => x.GetExportedTypes()).Where(x => x.IsClass && x.IsPublic && !x.IsAbstract && typeof(T).IsAssignableFrom(x));

            foreach(Type type in types)
            {
                modelbuilder.Entity(type);
            }
        }
    }
}
