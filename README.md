The solution consists of 3 projects:
AsposTest - target class library; 
Tests - Nunit test projects;
ConsoleAppTest - dummy console project with an example of usage of AsposeTestClassLibrary.

Workers collection is wrapped into DataContext class in order to the possibility of substitution with real database data context.
DataContext defined as a singleton to avoid duplications of data contexts.
Operations with DataContext are thread-safe on the repository level
Besides workers collection, there is a subordinates collection, the purpose of which is a quick search for subordinates.
For a complete list of subordinates a function runs recursively so StackOverflow exception may be thrown if worker's hierarchy is very deep.   

Calculation of wages is implemented with the decorator pattern. I don't insist that it was necessary, but the pattern is suitable for this purpose. 

Calculated workers wages are stored in a custom generic cache. The cache resets every request.
Due to the cache usage the application uses a bit more memory but not significantly.

Known issues:
there is no tracking of the history of hiring and firing workers so calculation for past dates may be inaccurate.
No check of constraint that employee can't have subordinates.
There is no logging in the code.
Internal implementation is exposed as public because I use interfaces and interfaces require public methods and properties.

Performance test:
on a sample from ConsoleAppTest TotalSum calculation takes 9.7 seconds and 45Mb of memory on my machine. 
(insertion of 35 000 workers, 20 parallel calculations of total company wages).