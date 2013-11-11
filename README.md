ValueObject
===========

ValueObject is a micro library for easily creating C# classes with value semantics. The library provides an abstract base class that overrides Equals, GetHashCode, and the `==` and `!=` operators. It also implements IEquatable.

Use
------------
To create a value object, simply inherit from ValueObject. By default, two objects will be considered equal if they are the same type and all of their public properties and fields are equal.

To prevent a public property or field from being considered in memberwise comparisons, decorate it with an `IgnoreMember` attribute.

```c#
class Customer : ValueObject
{
    public string Name { get; set; }
    public int Age;

    private int ssn;  //ignored in comparisons
    
    [IgnoreMember]
    public string Address;  //also ignored because of attribute
}
```
```c#

var customer1 = new Customer{ Name = "John", Age = 13, Address = "California" };
var customer2 = new Customer{ Name = "John", Age = 13, Address = "Florida" };

Debug.Assert(customer1 == customer2);
```

