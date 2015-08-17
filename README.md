# Ntegrity
A library of tools for tracking and controlling changes to your .NET assembly's public interface!

Sample output:
```
This is a human readable representation of the interface for the assembly:
Assembly: Ntegrity.TestTargetAssembly
Version:  v1.0.0.0
Targeting CLR version: v4.0.30319

CLASSES: 
	public class Ntegrity.TestTargetAssembly.ContainerClass
	CONSTRUCTORS:
		public Void .ctor()

	internal abstract class Ntegrity.TestTargetAssembly.InternalAbstractClass

	internal class Ntegrity.TestTargetAssembly.InternalClass
	CONSTRUCTORS:
		public Void .ctor()

	public class Ntegrity.TestTargetAssembly.PublicClass
	CONSTRUCTORS:
		public Void .ctor()

	[Ntegrity.TestTargetAssembly.TestAttributeAttribute]
	internal class Ntegrity.TestTargetAssembly.PublicClassWithAttributes
	CONSTRUCTORS:
		public Void .ctor()

	public sealed class Ntegrity.TestTargetAssembly.PublicSealedClass
	CONSTRUCTORS:
		public Void .ctor()

	public static class Ntegrity.TestTargetAssembly.StaticClass

	[System.AttributeUsageAttribute]
	public class Ntegrity.TestTargetAssembly.TestAttributeAttribute
	CONSTRUCTORS:
		public Void .ctor(System.String)

	private class Ntegrity.TestTargetAssembly.ContainerClass+NestedPrivateClass
	CONSTRUCTORS:
		public Void .ctor()

	protected class Ntegrity.TestTargetAssembly.ContainerClass+NestedProtectedClass
	CONSTRUCTORS:
		public Void .ctor()


INTERFACES: 
	public interface Ntegrity.TestTargetAssembly.IPublicInterface


ENUMS: 
	public enum Ntegrity.TestTargetAssembly.PublicEnum

	private enum Ntegrity.TestTargetAssembly.ContainerClass+NestedPrivateEnum

	protected enum Ntegrity.TestTargetAssembly.ContainerClass+NestedProtectedEnum


STRUCTS: 
	public struct Ntegrity.TestTargetAssembly.PublicStruct

	private struct Ntegrity.TestTargetAssembly.ContainerClass+NestedPrivateStruct

	protected struct Ntegrity.TestTargetAssembly.ContainerClass+NestedProtectedStruct
```
