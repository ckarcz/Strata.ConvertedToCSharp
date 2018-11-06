/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ExternalEnumNames = com.opengamma.strata.collect.named.ExtendedEnum.ExternalEnumNames;

	/// <summary>
	/// Test <seealso cref="ExtendedEnum"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExtendedEnumTest
	public class ExtendedEnumTest
	{

	  public virtual void test_enum_SampleNamed()
	  {
		ExtendedEnum<SampleNamed> test = ExtendedEnum.of(typeof(SampleNamed));
		assertEquals(test.lookupAll(), ImmutableMap.builder().put("Standard", SampleNameds.STANDARD).put("STANDARD", SampleNameds.STANDARD).put("More", MoreSampleNameds.MORE).put("MORE", MoreSampleNameds.MORE).put("Other", OtherSampleNameds.OTHER).put("Another1", SampleNamedInstanceLookup1.ANOTHER1).put("ANOTHER1", SampleNamedInstanceLookup1.ANOTHER1).put("Another2", SampleNamedInstanceLookup2.ANOTHER2).put("ANOTHER2", SampleNamedInstanceLookup2.ANOTHER2).build());
		assertEquals(test.lookupAllNormalized(), ImmutableMap.builder().put("Standard", SampleNameds.STANDARD).put("More", MoreSampleNameds.MORE).put("Other", OtherSampleNameds.OTHER).put("Another1", SampleNamedInstanceLookup1.ANOTHER1).put("Another2", SampleNamedInstanceLookup2.ANOTHER2).build());
		assertEquals(test.alternateNames(), ImmutableMap.of("Alternate", "Standard", "ALTERNATE", "Standard"));
		assertEquals(test.Type, typeof(SampleNamed));
		assertEquals(test.find("Standard"), SampleNameds.STANDARD);
		assertEquals(test.find("STANDARD"), SampleNameds.STANDARD);
		assertEquals(test.find("Rubbish"), null);
		assertEquals(test.lookup("Standard"), SampleNameds.STANDARD);
		assertEquals(test.lookup("Alternate"), SampleNameds.STANDARD);
		assertEquals(test.lookup("ALTERNATE"), SampleNameds.STANDARD);
		assertEquals(test.lookup("More"), MoreSampleNameds.MORE);
		assertEquals(test.lookup("More", typeof(MoreSampleNameds)), MoreSampleNameds.MORE);
		assertEquals(test.lookup("Other"), OtherSampleNameds.OTHER);
		assertEquals(test.lookup("Other", typeof(OtherSampleNameds)), OtherSampleNameds.OTHER);
		assertEquals(test.lookup("Another1"), SampleNamedInstanceLookup1.ANOTHER1);
		assertEquals(test.lookup("Another2"), SampleNamedInstanceLookup2.ANOTHER2);
		assertThrowsIllegalArg(() => test.lookup("Rubbish"));
		assertThrowsIllegalArg(() => test.lookup(null));
		assertThrowsIllegalArg(() => test.lookup("Other", typeof(MoreSampleNameds)));
		assertEquals(test.ToString(), "ExtendedEnum[SampleNamed]");
	  }

	  public virtual void test_enum_SampleNamed_externals()
	  {
		ExtendedEnum<SampleNamed> test = ExtendedEnum.of(typeof(SampleNamed));
		assertEquals(test.externalNameGroups(), ImmutableSet.of("Foo", "Bar"));
		assertThrowsIllegalArg(() => test.externalNames("Rubbish"));
		ExternalEnumNames<SampleNamed> fooExternals = test.externalNames("Foo");
		assertEquals(fooExternals.lookup("Foo1"), SampleNameds.STANDARD);
		assertEquals(fooExternals.lookup("Foo1", typeof(SampleNamed)), SampleNameds.STANDARD);
		assertEquals(fooExternals.lookup("Foo1", typeof(SampleNamed)), SampleNameds.STANDARD);
		assertEquals(fooExternals.externalNames(), ImmutableMap.of("Foo1", "Standard"));
		assertThrowsIllegalArg(() => fooExternals.lookup("Rubbish"));
		assertThrowsIllegalArg(() => fooExternals.lookup(null));
		assertThrowsIllegalArg(() => fooExternals.lookup("Other", typeof(MoreSampleNameds)));
		assertEquals(fooExternals.ToString(), "ExternalEnumNames[SampleNamed:Foo]");

		ExternalEnumNames<SampleNamed> barExternals = test.externalNames("Bar");
		assertEquals(barExternals.lookup("Foo1"), MoreSampleNameds.MORE);
		assertEquals(barExternals.lookup("Foo2"), SampleNameds.STANDARD);
		assertEquals(barExternals.reverseLookup(MoreSampleNameds.MORE), "Foo1");
		assertEquals(barExternals.reverseLookup(SampleNameds.STANDARD), "Foo2");
		assertThrowsIllegalArg(() => barExternals.reverseLookup(OtherSampleNameds.OTHER));
		assertEquals(barExternals.externalNames(), ImmutableMap.of("Foo1", "More", "Foo2", "Standard"));
		assertEquals(barExternals.ToString(), "ExternalEnumNames[SampleNamed:Bar]");
	  }

	  public virtual void test_enum_SampleOther()
	  {
		ExtendedEnum<SampleOther> test = ExtendedEnum.of(typeof(SampleOther));
		assertEquals(test.lookupAll(), ImmutableMap.of());
		assertEquals(test.alternateNames(), ImmutableMap.of());
		assertEquals(test.externalNameGroups(), ImmutableSet.of());
		assertThrowsIllegalArg(() => test.lookup("Rubbish"));
		assertThrowsIllegalArg(() => test.lookup(null));
		assertEquals(test.ToString(), "ExtendedEnum[SampleOther]");
	  }

	  public virtual void test_enum_lenient()
	  {
		ExtendedEnum<SampleNamed> test = ExtendedEnum.of(typeof(SampleNamed));
		assertEquals(test.findLenient("Standard"), SampleNameds.STANDARD);
		assertEquals(test.findLenient("A1"), SampleNameds.STANDARD);
		assertEquals(test.findLenient("A2"), MoreSampleNameds.MORE);
	  }

	  public virtual void test_enum_invalid()
	  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		Logger logger = Logger.getLogger(typeof(ExtendedEnum).FullName);
		Level level = logger.Level;
		try
		{
		  logger.Level = Level.OFF;
		  // these return empty instances to avoid ExceptionInInitializerError
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid1)).lookupAll().Empty, true);
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid2)).lookupAll().Empty, true);
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid3)).lookupAll().Empty, true);
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid4)).lookupAll().Empty, true);
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid5)).lookupAll().Empty, true);
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid6)).lookupAll().Empty, true);
		  assertEquals(ExtendedEnum.of(typeof(SampleInvalid7)).lookupAll().Empty, true);
		}
		finally
		{
		  logger.Level = level;
		}
	  }

	}

}