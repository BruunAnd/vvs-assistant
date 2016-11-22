using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.ViewModels.MVVM
{
    public class ViewModelBase : ObservableObject, IDataErrorInfo
    {
        public string this[string propName]
        {
            get
            {
                return ValidateProperty(propName);
            }
        }

        /// <summary>
        /// Validates a property based on its DataAnnotations
        /// </summary>
        /// <param name="Property Name"></param>
        /// <returns> Error Message or null if the property data is valid </returns>
        protected string ValidateProperty([CallerMemberName] string propName = null)
        {
            ValidationContext context = new ValidationContext(this, null, null);
            context.MemberName = propName;
            var errors = new List<ValidationResult>();
            if (Validator.TryValidateObject(this, context, errors, true))
                return null;
            else
            {
                var validationError = errors.SingleOrDefault(error => error.MemberNames
                                            .Any(member => member == propName));
                return validationError?.ErrorMessage;
            }
        }

        public string Error
        {
            get
            {
                //TODO: Implement whatever this is supposed to do
                return "No error";
            }
        }
    }
}
