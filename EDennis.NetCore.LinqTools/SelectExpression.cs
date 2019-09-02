using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Holds a paging specification and 
    /// provides a method to apply paging to
    /// an IQueryable according to the spec.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class SelectExpression<TEntity> : List<string>
        where TEntity : class {

        /// <summary>
        /// from https://github.com/dotlattice/LatticeUtils
        /// </summary>
        /// <typeparam name="TEntity">Source Object Type</typeparam>
        /// <param name="source">an IQueryable</param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public IQueryable ApplyTo(IQueryable<TEntity> source, ParameterExpression pe) {
            var properties = typeof(TEntity).GetProperties().Where(p => Contains(p.Name));

            var propertyExpressions = properties.Select(p => Expression.Property(pe, p));

            var anonymousType = AnonymousTypeUtils.CreateType(properties.ToDictionary(p => p.Name, p => p.PropertyType));
            var anonymousTypeConstructor = anonymousType.GetConstructors().Single();
            var anonymousTypeMembers = anonymousType.GetProperties().Cast<MemberInfo>().ToArray();

            // It's important to include the anonymous type members in the New expression, otherwise EntityFramework 
            // won't recognize this as the constructor of an anonymous type.
            var anonymousTypeNewExpression = Expression.New(anonymousTypeConstructor, propertyExpressions, anonymousTypeMembers);

            var selectLambdaMethod = GetExpressionLambdaMethod(pe.Type, anonymousType);
            var selectBodyLambdaParameters = new object[] { anonymousTypeNewExpression, new[] { pe } };
            var selectBodyLambdaExpression = (LambdaExpression)selectLambdaMethod.Invoke(null, selectBodyLambdaParameters);

            var selectMethod = GetQueryableSelectMethod(typeof(TEntity), anonymousType);
            var selectedQueryable = selectMethod.Invoke(null, new object[] { source, selectBodyLambdaExpression }) as IQueryable;
            return selectedQueryable;
        }


        /// <summary>
        /// from https://github.com/dotlattice/LatticeUtils
        /// </summary>
        private static MethodInfo GetExpressionLambdaMethod(Type entityType, Type funcReturnType) {
            var prototypeLambdaMethod = GetStaticMethod(() => System.Linq.Expressions.Expression.Lambda<Func<object, object>>(default(Expression), default(IEnumerable<ParameterExpression>)));
            var lambdaGenericMethodDefinition = prototypeLambdaMethod.GetGenericMethodDefinition();
            var funcType = typeof(Func<,>).MakeGenericType(entityType, funcReturnType);
            var lambdaMethod = lambdaGenericMethodDefinition.MakeGenericMethod(funcType);
            return lambdaMethod;
        }

        /// <summary>
        /// from https://github.com/dotlattice/LatticeUtils
        /// </summary>
        private static MethodInfo GetQueryableSelectMethod(Type entityType, Type returnType) {
            var prototypeSelectMethod = GetStaticMethod(() => Queryable.Select(default(IQueryable<object>), default(Expression<Func<object, object>>)));
            var selectGenericMethodDefinition = prototypeSelectMethod.GetGenericMethodDefinition();
            return selectGenericMethodDefinition.MakeGenericMethod(new[] { entityType, returnType });
        }

        /// <summary>
        /// from https://github.com/dotlattice/LatticeUtils
        /// </summary>
        public static MethodInfo GetStaticMethod(Expression<Action> expression) {
            var lambda = expression as LambdaExpression;
            var methodCallExpression = lambda.Body as MethodCallExpression;
            return methodCallExpression.Method;
        }

    }

}
