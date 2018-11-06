/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Test <seealso cref="ScenarioPerturbation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScenarioPerturbationTest
	public class ScenarioPerturbationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test_none()
	  {
		ScenarioPerturbation<double> test = ScenarioPerturbation.none();
		assertEquals(test.ScenarioCount, 1);
		MarketDataBox<double> box1 = MarketDataBox.ofScenarioValues(1d, 2d, 3d);
		assertEquals(test.applyTo(box1, REF_DATA), box1);
		MarketDataBox<double> box2 = MarketDataBox.ofSingleValue(1d);
		assertEquals(test.applyTo(box2, REF_DATA), box2);
	  }

	//  public void test_generics() {
	//    // Number perturbation should be able to alter a Double box, returning a Number box
	//    ScenarioPerturbation<Number> test = ScenarioPerturbation.none();
	//    assertEquals(test.getScenarioCount(), 1);
	//    MarketDataBox<Double> box = MarketDataBox.ofScenarioValues(1d, 2d, 3d);
	//    MarketDataBox<Number> perturbed = test.applyTo(box);
	//    assertEquals(perturbed, box);
	//  }

	  public virtual void coverage()
	  {
		ScenarioPerturbation<double> test = ScenarioPerturbation.none();
		coverImmutableBean((ImmutableBean) test);
	  }

	}

}