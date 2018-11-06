using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Mutable builder for building instances of <seealso cref="PointShifts"/>.
	/// <para>
	/// This is created via <seealso cref="PointShifts#builder(ShiftType)"/>.
	/// </para>
	/// </summary>
	public sealed class PointShiftsBuilder
	{

	  /// <summary>
	  /// The type of shift to apply to the rates.
	  /// </summary>
	  private readonly ShiftType shiftType;
	  /// <summary>
	  /// The shift amounts, keyed by the identifier of the node to which they should be applied.
	  /// <para>
	  /// This is a linked map in order to preserve the insertion order. This means the node identifiers
	  /// will appear in the same order as the nodes, assuming the shifts are added in node order (which seems
	  /// likely).
	  /// </para>
	  /// </summary>
	  private readonly IDictionary<Pair<int, object>, double> shifts = new LinkedHashMap<Pair<int, object>, double>();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor used by {@code PointShifts.builder}.
	  /// </summary>
	  /// <param name="shiftType">  the type of shift to apply to the rates </param>
	  internal PointShiftsBuilder(ShiftType shiftType)
	  {
		this.shiftType = ArgChecker.notNull(shiftType, "shiftType");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a shift for a parameter to the builder.
	  /// </summary>
	  /// <param name="scenarioIndex">  the index of the scenario containing the shift </param>
	  /// <param name="nodeIdentifier">  the identifier of the node to which the shift should be applied </param>
	  /// <param name="shiftAmount">  the size of the shift </param>
	  /// <returns> this builder </returns>
	  public PointShiftsBuilder addShift(int scenarioIndex, object nodeIdentifier, double shiftAmount)
	  {

		ArgChecker.notNull(nodeIdentifier, "nodeIdentifier");
		ArgChecker.notNegative(scenarioIndex, "scenarioIndex");
		shifts[Pair.of(scenarioIndex, nodeIdentifier)] = shiftAmount;
		return this;
	  }

	  /// <summary>
	  /// Adds multiple shifts to the builder.
	  /// </summary>
	  /// <param name="scenarioIndex">  the index of the scenario containing the shifts </param>
	  /// <param name="shiftMap">  the shift amounts, keyed by the identifier of the node to which they should be applied </param>
	  /// <returns> this builder </returns>
	  public PointShiftsBuilder addShifts<T1>(int scenarioIndex, IDictionary<T1> shiftMap)
	  {
		ArgChecker.notNull(shiftMap, "shiftMap");
		ArgChecker.notNegative(scenarioIndex, "scenarioIndex");
		MapStream.of(shiftMap).forEach((id, shift) => shifts.put(Pair.of(scenarioIndex, id), shift));
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance of <seealso cref="PointShifts"/> built from the data in this builder.
	  /// </summary>
	  /// <returns> an instance of <seealso cref="PointShifts"/> built from the data in this builder </returns>
	  public PointShifts build()
	  {
		// This finds the scenario count by finding the maximum index and adding 1.
		// If OptionalInt had map() it could be written more sensibly as: ...max().map(i -> i + 1).orElse(0)
		// but it doesn't, hence using -1 and adding 1 to it for the case of zero scenarios
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		int scenarioCount = shifts.Keys.Select(Pair::getFirst).Max().orElse(-1) + 1;
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<object> nodeIdentifiers = shifts.Keys.Select(Pair::getSecond).Distinct().collect(toImmutableList());
		DoubleMatrix shiftMatrix = DoubleMatrix.of(scenarioCount, nodeIdentifiers.Count, (r, c) => shiftValue(r, nodeIdentifiers[c]));
		return new PointShifts(shiftType, shiftMatrix, nodeIdentifiers);
	  }

	  private double shiftValue(int scenarioIndex, object nodeIdentifier)
	  {
		double? shift = shifts[Pair.of(scenarioIndex, nodeIdentifier)];
		return shift != null ? shift.Value : 0;
	  }
	}

}