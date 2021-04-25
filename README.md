# ASP.NET-Core-WebApi-Three-Layer-Architecture

A host is an object that encapsulates an app's resources, such as:

Dependency injection (DI)
Logging
Configuration
IHostedService implementations

The Logging property can have LogLevel and log provider properties. The LogLevel specifies the minimum level to log for selected categories. In the preceding JSON, Information and Warning log levels are specified. LogLevel indicates the severity of the log and ranges from 0 to 6:

Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5, and None = 6.