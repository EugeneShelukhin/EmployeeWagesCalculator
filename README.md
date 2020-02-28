The solution consists of 3 projects:
AsposTest - target class library 
Tests - Nunit test projects
ConsoleAppTest - dummy console project with an example of usage of AsposeTestClassLibrary.

worker's collection is wrapped into DataContext class in order to the possibility of substitution with real database data context.
DataContext defined as a singleton to avoid duplications of data contexts
operations with DataContext are thread-safe on the repository level (with readwritelockslim)

calculation of wages is implemented with the decorator pattern. I don't insist that it was necessary, but the pattern is suitable for this purpose. 

calculated workers wages are stored in a custom generic cache as well as previously founded subordinates. the cache 
TODO: there is an assumption that it will work faster if use an ordered collection of subordinates instead of the cache of founded subordinates.
Due to the cache usage the application uses a bit more memory but not significantly.

known issues:
there is no tracking of the history of hiring and firing workers so calculation for past dates may be inaccurate
no check of constraint that employee can't have subordinates
there is no logging in the code
internal implementation is exposed as public because I use interfaces and interfaces require public methods and properties


performance check:
on a sample from ConsoleAppTest TotalSum calculation takes 5.5seconds and 28Mb of memory. (12 cores cpu)