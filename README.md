The solution consists on 3 projects:
AsposTest - aim class library 
Tests - Nunit test projects
ConsoleAppTest - dummy console project with example of usage of AsposeTestClassLibrary.

workers collection is wrapped into DataContext class in order to possibility of substitution with real database data context.
workers collection defined as singleton to avoid duplications of data contexts

calculation of wages is implemented with decorator pattern. I don't insists that it was necessery, but the pattern is suitable for this purpose. 