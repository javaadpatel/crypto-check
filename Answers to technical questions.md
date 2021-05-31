## Technical Questions

---

1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.

   I spent a total of 22 hours on the assignment. There are a number of improvements that I would make to my solution, namely:

   - Performance:

     - I implemented a basic caching layer and only applied it to one of the API calls (get all cryptocurrencies). I would apply the caching to all API calls and also enhance the cache service to take a cache validation function, which could validate whether the cached item should be refreshed or not. Additionally, the service could be enhanced to receive the cache options (expirations).
     - Rate limiting either from the client side or within the API to prevent users from spamming the service and taking it offline.
     - Serving the React front-end from a CDN.

   - Security:

     - Storing configuration secrets in Azure Key vault and referencing it from there.
     - Applying more specific CORS rules
     - Securing the API with jwt tokens, this would require a login mechanism such as Auth0 to be implemented on the front-end. This would allow only authorized users to call the API and additionally allow for customized responses based on the user's account tier (eg. free users get quotes cached less than 5 minutes ago, paid users get real-time quotes)

   - User Experience:

     - Implement a dropdown type search for the user input, this could use the basic search endpoint that I created and provide the user with a list of cryptocurrencies to choose from. They could then type either the symbol or the name and know whether we support it.
     - Keep a history of quotes on the front-end for a user to quickly see quotes that they've requested in the past.

   - Developer Experience:
     - Create a CI/CD pipeline with the four stages:
       - Infrastructure deployment using Terraform
       - Build and Test
       - Deployment
       - Integration Testing
     - Architecture diagram highlighting the main components of the solution

2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

   The latest version of c# is version 9, I am currently reading a book called "C# 9 in depth" to learn more about it. The Azure Functions runtime is currently limited to .Net Core 3.1 so the latest usable C# version is 8. In C# 8, the most useful feature has been async streams.
   ????

3. How would you track down a performance issue in production? Have you ever had to do this?

   In production, I would go to the monitoring tool, for example in Azure this would be Application Insights. I would then use it to identify which requests were slow and the scenario's for which they occur (eg. specific time of day or specific request parameters). Application insights shows you the execution time of your code as well as highlighting hotspots for performance and it shows execution time of external calls (eg. database calls). If its an external call like a database call that is slow then I would go to that databases metrics and see if its being overloaded, if not then maybe its related to the way we're querying it or the amount of data being retrieved, these could lead to changes to queries or changes to caching strategies for instance. If the slow performance was coming from inside the code base then I would run the code locally with a profiler while replicating the scenerio to isolate the poorly performing code.

   I have been responsible for maintaining production systems and have had to do this exact thing either proactively because of alerts going off or reactively because of user's queries.

4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

   The latest technical book I've read was `The Manager's Path`, from which I learned a lot about the responsibilities and challenges of engineers going down a management path. You can find my learnings about the tech lead position [here](https://javaadpatel.com/what-is-a-tech-lead/).

   The other technical book I'm currently reading is `System Design Interview`, the book is a great source of information on how to design different systems. The chapter that I've found most insightful so far was how to design a distributed Key-Value store, from which I learned more about distributed systems such as the CAP theorem, quorum consensus and consistency modes and when to use them.

   The last tech conference I attended was GrafanaCon, at which I learned more advanced uses of Grafana and building more actionable dashboards.

   I also consume a lot of knowledge through online courses (currently doing `Rest APIs with Flask and Python` to learn Python for DevOps related work) and blog posts.

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
