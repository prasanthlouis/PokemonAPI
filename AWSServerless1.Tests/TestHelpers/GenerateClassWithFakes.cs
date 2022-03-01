using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PokemonAPI.Tests.TestHelpers
{
    public class GenerateClassWithFakes
    {
        public T Generate<T>(params object[] parameters) where T : class
        {
            if (!typeof(T).IsClass)
            {
                throw new System.Exception("The type passed in is not a class");
            }

            var passedParameters = new List<object>(parameters);
            var resultParameters = new List<object>();
            var obj = (from a in typeof(T).GetConstructors()
                       let b = a.GetParameters()
                       orderby b.Count() descending
                       select new
                       {
                           ctor = a,
                           parameters = new List<ParameterInfo>(b)

                       }).FirstOrDefault();

            if (obj == null)
                return null;

            if (parameters.Length > obj.parameters.Count)
                throw new System.Exception("You've passed in more parameters than the actual constructor has");

            foreach (var parameter in obj.parameters)
            {
                var param = passedParameters.SingleOrDefault(x => parameter.ParameterType.IsInstanceOfType(x));
                if(param != null)
                {
                    resultParameters.Add(param);
                }
                else
                {
                    var method = typeof(A).GetMethod("Fake", new Type[] { }).MakeGenericMethod(parameter.ParameterType);
                    param = method.Invoke(null, null);
                    resultParameters.Add(param);
                }

                
            }

            var createdObject = (T)obj.ctor.Invoke(resultParameters.ToArray());
            return createdObject;

        }
    }
}
