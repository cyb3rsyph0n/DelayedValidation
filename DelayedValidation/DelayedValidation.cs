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
        #region Fields

        private bool isDirty;

        private List<ValidationRule> validationRules = new List<ValidationRule>();

        #endregion

        #region Properties

        protected internal List<string> ValidationExceptions { get; } = new List<string>();

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
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (validationError == null) throw new ArgumentNullException(nameof(validationError));
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

            return ValidationExceptions.AsReadOnly();
        }

        /// <summary>
        ///     Handles the class validation but can be overridden if needed
        /// </summary>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public virtual bool Validate(bool throwExceptions = true)
        {
            //IF INHERITS IDRAFTABLE AND IS A DRAFT THEN DO NOT THROW ERRORS REGARDLESS OF throwException PARAM
            var isDraft = GetType().GetInterface(nameof(IDraftable)) != null && ((IDraftable)this).IsDraft;

            //CLEAR OUT THE LIST OF EXCEPTIONS
            ValidationExceptions.Clear();

            validationRules.ForEach(
                v =>
                {
                    //USE A TRY IN CASE WE'RE NOT SUPPOSED TO THROW ERRORS WE CAN CONTAIN THEM AND TRACK THEM AS VALIDATION ERRORS
                    try
                    {
                        if (v.ValidationFunc.Invoke(v.Arguments)) return;
                        if (throwExceptions && !isDraft) throw v.ValidationError;

                        ValidationExceptions.Add(v.ValidationError.Message);
                    }
                    catch (Exception e)
                    {
                        if (throwExceptions && !isDraft) throw e;

                        ValidationExceptions.Add(e.Message);
                    }
                });

            //RETURN TRUE IF THERE WERE NO ERRORS OR FALSE IF SOMETHING FAILED AND UPDATE THE isDirty FLAG
            return !(isDirty = ValidationExceptions.Count != 0);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles retrieving the value but first running the validate if we're dirty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        protected internal T GetField<T>(T field)
        {
            if (isDirty) Validate();
            return field;
        }

        /// <summary>
        ///     Handles setting the dirty flag and updating the property value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <param name="enforceValidation"></param>
        protected internal void SetField<T>(
            ref T field,
            T value,
            bool validate = false,
            bool enforceValidation = false)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            isDirty = true;
            field = value;

            //IF VALIDATE IS TRUE THEN RUN VALIDATION DURING THE SETTER
            if (validate) Validate(enforceValidation);
        }

        #endregion

        /// <summary>
        ///     Class to contain the rule and the properties for validation
        /// </summary>
        public class ValidationRule
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Constructor for creating a new validation rule
            /// </summary>
            /// <param name="validationFunc"></param>
            /// <param name="arguments"></param>
            /// <param name="validationError"></param>
            public ValidationRule(Func<object[], bool> validationFunc, object[] arguments, Exception validationError)
            {
                if (validationFunc == null) throw new ArgumentNullException(nameof(validationFunc));
                if (validationError == null) throw new ArgumentNullException(nameof(validationError));

                ValidationFunc = validationFunc;
                Arguments = arguments;
                ValidationError = validationError;
            }

            #endregion

            #region Properties

            /// <summary>
            ///     Arguments that should be passed into validation method
            /// </summary>
            internal object[] Arguments { get; }

            /// <summary>
            ///     Error that should be thrown in the event of a failure
            /// </summary>
            internal Exception ValidationError { get; }

            /// <summary>
            ///     Method to be called that will run the validation
            /// </summary>
            internal Func<object[], bool> ValidationFunc { get; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Override equals to test if the functions passed in are equal
            /// </summary>
            /// <param name="obj">compared object</param>
            /// <returns>true if the objects are the same</returns>
            public override bool Equals(object obj)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));
                var v = obj as ValidationRule;

                return ValidationFunc.Equals(v.ValidationFunc);
            }

            /// <summary>
            ///     override to return the hascode of the validation function
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return ValidationFunc.GetHashCode();
            }

            #endregion
        }
    }
}