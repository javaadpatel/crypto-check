## Technical Questions

---

1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.

   I spent 22 hours on the assignment.

2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

   The latest version of c# is version 9, I am currently reading a book called "C# 9 in depth" to learn more about it. The Azure Functions runtime is currently limited to .Net Core 3.1 so the latest usable C# version is 8. In C# 8, the most useful feature has been async streams.

3. How would you track down a performance issue in production? Have you ever had to do this?

   In production, I would go to the monitoring tool, for example in Azure this would be Application Insights. I would then use it to identify which requests were slow and the scenario's for which they occur (eg. specific time of day or specific request parameters). Application insights shows you the execution time of your code as well as highlighting hotspots for performance and it shows execution time of external calls (eg. database calls). If its an external call like a database call that is slow then I would go to that databases metrics and see if its being overloaded, if not then maybe its related to the way we're querying it or the amount of data being retrieved, these could lead to changes to queries or changes to caching strategies for instance. If the slow performance was coming from inside the code base then I would run the code locally with a profiler while replicating the scenerio to isolate the poorly performing code.

   I have been responsible for maintaining production systems and have had to do this exact thing either proactively because of alerts going off or reactively because of user's queries.

4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

5. What do you think about this technical assessment?

   I really enjoyed this assessment. I liked how open ended it was and that it gave me a lot to think about when designing the solution in terms of performance, security, scalability, user experience and design.

6. Please, describe yourself using JSON

```
{
    "name": "Javaad",
    "surname": "Patel",
    "occupation": "Developer",
    "interests": [
        "Motorcycle racing and maintenance",
        "Fantasy books"
    ],
    "favoriteQuotes": [
        "I never lose. I either win or learn - Nelson Mandela",
        "To be yourself in a world that is constantly trying to make you something else is the greatest accomplishment - Ralph Waldo Emerson"
    ]
}
```
