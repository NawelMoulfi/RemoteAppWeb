using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
//using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
//using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace RemoteApp.Data.Models
{

    public class EntityBase : INotifyPropertyChanged//, IDataErrorInfo
    {
        //public string GetErrorText(string propertyName, object value)
        //{
        //    var v = GetPropertyValidator(GetType(), propertyName);
        //    return v?.GetErrorText(value, this);
        //}

        //public static PropertyValidator GetPropertyValidator(Type type, string propertyName)
        //{
        //    MemberInfo memberInfo = type.GetProperty(propertyName);
        //    return PropertyValidator.FromAttributes(GetAllAttributes(memberInfo), propertyName);
        //}

        //public static Attribute[] GetAllAttributes(MemberInfo member)
        //{
        //    return member.GetCustomAttributes(false).OfType<Attribute>()
        //          .Concat(MetadataHelper.GetExternalAndFluentAPIAttrbutes
        //            (member.ReflectedType, member.Name) ?? new Attribute[0])
        //        .Where(a => a is DXValidationAttribute || a is ValidationAttribute).ToArray();

        //}

        //public virtual string this[string columnName]
        //{
        //    get
        //        {

        //        try
        //        {
        //            var propertyInfo = GetType().GetProperty(columnName);
        //            return propertyInfo != null ? GetErrorText(columnName, propertyInfo.GetValue(this)) : "";
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            return null;
        //        }
        //    }
        //}

        public string Error => "";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //try
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("propertyExpression should represent access to a member");
            string memberName = memberExpr.Member.Name;
            RaisePropertyChanged(memberName);
        }
    }
}