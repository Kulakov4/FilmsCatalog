using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FilmsCatalog.Extensions
{
    public static class Reflection
    {
        /// <summary>
        /// Расширение, позволяет копировать все свойства из одного объекта в другой
        /// </summary>
        /// <param name="source">Источник</param>
        /// <param name="destination">Назначение</param>
        public static void CopyProperties(this object destination, object source)
        {
            // Если где-то пусто, то бросаем ошибку
            if (source == null || destination == null)
                throw new Exception("Источник и/или назначение пусты");
            // Получаем типы объектов
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // По очереди проходим все свойства родительского объекта
            // и заполняем объект назначения
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);   // получаем свойство цели
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                if (targetProperty.GetMethod.IsVirtual)     //виртуальные свойства не задаём
                {
                    continue;
                }
                // все тесты пройдены, можем задать свойство
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}
