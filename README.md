# DelayedValidation

 This is a sample project to allow an object to support delayed validation. This should assist with properties that are dependant on one another but set through two separate methods.

## Usage

 Inherit from the base class DelayedValidation and add Validate(true) to any property read methods. Rules should be added through the setters with AddDelayedValidationRule. Optionally you can call Validate(false) through the setter to force validation of the rule and potentially speed up any future reads that would trigger validation.

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## History

 This is still a work in progress. If you set a property multiple times it will add the validation rule over and over. On simple properties this is not an issue but if each rule were to call a website or remote API the turn around time for calling over and over again could get expensive.
