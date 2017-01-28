namespace DelayedValidation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Class to assist with delayed validation of domain objects that must "always" be valid but have circular
    ///     dependencies on properties
    /// </summary>
    public abstract class DelayedValidation
    {
        #region Properties

        protected internal List<string> validationExceptions { get; } = new List<string>();

        private List<ValidationRule> validationRules { get; } = new List<ValidationRule>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Queues a validation rule for checking before a read can occur
        /// </summary>
        /// <param name="action"></param>
        /// <param name="arguments"></param>
        /// <param name="validationError"></param>
        public void AddDelayedValidationRule(
            Func<object[], bool> action,
            object[] arguments,
            Exception validationError)
        {
            var newRule = new ValidationRule(action, arguments, validationError);

            //TODO: VALIDATION RULES DUPLICATE IF YOU CALL ADD OVER AND OVER
            if (!validationRules.Contains(newRule)) validationRules.Add(newRule);
        }

        /// <summary>
        ///     Get a list of validation errors that have occured
        /// </summary>
        /// <returns></returns>
        public IList<string> GetValidationErrors()
        {
            Validate(false);

            return validationExceptions.AsReadOnly();
        }

        /// <summary>
        ///     Handles the class validation but can be overridden if needed
        /// </summary>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public virtual bool Validate(bool throwExceptions)
        {
            //IF INHERITS IDRAFTABLE AND IS A DRAFT THEN DO NOT THROW ERRORS REGARDLESS OF throwException PARAM
            var isDraft = GetType()
                              .GetInterface(nameof(IDraftable)) != null && ((IDraftable)this).isDraft;

            //CLEAR OUT THE LIST OF EXCEPTIONS
            validationExceptions.Clear();

            validationRules.ForEach(
                v =>
                {
                    if (v.ValidationFunc.Invoke(v.Arguments)) return;
                    if (throwExceptions && !isDraft) throw v.ValidationError;

                    validationExceptions.Add(v.ValidationError.Message);
                });

            //IF EVERYTHING PASSES VALIDATION THEN WE DO NOT NEED TO KEEP RE-VALIDATING THE SAME RULES
            if (validationExceptions.Count == 0) validationRules.Clear();

            //RETURN TRUE IF THERE WERE NO ERRORS OR FALSE IF SOMETHING FAILED
            return validationExceptions.Count == 0;
        }

        #endregion

        /// <summary>
        ///     Class to contain the rule and the properties for validation
        /// </summary>
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

            /// <summary>
            ///     Arguments that should be passed into validation method
            /// </summary>
            public object[] Arguments { get; }

            /// <summary>
            ///     Error that should be thrown in the event of a failure
            /// </summary>
            public Exception ValidationError { get; }

            /// <summary>
            ///     Method to be called that will run the validation
            /// </summary>
            public Func<object[], bool> ValidationFunc { get; }

            #endregion
        }
    }
}