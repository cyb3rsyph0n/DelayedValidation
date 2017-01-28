namespace DelayedValidation
{
    using System;
    using System.Collections.Generic;

    public abstract class DelayedValidation
    {
        #region Properties

        protected internal bool isValid { get; private set; }

        protected internal List<string> validationExceptions { get; set; } = new List<string>();

        private List<ValidationRule> validationRules { get; } = new List<ValidationRule>();

        #endregion

        #region Public Methods and Operators

        public void AddValidationRule(Func<object[], bool> action, object[] arguments, Exception validationError)
        {
            validationRules.Add(new ValidationRule(action, arguments, validationError));
        }

        public virtual bool Validate(bool throwExceptions)
        {
            isValid = false;
            validationExceptions.Clear();

            validationRules.ForEach(
                v =>
                {
                    if (v.ValidationFunc.Invoke(v.Arguments))
                    {
                        if (throwExceptions) throw v.ValidationError;

                        validationExceptions.Add(v.ValidationError.Message);
                    }
                });

            return validationExceptions.Count == 0;
        }

        #endregion

        public class ValidationRule
        {
            #region Constructors and Destructors

            public ValidationRule(Func<object[], bool> validationFunc, object[] arguments, Exception validationError)
            {
                ValidationFunc = validationFunc;
                Arguments = arguments;
                ValidationError = validationError;
            }

            #endregion

            #region Public Properties

            public object[] Arguments { get; }

            public Exception ValidationError { get; }

            public Func<object[], bool> ValidationFunc { get; }

            #endregion
        }
    }
}