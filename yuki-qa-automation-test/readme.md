Yuki QA Automation Test.

The goal of this test is to create a basic setup for an end to end tests solution using C#, Nunit/Xunit and Selenium WebDriver.

what content you will find?
- yuki-qa-automation-tests: the folder containing the solution with the (empty) tests projects you'll work on (.net core)
- yuki-qa-automation: the folder containing a .net core solution of the web application to create tests for

what you're expected to do:

1) run and analyse the suggsestions in the web app
2) open up the tests solution and setup the basic code to:
 a) configure selenium and install the dependencies required for it to work
 b) setup the basic design for a reusable tests solution in C# (either Nunit/Xunit are valid frameworks to use)
 c) implement (at least) the cases described in the webapp page, taking into account the following aspects:
 - general coding best practices/patterns
 - automation testing best practices/paterns
 - performance (eg. execution in a pipeline, network latency, website response time delays, etc)
 - writting the code in a way it will keep working performantly in unnattended contexts (eg. a CI/CD pipeline)