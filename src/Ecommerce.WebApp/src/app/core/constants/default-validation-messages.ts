type ErrorRegistry = { [key in string]: (...args:any) => string };

export type ErrorHandler = {
  global?:ErrorRegistry,
  field?:ErrorRegistry
}

const DEFAULT_VALIDATION_MESSAGES: ErrorHandler = {
  field:{
      "required": () => 'Field is required',
      "minlength": ({ requiredLength }) => `The field a least need to be ${requiredLength} characters long`,
      "maxlength":({ requiredLength }) =>`The field a least need to be no greater than ${requiredLength} characters long`
  }
}

export { DEFAULT_VALIDATION_MESSAGES };

