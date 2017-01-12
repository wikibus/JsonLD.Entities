using System;
using System.Reflection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace JsonLD.Entities.Tests.SpecflowHelpers
{
    public static class TableExtensions
    {
        private static readonly MethodInfo RealCreateInstance;

        static TableExtensions()
        {
            RealCreateInstance = typeof(TableHelperExtensionMethods).GetMethod("CreateInstance", new[] { typeof(Table) });
        }

        public static object CreateInstance(this Table table, Type objecType)
        {
            var createInstance = RealCreateInstance.MakeGenericMethod(objecType);

            return createInstance.Invoke(null, new object[] { table });
        }
    }
}