### Fixing Aerospike CLS non compliance error

```csharp
// non Common Language Specification (CLS) compliant code example
public readonly Policy readPolicyDefault;
public Policy ReadPolicyDefault{get {return readPolicyDefault;}};

// Fix is to change: public readonly Policy readPolicyDefaultProp;
```

[My pull request](https://github.com/aerospike/aerospike-client-csharp/pull/47)
