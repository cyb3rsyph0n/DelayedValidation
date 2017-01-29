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

## Todo

 Look at injecting the Update(true) into property getters

## History

 This is still a work in progress. If you set a property multiple times it will add the validation rule over and over. On simple properties this is not an issue but if each rule were to call a website or remote API the turn around time for calling over and over again could get expensive.

## License

MIT License

Copyright (c) 2017 CyberSyphon, LLC.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
