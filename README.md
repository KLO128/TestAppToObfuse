[LoGiC.NET](https://github.com/AnErrupTion/LoGiC.NET) obfuscator looks good, but I have found some issues with renaming...

## My Changes in LoGiC.NET
I had to made some **cosmetic changes** in LoGiC.NET for my needs.

1. I have excluded `InvalidMetadata` protection, because IIS server does not support such protected libraries with invalid metadata.
   ``` CSharp
   Protection[] protections = new Protection[]
        {
                new Renamer(),
                new AntiTamper(),
                new JunkDefs(),
                new StringEncryption(),
                new AntiDe4dot(),
                new ControlFlow(),
                new IntEncoding(),
                new ProxyAdder(),
                // new InvalidMetadata()
        };
   ```
2. Renamer: I have commented out renaming of namespaces and data types (classes, interfaces, structs, delegates, enums,...)
   ``` CSharp
   //if (CanRename(type))
   //{
   //    // Hide namespace
   //    type.Namespace = string.Empty;
   //    type.Name = Randomizer.String(MemberRenamer.StringLength());
   //}
   ``` 
3. Renamer:  I have commented out renaming of instance fields, because it did not work - it renamed some fields and then called the original name...
   ``` CSharp
   public static bool CanRename(object obj)
   {
       DefAnalyzer analyze;
       if (obj is MethodDef method) analyze = new MethodDefAnalyzer();
       else if (obj is PropertyDef prop) analyze = new PropertyDefAnalyzer();
       else if (obj is EventDef) analyze = new EventDefAnalyzer();
       else if (obj is FieldDef) return false;
       else if (obj is Parameter) analyze = new ParameterAnalyzer();
       else if (obj is TypeDef) return false;
       else return false;

       return analyze.Execute(obj);
   }
   ```
4. Configuration: I had to exclude ProxyCallsIntensity due to stack overflowing...
   ```
   ProxyCallsIntensity: 0
   ```
## My Project Issue
**In my project, I came to the exception of calling an internal method with the original name in the same class's public constructor, but the declaration of the method was renamed after the obfuscation!!! :-(**

- I think it could be caused by not supported version of .NET (.NET 6).
- ...or there must be some issue with the privacy: The constructor's public modifier, where it is called, **is not the privacy modifier of the called method.** I think, that program attaches wrong metadata of privacy to this method in some code branch, and it assumes it is public, so it does not rename it when calling it.

``` CSharp
public MyService(IDependency dependency) : base(dependency)
{
    InitData();
}
...
// original InitData()
internal void rgrehrehrehzhwefwegweh()
{
}
```

**Unfortunately, I could not reproduce this issue creating a new sample project (TestAppToObfuse), but I have found a similar one...**

## Sample Project Issue 1
- I have created a new sample project to reproduce the renaming issue and I have explored another issue.
- Reproduction of the issue was not successful: similar case of the declaration and the call was renamed correctly, but it calls a property setter helper method called **set_PreviousResult()**, which does not exist in the original neither obfuscated code.
- **PreviousResult** property only exists and I have used the old syntax of getter and setter (if it matters).

### Steps to Reproduce
1. You can **download** the github project [TestAppToObfuse](https://github.com/KLO128/TestAppToObfuse).
2. Obfuscate the libraries TestAppToObfuse.Services and TestAppToObfuse.Services.Impl
3. Copy the result libraries' DLLs into the Libs folder...
4. ...and run TestAppToObfuse.exe, you will get the exception:
   ```
   Method not found: 'Void TestAppToObfuse.Services.SomeServiceBase.set_PreviousResult(System.String)`
   ```

## Sample Project Small Issue 2
As you can see after disassembling, protected **non-internal** method `DoItInner(string what)` of `SomeService` is renamed - it should be not in my opinion, because it is visible outside the assembly as it is protected method in a public class. I know, that it is not a public well known interface, but some class could count with the original name as it would like to inherit it.

> Many thanks for any help to solve these issues! (Specially My Project Issue or Sample Project Issue 1).
> 