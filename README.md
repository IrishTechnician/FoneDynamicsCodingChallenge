Fone Dynamics Coding Challenge
=======

Fone Dynamics Backend Development Coding Challenge

## Getting started with this Cache

The cache was not nuget packaged up, so for the purposes of this test the class library will need to be included in the resulting testing project. 


```csharp
// Code Snippet for Console Application 
static void Main()
{
	        // Construct a cache
            Cache<int, int> cd = new Cache<int, int>(10000);

            // Bombard the Cache with 10000 competing AddOrUpdates
            Parallel.For(0, 10000, i => { cd.AddOrUpdate(i, i); });

            //After 10000 AddOrUpdates cd[9999] should be 9999
            cd.TryGetValue(9999, out var value);
}
```

Code coverage is 100%
![Code Coverage](https://github.com/IrishTechnician/FoneDynamicsCodingChallenge/blob/master/CodeCoverage.png "Code Coverage")