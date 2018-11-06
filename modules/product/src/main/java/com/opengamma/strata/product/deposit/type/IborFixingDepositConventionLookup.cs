using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
{

	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Allows conventions to be created dynamically from an index name.
	/// </summary>
	internal sealed class IborFixingDepositConventionLookup : NamedLookup<IborFixingDepositConvention>
	{

	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly IborFixingDepositConventionLookup INSTANCE = new IborFixingDepositConventionLookup();

	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ConcurrentMap<string, IborFixingDepositConvention> BY_NAME = new ConcurrentDictionary<string, IborFixingDepositConvention>();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IborFixingDepositConventionLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override IborFixingDepositConvention lookup(string name)
	  {
		IborFixingDepositConvention value = BY_NAME.get(name);
		if (value == null)
		{
		  IborFixingDepositConvention created = createByName(name);
		  if (created != null)
		  {
			string correctName = created.Name;
			value = BY_NAME.computeIfAbsent(correctName, k => created);
			BY_NAME.putIfAbsent(correctName.ToUpper(Locale.ENGLISH), value);
		  }
		}
		return value;
	  }

	  public IDictionary<string, IborFixingDepositConvention> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static IborFixingDepositConvention createByName(string name)
	  {
		return IborIndex.extendedEnum().find(name).map(index => ImmutableIborFixingDepositConvention.of(index)).orElse(null);
	  }

	}

}